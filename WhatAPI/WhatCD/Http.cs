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
        /// Base URI for all communication with the WhatStatus server.
        /// </summary>
        public Uri BaseWhatStatusUri
        {
            get { return this._baseWhatStatusUri; }
            set { this._baseWhatStatusUri = value; }
        }
        private Uri _baseWhatStatusUri = new Uri("https://whatstatus.info");

        /// <summary>
        /// Base URI for all communication with the WhatCD server.
        /// </summary>
        public Uri BaseWhatCDUri
        {
            get { return this._baseWhatCDUri; }
            set { this._baseWhatCDUri = value; }
        }
        private Uri _baseWhatCDUri = new Uri("https://what.cd");

        /// <summary>
        /// Container for WhatCD cookies. Required to create and maintain a logged-on session.
        /// </summary>
        public CookieContainer CookieJar
        {
            get { return this._cookieJar; }
            private set { this._cookieJar = value; }
        }
        private CookieContainer _cookieJar = new CookieContainer();


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
            var fullUri = new Uri(uri, query);
            var wrw = new WebRequestWrapper(fullUri, WebRequestWrapper.RequestMethod.GET, "application/json; charset=utf-8", ref _cookieJar);
            using (var response = wrw.PerformWebRequest())
            using (var reader = new StreamReader(response))
            {
                var json = reader.ReadToEnd();
                if (Regex.IsMatch(json, @"^\{""status"":""failure""")) throw new WebException(string.Format("Request for {0} returned Json status failure: {1}", fullUri, json));
                return json;
            }
        }

        /// <summary>
        /// Performs a binary request. 
        /// </summary>
        /// <param name="uri">Base URI.</param>
        /// <param name="query">Request arguments (appended to the base URI).</param>
        /// <returns>Raw binary response.</returns>
        public byte[] RequestBytes(Uri uri, string query, out string contentDisposition, out string contentType)
        {
            var wrw = new WebRequestWrapper(new Uri(uri, query), WebRequestWrapper.RequestMethod.GET, "application/json; charset=utf-8", ref _cookieJar);
            using (var response = wrw.PerformWebRequest())
            {
                var result = new MemoryStream();
                var buffer = new byte[4096];
                int read;
                while ((read = response.Read(buffer, 0, buffer.Length)) > 0)
                {
                    result.Write(buffer, 0, read);
                }
                contentDisposition = wrw.WebHeaders["Content-Disposition"].ToString();
                contentType = wrw.WebHeaders["Content-Type"].ToString();
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
            int score;
            var wrw = new WebRequestWrapper(new Uri(this.BaseWhatCDUri, "logchecker.php"), WebRequestWrapper.RequestMethod.POST, "application/x-www-form-urlencoded", ref _cookieJar, postData);
            using (var response = wrw.PerformWebRequest())
            using (var reader = new StreamReader(response))
            {
                score = ExtractLogScore(reader.ReadToEnd());
            }
            return score;
        }

        /// <summary>
        /// Logs off the current What.CD session.
        /// </summary>
        /// <param name="authKey">WhatCD users' authentication key.</param>
        public void Logoff(string authKey)
        {
            var wrw = new WebRequestWrapper(new Uri(this.BaseWhatCDUri, string.Format("logout.php?auth={0}", authKey)), WebRequestWrapper.RequestMethod.POST, "application/x-www-form-urlencoded", ref _cookieJar);
            using (wrw.PerformWebRequest()) { }
        }

        /// <summary>
        /// Logs a user in to What.CD and stores session cookies.
        /// </summary>
        /// <param name="username">What.CD account username.</param>
        /// <param name="password">What.CD account password.</param>
        private void Login(string username, string password)
        {
            var wrw = new WebRequestWrapper(new Uri(this.BaseWhatCDUri, "login.php"), WebRequestWrapper.RequestMethod.POST, "application/x-www-form-urlencoded", ref _cookieJar, string.Format("username={0}&password={1}", Uri.EscapeDataString(username), Uri.EscapeDataString(password)));
            using (wrw.PerformWebRequest()) { }
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
    }
}
