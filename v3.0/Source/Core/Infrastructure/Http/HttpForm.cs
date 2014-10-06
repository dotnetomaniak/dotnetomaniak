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

        //static HttpForm()
        //{
        //    // Ignore the ssl certificate error which we usually get in the web browser such as expired etc.
        //    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        //}

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

        public HttpFormResponse Get(HttpFormGetRequest getRequest)
        {
            Check.Argument.IsNotNull(getRequest, "getRequest");
            Check.Argument.IsNotEmpty(getRequest.Url, "getRequest.Url");

            WebRequest request = CreateRequest(getRequest.Url, getRequest.UserName, getRequest.Password, getRequest.ContentType, getRequest.Headers, getRequest.Cookies, false);

            return ReadResponse(request);
        }

        public void GetAsync(HttpFormGetRequest getRequest)
        {
            GetAsync(getRequest, null, null);
        }

        public void GetAsync(HttpFormGetRequest getRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotNull(getRequest, "getRequest");
            Check.Argument.IsNotEmpty(getRequest.Url, "getRequest.Url");

            WebRequest request = CreateRequest(getRequest.Url, getRequest.UserName, getRequest.Password, getRequest.ContentType, getRequest.Headers, getRequest.Cookies, false);

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

        public HttpFormResponse Post(HttpFormPostRequest postRequest)
        {
            string rawData = PrepareRequestBody(postRequest.FormFields);

            return Post(postRequest, rawData);
        }

        public HttpFormResponse Post(HttpFormPostRawRequest postRequest)
        {
            return Post(postRequest, postRequest.Data);
        }

        public void PostAsync(HttpFormPostRequest postRequest)
        {
            PostAsync(postRequest, null, null);
        }

        public void PostAsync(HttpFormPostRequest postRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError)
        {
            string rawData = PrepareRequestBody(postRequest.FormFields);

            PostAsync(postRequest, rawData, onComplete, onError);
        }

        public void PostAsync(HttpFormPostRawRequest postRequest)
        {
            PostAsync(postRequest, null, null);
        }

        public void PostAsync(HttpFormPostRawRequest postRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError)
        {
            PostAsync(postRequest, postRequest.Data, onComplete, onError);
        }

        internal HttpFormResponse Post(HttpFormGetRequest postRequest, string rawData)
        {
            Check.Argument.IsNotNull(postRequest, "postRequest");
            Check.Argument.IsNotEmpty(postRequest.Url, "postRequest.Url");

            WebRequest request = CreateRequest(postRequest.Url, postRequest.UserName, postRequest.Password, postRequest.ContentType, postRequest.Headers, postRequest.Cookies, true);

            PreapreRequestToPost(request, rawData);

            return ReadResponse(request);
        }

        internal void PostAsync(HttpFormGetRequest postRequest, string rawData, Action<HttpFormResponse> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotNull(postRequest, "postRequest");
            Check.Argument.IsNotEmpty(postRequest.Url, "postRequest.Url");

            WebRequest request = CreateRequest(postRequest.Url, postRequest.UserName, postRequest.Password, postRequest.ContentType, postRequest.Headers, postRequest.Cookies, true);

            byte[] content = Encoding.UTF8.GetBytes(rawData);
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
            var states = (StateContainer) result.AsyncState;

            try
            {
                states.RequestStream = states.Request.EndGetRequestStream(result);

                states.RequestStream.BeginWrite(states.RequestContent, 0, states.RequestContent.Length, WriteCallback, states);
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
            var states = (StateContainer) result.AsyncState;

            try
            {
                states.RequestStream.EndWrite(result);
                states.RequestStream.Close();

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
            var states = (StateContainer) result.AsyncState;

            try
            {
                var httpFormResponse = new HttpFormResponse();
                WebResponse response = states.Request.EndGetResponse(result);

                PopulateHeadersAndCookies(response, httpFormResponse);

                using (Stream stream = response.GetResponseStream())
                {
                    const int BufferLength = 8096;
                    var buffer = new byte[BufferLength];

                    using (var ms = new MemoryStream())
                    {
                        int bytesRead;

                        while ((bytesRead = stream.Read(buffer, 0, BufferLength)) > 0)
                        {
                            ms.Write(buffer, 0, bytesRead);
                        }

                        ms.Flush();

                        httpFormResponse.Response = Encoding.UTF8.GetString(ms.ToArray());
                    }

                    if (states.OnComplete != null)
                    {
                        states.OnComplete(httpFormResponse);
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

        internal static void PreapreRequestToPost(WebRequest request,string rawData)
        {
            byte[] content = Encoding.UTF8.GetBytes(rawData);
            request.ContentLength = content.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(content, 0, content.Length);
            }
        }

        internal static string PrepareRequestBody(NameValueCollection formFields)
        {
            var requestBody = new StringBuilder();

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

        internal static HttpFormResponse ReadResponse(WebRequest request)
        {
            const int MaxTry = 3;

            int tryCount = 0;
            var httpFormResponse = new HttpFormResponse();

            // Sometimes the external site can throw exception so we might
            // have to retry few more times
            while (string.IsNullOrEmpty(httpFormResponse.Response) && (tryCount < MaxTry))
            {
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        PopulateHeadersAndCookies(response, httpFormResponse);

                        using (var sr = new StreamReader(response.GetResponseStream()))
                        {
                            httpFormResponse.Response = sr.ReadToEnd();
                        }
                    }
                }
                catch (WebException)
                {
                    tryCount += 1;
                    Thread.Sleep(200);
                }
            }

            return httpFormResponse;
        }

        internal static void PopulateHeadersAndCookies(WebResponse webResponse, HttpFormResponse response)
        {
            response.Headers.Add(webResponse.Headers);

            var httpWebResponse = webResponse as HttpWebResponse;

            if (httpWebResponse != null)
            {
                foreach (Cookie cookie in httpWebResponse.Cookies)
                {
                    response.Cookies.Add(cookie.Name, cookie.Value);
                }
            }
        }

        protected internal virtual WebRequest CreateRequest(string url, string userName, string password, string contentType, NameValueCollection headers, NameValueCollection cookies, bool isPost)
        {
            var request = (HttpWebRequest) WebRequest.Create(new Uri(url));

            request.Method = isPost ? "POST" : "GET";
            request.UserAgent = _userAgent;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                request.Credentials = new NetworkCredential(userName, password);
            }

            if (_maximumRedirects > 0)
            {
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = _maximumRedirects;
            }
            else
            {
                request.AllowAutoRedirect = false;
            }

            request.Accept = "*/*";
            request.Expect = string.Empty;

            if (headers.Count > 0)
            {
                request.Headers.Add(headers);
            }

            if (cookies.Count > 0)
            {
                request.CookieContainer = new CookieContainer();

                foreach(string key in cookies)
                {
                    request.CookieContainer.Add(new Cookie(key, cookies[key]));
                }
            }

            if (_timeout > 0)
            {
                request.Timeout = _timeout;
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }

            if ((isPost) && (string.IsNullOrEmpty(request.ContentType)))
            {
                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            }

            if (!isPost)
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

            public Stream RequestStream
            {
                get;
                set;
            }

            public byte[] RequestContent
            {
                get;
                set;
            }

            public Action<HttpFormResponse> OnComplete
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