using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhatCD
{
    internal class Http
    {

        /// <summary>
        /// Http request method.
        /// </summary>
        private enum RequestMethod
        {
            GET,
            POST
        }

        /// <summary>
        /// Base URI for all communication with the WhatStatus server.
        /// </summary>
        public Uri BaseWhatStatusUri
        {
            get { return this.baseWhatStatusUri; }
            set { this.baseWhatStatusUri = value; }
        }
        private Uri baseWhatStatusUri = new Uri("https://whatstatus.info");

        /// <summary>
        /// Base URI for all communication with the WhatCD server.
        /// </summary>
        public Uri BaseWhatCDUri
        {
            get { return this.baseWhatCDUri; }
            set { this.baseWhatCDUri = value; }
        }
        private Uri baseWhatCDUri = new Uri("https://what.cd");

        /// <summary>
        /// Container for WhatCD cookies. Required to create and maintain a logged-on session.
        /// </summary>
        public CookieContainer CookieJar
        {
            get { return this.cookieJar; }
            private set { this.cookieJar = value; }
        }
        private CookieContainer cookieJar = new CookieContainer();

        /// <summary>
        /// Static flag to indicate if threads must wait before making any more requests to the WhatCD server.
        /// </summary>
        private static bool CountdownInProgress;

        /// <summary>
        /// Static lock object used with all Http requests to ensure multiple threads adhere to the designated minimum query delay for communicating with WhatCD servers.
        /// </summary>
        private static object HttpLocker = new object();

        /// <summary>
        /// Attempts to authenticate to WhatCD server and creates a new logged-on session.
        /// </summary>
        /// <param name="username">WhatCD username.</param>
        /// <param name="password">WhatCD password.</param>
        public Http(string username, string password)
        {
            this.Login(username, password);
        }

        /// <summary>
        /// Performs a JSON request. 
        /// </summary>
        /// <param name="uri">Base URI.</param>
        /// <param name="query">Request arguments (appended to the base URI).</param>
        /// <returns>Raw Json response.</returns>
        public string RequestJson(Uri uri, string query)
        {
            using (var reader = new StreamReader(this.PerformWebRequest(new Uri(uri, query), RequestMethod.GET, "application/json; charset=utf-8", true)))
            {
                var json = reader.ReadToEnd();
                ValidateJsonException(json);
                return json;
            }
        }

        /// <summary>
        /// Performs a binary request. 
        /// </summary>
        /// <param name="uri">Base URI.</param>
        /// <param name="query">Request arguments (appended to the base URI).</param>
        /// <returns>Raw binary response.</returns>
        public byte[] RequestBytes(Uri uri, string query)
        {
            using (var stream = this.PerformWebRequest(new Uri(uri, query), RequestMethod.GET, "application/json; charset=utf-8", true))
            {
                var result = new MemoryStream();
                var buffer = new byte[4096];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    result.Write(buffer, 0, read);
                }
                return result.ToArray();
            }
        }

        /// <summary>
        /// Determines the rip log score out of 100.
        /// </summary>
        /// <param name="log">Log file contents.</param>
        /// <param name="authKey">WhatCD users' authentication key.</param>
        /// <returns>Log score out of 100.</returns>
        public int GetFlacLogScore(string log, string authKey)
        {
            var postData = string.Format("action=takeupload&auth={0}&log_contents={1}", authKey, HttpUtility.UrlEncode(log));

            using (var stream = new StreamReader(this.PerformWebRequest(new Uri(this.BaseWhatCDUri, "logchecker.php"), RequestMethod.POST, "application/x-www-form-urlencoded", true)))
            {
                return ExtractLogScore(stream.ReadToEnd());
            }
        }

        /// <summary>
        /// Logs off the current What.CD session.
        /// </summary>
        /// <param name="authKey">WhatCD users' authentication key.</param>
        public void Logoff(string authKey)
        {
            this.PerformWebRequest(new Uri(this.BaseWhatCDUri, string.Format("logout.php?auth={0}", authKey)), RequestMethod.POST, "application/x-www-form-urlencoded", true);
        }

        /// <summary>
        /// Logs a user in to What.CD and stores session cookies.
        /// </summary>
        /// <param name="username">What.CD account username.</param>
        /// <param name="password">What.CD account password.</param>
        private void Login(string username, string password)
        {
            this.PerformWebRequest(new Uri(this.BaseWhatCDUri, "login.php"), RequestMethod.POST, "application/x-www-form-urlencoded", true, string.Format("username={0}&password={1}", Uri.EscapeDataString(username), Uri.EscapeDataString(password)));
        }

        /// <summary>
        /// Performs a web request.
        /// </summary>
        /// <param name="uri">Uri to query, including any arguments.</param>
        /// <param name="requestMethod">Type of request method.</param>
        /// <param name="contentType">Type of content.</param>
        /// <param name="useCookieJar">Flag to indicate if WhatCD cookie store should be included in the request. Do not use cookies when querying WhatStatus.</param>
        /// <param name="dataToPost">Data to post with RequestMethod == "POST".</param>
        /// <returns>Response Stream object.</returns>
        private Stream PerformWebRequest(Uri uri, RequestMethod requestMethod, string contentType, bool useCookieJar, string dataToPost = null)
        {
            HttpWebResponse response = null;

            try
            {
                Monitor.TryEnter(HttpLocker);
                WaitUntilCountdownComplete();

                var request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = requestMethod.ToString();
                request.ContentType = contentType;
                if (useCookieJar) request.CookieContainer = this.CookieJar;

                switch (requestMethod)
                {
                    case RequestMethod.GET:
                        response = request.GetResponse() as HttpWebResponse;
                        return response.GetResponseStream();

                    case RequestMethod.POST:
                        using (var writer = new StreamWriter(request.GetRequestStream()))
                        {
                            writer.Write(dataToPost);
                        }
                        response = request.GetResponse() as HttpWebResponse;
                        return response.GetResponseStream();

                    default:
                        throw new WebException(string.Format("Unknown RequestMethod: {0}", requestMethod.ToString()));
                }
            }
            finally
            {
                if (response.StatusCode != HttpStatusCode.OK) throw new WebException(string.Format("Non-OK HTTP status returned. Status code {0}: {1}", response.StatusCode, response.StatusDescription));
                // TODO: Should response be copied to another stream and then .Close() prior to exiting the method?
                //if (response != null) response.Close();
                ActivateCountdown();
                Monitor.Exit(HttpLocker);
            }
        }

        /// <summary>
        /// Raises a WebException if the response json is of the standard WhatCD json error format.
        /// </summary>
        /// <param name="json">Json returned from WhatCD server</param>
        private static void ValidateJsonException(string json)
        {
            var error = new Regex(@"^\{""status"":""failure"",""error"":""(?<Error>.+)""\}", RegexOptions.IgnoreCase).Matches(json);
            if (error.Count > 0) throw new WebException("Server returned standard Json error: " + error[0].Groups["Error"].Value.ToString().Trim());
        }

        /// <summary>
        /// Attempts to extract a log files' score (out of 100) from the WhatCD log checker html.
        /// Note that the json api currently does not support this feature.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static int ExtractLogScore(string html)
        {
            var regex = new Regex(@">(?<score>[-\d]+)</span> \(out of 100\)</blockquote>");
            var matches = regex.Matches(html);
            if (matches.Count != 1) throw new WebException("Expected log score was not found");
            int score;
            if (!int.TryParse(matches[0].Groups["score"].Value.ToString(), out score)) throw new Exception("Failed to convert score to int.");
            return score;
        }

        /// <summary>
        /// Waits until designated minimum WhatCD query delay has elapsed.
        /// </summary>
        private static void WaitUntilCountdownComplete()
        {
            while (CountdownInProgress)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Ensures queries to the WhatCD server do not occur more frequently than is allowed (based on the minimum query delay value).
        /// </summary>
        private static void ActivateCountdown()
        {
            CountdownInProgress = true;
            Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(2000);
                    CountdownInProgress = false;
                });
        }
    }
}
