using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WhatCD
{
    internal class WebRequestWrapper
    {

        public Uri Uri { get; private set; }
        public RequestMethod RequestMeth { get; private set; }
        public string ContentType { get; private set; }
        public CookieContainer CookieJar { get; private set; }
        public string DataToPost { get; private set; }

        public WebHeaderCollection WebHeaders { get; private set; }

        /// <summary>
        /// Static flag to indicate if threads must wait before making any more requests to the WhatCD server.
        /// </summary>
        private static bool _countdownInProgress;

        /// <summary>
        /// Static lock object used with all Http requests to ensure multiple threads adhere to the designated minimum query delay for communicating with WhatCD servers.
        /// </summary>
        private static object _httpLocker = new object();
        private static bool _lockTaken;
        private static Task _task;

        public enum RequestMethod
        {
            GET,
            POST
        }

        public WebRequestWrapper(Uri uri, RequestMethod requestMethod, string contentType, ref CookieContainer cookieJar, string dataToPost = null)
        {
            this.Uri = uri;
            this.RequestMeth = requestMethod;
            this.ContentType = contentType;
            this.CookieJar = cookieJar;
            this.DataToPost = dataToPost;
        }

        public Stream PerformWebRequest()
        {
            WaitUntilCountdownComplete();
            Monitor.Enter(_httpLocker, ref _lockTaken);

            this.WebHeaders = null;
            HttpWebResponse response = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(this.Uri);
                request.Method = this.RequestMeth.ToString();
                request.ContentType = this.ContentType;
                if (this.CookieJar != null) request.CookieContainer = this.CookieJar;

                if (this.RequestMeth == RequestMethod.POST)
                {
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(this.DataToPost);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                this.WebHeaders = response.Headers;
                return response.GetResponseStream();
            }
            finally
            {
                if (response != null && response.StatusCode != HttpStatusCode.OK) throw new WebException(string.Format("Non-OK HTTP status returned from {0}. Status code {1}: {2}", this.Uri.ToString(), response.StatusCode, response.StatusDescription));
                ActivateCountdown();
                Monitor.Exit(_httpLocker);
                _lockTaken = false;
            }
        }

        private static void WaitUntilCountdownComplete()
        {
            while (_countdownInProgress || _lockTaken)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void ActivateCountdown()
        {
            _countdownInProgress = true;
            if (_task != null && _task.Status != TaskStatus.RanToCompletion) throw new Exception(string.Format("Unexpected task status: {0}", _task.Status.ToString()));
            _task = Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(2000);
                _countdownInProgress = false;
            });
        }

    }
}
