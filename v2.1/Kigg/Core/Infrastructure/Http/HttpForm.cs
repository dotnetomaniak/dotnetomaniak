namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Threading;

    public class HttpForm : IHttpForm
    {
        private readonly string _userAgent;
        private readonly int _timeout;
        private readonly bool _requestCompressed;
        private readonly int _maximumRedirects;

        static HttpForm()
        {
            // Ignore the ssl certificate error which we usually get in the web browser such as expired etc.
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public HttpForm(string userAgent, int timeout, bool requestCompressed, int maximumRedirects)
        {
            Check.Argument.IsNotEmpty(userAgent, "userAgent");
            Check.Argument.IsNotNegative(timeout, "timeout");
            Check.Argument.IsNotNegative(maximumRedirects, "maximumRedirects");

            _userAgent = userAgent;
            _timeout = timeout;
            _requestCompressed = requestCompressed;
            _maximumRedirects = maximumRedirects;
        }

        public string Get(string url)
        {
            return Get(url, null);
        }

        public string Get(string url, NameValueCollection headers)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            WebRequest request = CreateRequest(url, headers, false);

            return ReadResponse(request);
        }

        public void GetAsync(string url)
        {
            GetAsync(url, null, null, null);
        }

        public void GetAsync(string url, Action<string> onComplete, Action<Exception> onError)
        {
            GetAsync(url, null, onComplete, onError);
        }

        public void GetAsync(string url, NameValueCollection headers)
        {
            GetAsync(url, headers, null, null);
        }

        public void GetAsync(string url, NameValueCollection headers, Action<string> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            WebRequest request = CreateRequest(url, headers, false);

            try
            {
                request.BeginGetResponse(ResponseCallback, new StateContainer { Request = request, OnComplete = onComplete, OnError = onError });
            }
            catch (Exception e)
            {
                if (onError != null)
                {
                    onError(e);
                }
            }
        }

        public string Post(string url, NameValueCollection formFields)
        {
            return Post(url, null, formFields);
        }

        public string Post(string url, NameValueCollection headers, NameValueCollection formFields)
        {
            Check.Argument.IsNotNull(formFields, "formFields");

            return Post(url, headers, PrepareRequestBody(formFields));
        }

        public string Post(string url, string rawData)
        {
            return Post(url, null, rawData);
        }

        public string Post(string url, NameValueCollection headers, string rawData)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");
            Check.Argument.IsNotEmpty(rawData, "rawData");

            WebRequest request = CreateRequest(url, headers, true);

            byte[] content = Encoding.ASCII.GetBytes(rawData);

            request.ContentLength = content.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(content, 0, content.Length);
            }

            return ReadResponse(request);
        }

        public void PostAsync(string url, NameValueCollection formFields)
        {
            PostAsync(url, null, formFields);
        }

        public void PostAsync(string url, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError)
        {
            PostAsync(url, null, formFields, onComplete, onError);
        }

        public void PostAsync(string url, NameValueCollection headers, NameValueCollection formFields)
        {
            Check.Argument.IsNotNull(formFields, "formFields");

            PostAsync(url, headers, PrepareRequestBody(formFields));
        }

        public void PostAsync(string url, NameValueCollection headers, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotNull(formFields, "formFields");

            PostAsync(url, headers, PrepareRequestBody(formFields), onComplete, onError);
        }

        public void PostAsync(string url, string rawData)
        {
            PostAsync(url, null, rawData);
        }

        public void PostAsync(string url, NameValueCollection headers, string rawData)
        {
            PostAsync(url, headers, rawData, null, null);
        }

        public void PostAsync(string url, NameValueCollection headers, string rawData, Action<string> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");
            Check.Argument.IsNotEmpty(rawData, "rawData");

            WebRequest request = CreateRequest(url, headers, true);

            byte[] content = Encoding.ASCII.GetBytes(rawData);
            request.ContentLength = content.Length;

            try
            {
                request.BeginGetRequestStream(RequestCallback, new StateContainer { Request = request, RequestContent = content, OnComplete = onComplete, OnError = onError });
            }
            catch (Exception e)
            {
                if (onError != null)
                {
                    onError(e);
                }
            }
        }

        internal static void RequestCallback(IAsyncResult result)
        {
            StateContainer states = (StateContainer) result.AsyncState;

            try
            {
                states.RequesteStream = states.Request.EndGetRequestStream(result);

                states.RequesteStream.BeginWrite(states.RequestContent, 0, states.RequestContent.Length, WriteCallback, states);
            }
            catch (Exception e)
            {
                if (states.OnError != null)
                {
                    states.OnError(e);
                }
            }
        }

        internal static void WriteCallback(IAsyncResult result)
        {
            StateContainer states = (StateContainer) result.AsyncState;

            try
            {
                states.RequesteStream.EndWrite(result);
                states.RequesteStream.Close();

                states.Request.BeginGetResponse(ResponseCallback, states);
            }
            catch (Exception e)
            {
                if (states.OnError != null)
                {
                    states.OnError(e);
                }
            }
        }

        internal static void ResponseCallback(IAsyncResult result)
        {
            StateContainer states = (StateContainer) result.AsyncState;

            try
            {
                WebResponse response = states.Request.EndGetResponse(result);

                using (Stream stream = response.GetResponseStream())
                {
                    const int BufferLength = 8096;
                    byte[] buffer = new byte[BufferLength];
                    string responseString;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        int bytesRead;

                        while ((bytesRead = stream.Read(buffer, 0, BufferLength)) > 0)
                        {
                            ms.Write(buffer, 0, bytesRead);
                        }

                        ms.Flush();

                        responseString = Encoding.UTF8.GetString(ms.ToArray());
                    }

                    if (states.OnComplete != null)
                    {
                        states.OnComplete(responseString);
                    }
                }
            }
            catch (Exception e)
            {
                if (states.OnError != null)
                {
                    states.OnError(e);
                }
            }
        }

        internal static string PrepareRequestBody(NameValueCollection formFields)
        {
            StringBuilder requestBody = new StringBuilder();

            foreach (string key in formFields.AllKeys)
            {
                if (requestBody.Length > 0)
                {
                    requestBody.Append("&");
                }

                requestBody.Append("{0}={1}".FormatWith(key, formFields[key]));
            }

            return requestBody.ToString();
        }

        internal static string ReadResponse(WebRequest request)
        {
            const int MaxTry = 3;
            int tryCount = 0;
            string responseString = null;

            // Sometimes the external site can throw exception so we might
            // have to retry few more times
            while (string.IsNullOrEmpty(responseString) && (tryCount < MaxTry))
            {
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = sr.ReadToEnd();
                        }
                    }
                }
                catch (WebException)
                {
                    tryCount += 1;
                    Thread.Sleep(200);
                }
            }

            return responseString;
        }

        protected internal virtual WebRequest CreateRequest(string url, NameValueCollection headers, bool isPost)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Method = isPost ? "POST" : "GET";
            request.UserAgent = _userAgent;

            if (_maximumRedirects == 0)
            {
                request.AllowAutoRedirect = false;
            }

            if (_maximumRedirects > 0)
            {
                request.MaximumAutomaticRedirections = _maximumRedirects;
            }

            request.Accept = "*/*";
            request.Expect = string.Empty;

            if (headers != null)
            {
                request.Headers.Add(headers);
            }

            if (_timeout > 0)
            {
                request.Timeout = _timeout;
            }

            if (isPost)
            {
                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            }
            else
            {
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
            }

            if (_requestCompressed)
            {
                request.Headers.Add("Accept-Encoding", "gzip,deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return request;
        }

        internal sealed class StateContainer
        {
            public WebRequest Request
            {
                get;
                set;
            }

            public Stream RequesteStream
            {
                get;
                set;
            }

            public byte[] RequestContent
            {
                get;
                set;
            }

            public Action<string> OnComplete
            {
                get;
                set;
            }

            public Action<Exception> OnError
            {
                get;
                set;
            }
        }
    }
}