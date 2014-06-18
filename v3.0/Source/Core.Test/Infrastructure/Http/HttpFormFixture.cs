using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    public class HttpFormFixture
    {
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; .NET CLR 3.0.04506; .NET CLR 3.5.21022)";
        private const int Timeout = 15000;

        private readonly Mock<HttpForm> _httpForm;

        public HttpFormFixture()
        {
            _httpForm = new Mock<HttpForm>(UserAgent, Timeout, true, 8);
        }

        [Fact]
        public void Get_Should_Return_Correct_Response()
        {
            const string Response ="Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            var result = _httpForm.Object.Get(new HttpFormGetRequest { Url = "http://www.test.com" });

            Assert.Equal(Response, result.Response);
        }

        [Fact]
        public void GetAsync_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Setup(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<object>())).Returns(result.Object);
            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.GetAsync(new HttpFormGetRequest { Url = "http://www.test.com" }));
        }

        [Fact]
        public void GetAsync_Should_Handle_Exception_When_Exception_Occurrs()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();

            request.Setup(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<WebException>();
            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            bool exceptionOccurred = false;

            _httpForm.Object.GetAsync(new HttpFormGetRequest { Url = "http://test.com"}, delegate {}, e => exceptionOccurred = true);

            Assert.True(exceptionOccurred);
        }

        [Fact]
        public void Post_With_FormFields_Should_Return_Correct_Response()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            var result = _httpForm.Object.Post(new HttpFormPostRequest { Url =  "http://www.test.com", FormFields = new NameValueCollection{ { "foo", "bar" }}});

            Assert.Equal(Response, result.Response);
        }

        [Fact]
        public void Post_With_RawData_Should_Return_Correct_Response()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            var result = _httpForm.Object.Post(new HttpFormPostRawRequest { Url = "http://www.test.com", Data = "foo=bar"});

            Assert.Equal(Response, result.Response);
        }

        [Fact]
        public void PostAsync_With_FormFields_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Setup(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync(new HttpFormPostRequest { Url = "http://www.test.com", FormFields = new NameValueCollection { { "foo", "bar" } } }));
        }

        [Fact]
        public void PostAsync_With_FormFields_And_Callbacks_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Setup(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync(new HttpFormPostRequest { Url = "http://www.test.com", FormFields = new NameValueCollection { { "foo", "bar" } } }, delegate {}, delegate {}));
        }

        [Fact]
        public void PostAsync_With_RawData_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Setup(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync(new HttpFormPostRawRequest { Url = "http://www.test.com", Data = "foo=bar" }));
        }

        [Fact]
        public void PostAsync_With_RawData_And_Callbacks_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Setup(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync(new HttpFormPostRawRequest { Url = "http://www.test.com", Data = "foo=bar" }, delegate {}, delegate {}));
        }

        [Fact]
        public void Internal_PostAsync_Should_Handle_Exception_When_Exception_Occurrs()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();

            request.Setup(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<WebException>();
            _httpForm.Setup(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            bool exceptionOccurred = false;

            _httpForm.Object.PostAsync(new HttpFormPostRequest { Url = "http://test.com" }, "foo=bar", delegate { }, e => exceptionOccurred = true);

            Assert.True(exceptionOccurred);
        }

        [Fact]
        public void CreateRequest_For_Get_Should_Return_Correct_Request()
        {
            var httpForm = CreateHttpForm();
            var request = (HttpWebRequest) httpForm.CreateRequest("http://www.test.com", "foo", "bar", "text/xml", new NameValueCollection{{"foo", "bar"}}, new NameValueCollection(), false);

            VerifyRequest(request, "GET");
        }

        [Fact]
        public void CreateRequest_For_Post_Should_Return_Correct_Request()
        {
            var httpForm = CreateHttpForm();
            var request = (HttpWebRequest) httpForm.CreateRequest("http://www.test.com", "foo", "bar", string.Empty, new NameValueCollection(), new NameValueCollection(), true);

            VerifyRequest(request, "POST");
        }

        [Fact]
        public void CreateRequest_Should_Return_Request_Which_AllowAutoRedirect_Is_Set_False_When_Zero_Is_Passed_As_MaximumRedirects_In_Constructor()
        {
            var httpForm = CreateHttpForm(0);
            var request = (HttpWebRequest)httpForm.CreateRequest("http://www.test.com", "foo", "bar", "text/xml", new NameValueCollection(), new NameValueCollection(), false);

            Assert.False(request.AllowAutoRedirect);
        }

        [Fact]
        public void ReadResponse_Should_Return_Correct_Response()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            request.Setup(r => r.GetResponse()).Throws<WebException>();

            var result = HttpForm.ReadResponse(request.Object);

            Assert.Null(result.Response);
        }

        [Fact]
        public void ReadResponse_Should_Return_Null_For_Invalid_Url()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            request.Setup(r => r.GetResponse()).Throws<WebException>();

            var result = HttpForm.ReadResponse(request.Object);

            Assert.Null(result.Response);
        }

        [Fact]
        public void PrepareRequestBody_Should_Return_Correct_Body()
        {
            NameValueCollection formFields = new NameValueCollection
                                                 {
                                                     { "foo", "bar" },
                                                     { "hello", "world" }
                                                 };

            string body = HttpForm.PrepareRequestBody(formFields);

            Assert.Equal("foo=bar&hello=world", body);
        }

        [Fact]
        public void RequestCallback()
        {
            var request = new Mock<WebRequest>();
            var stream = new Mock<Stream>();
            var content = new byte[512];

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                RequestStream = stream.Object,
                                RequestContent = content,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.SetupGet(r => r.AsyncState).Returns(state);
            request.Setup(r => r.EndGetRequestStream(It.IsAny<IAsyncResult>())).Returns(stream.Object);
            stream.Setup(s => s.BeginWrite(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>()));

            HttpForm.RequestCallback(result.Object);
        }

        [Fact]
        public void RequestCallback_Should_Handle_Exception()
        {
            var request = new Mock<WebRequest>();
            var stream = new Mock<Stream>();
            var content = new byte[512];

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                RequestStream = stream.Object,
                                RequestContent = content,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.SetupGet(r => r.AsyncState).Returns(state);
            request.Setup(r => r.EndGetRequestStream(It.IsAny<IAsyncResult>())).Returns(stream.Object);
            stream.Setup(s => s.BeginWrite(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<WebException>();

            HttpForm.RequestCallback(result.Object);
        }

        [Fact]
        public void WriteCallback()
        {
            var request = new Mock<WebRequest>();
            var stream = new Mock<Stream>();

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                RequestStream = stream.Object,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.SetupGet(r => r.AsyncState).Returns(state);
            stream.Setup(s => s.EndWrite(It.IsAny<IAsyncResult>()));
            stream.Setup(s => s.Close());
            request.Setup(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<Exception>();

            HttpForm.WriteCallback(result.Object);
        }

        [Fact]
        public void WriteCallback_Should_Handle_Exception()
        {
            var request = new Mock<WebRequest>();
            var stream = new Mock<Stream>();

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                RequestStream = stream.Object,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.SetupGet(r => r.AsyncState).Returns(state);
            stream.Setup(s => s.EndWrite(It.IsAny<IAsyncResult>()));
            stream.Setup(s => s.Close());
            request.Setup(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<WebException>();

            HttpForm.WriteCallback(result.Object);
        }

        [Fact]
        public void ResponseCallback()
        {
            var request = new Mock<WebRequest>();

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();
            result.SetupGet(r => r.AsyncState).Returns(state);

            var ms = new MemoryStream(Encoding.Default.GetBytes("This is a dummy response"));
            var response = new Mock<WebResponse>();
            response.Setup(r => r.GetResponseStream()).Returns(ms);

            request.Setup(r => r.EndGetResponse(It.IsAny<IAsyncResult>())).Returns(response.Object);

            HttpForm.ResponseCallback(result.Object);
        }

        [Fact]
        public void ResponseCallback_Should_Handle_Exception()
        {
            var request = new Mock<WebRequest>();

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.SetupGet(r => r.AsyncState).Returns(state);
            request.Setup(r => r.GetResponse()).Throws<WebException>();

            HttpForm.ResponseCallback(result.Object);
        }

        private static void VerifyRequest(HttpWebRequest request, string method)
        {
            Assert.Equal(method, request.Method);
            Assert.Equal(UserAgent, request.UserAgent);
            Assert.Equal("*/*", request.Accept);
            Assert.Null(request.Expect);
            Assert.Equal(Timeout, request.Timeout);
            Assert.Equal("gzip,deflate", request.Headers["Accept-Encoding"]);
            Assert.Equal(DecompressionMethods.GZip | DecompressionMethods.Deflate, request.AutomaticDecompression);
        }

        private static HttpForm CreateHttpForm()
        {
            return CreateHttpForm(5);
        }

        private static HttpForm CreateHttpForm(int maximumRedirects)
        {
            return new HttpForm(UserAgent, Timeout, true, maximumRedirects);
        }

        private static Mock<WebRequest> MockRequest(string responseString)
        {
            var data = Encoding.Default.GetBytes(responseString);
            var ms = new MemoryStream(data);

            var response = new Mock<WebResponse>();
            response.Setup(r => r.GetResponseStream()).Returns(ms);
            response.SetupGet(r => r.Headers).Returns(new WebHeaderCollection { { "foo", "bar" } });

            var request = new Mock<WebRequest>();
            request.Setup(r => r.GetRequestStream()).Returns(new MemoryStream());
            request.Setup(r => r.GetResponse()).Returns(response.Object);

            return request;
        }
    }
}