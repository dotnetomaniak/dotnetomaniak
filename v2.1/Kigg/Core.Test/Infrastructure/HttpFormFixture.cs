using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

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

            _httpForm.Expect(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            var result = _httpForm.Object.Get("http://www.test.com");

            Assert.Equal(Response, result);
        }

        [Fact]
        public void Get_With_Header_Should_Return_Correct_Response()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            _httpForm.Expect(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            var result = _httpForm.Object.Get("http://www.test.com", new NameValueCollection { {"foo", "bar"} } );

            Assert.Equal(Response, result);
        }

        [Fact]
        public void GetAsync_Should_Not_Throw_Any_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<object>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.GetAsync("http://test.com"));
        }

        [Fact]
        public void GetAsync_With_Callbacks_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<object>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.GetAsync("http://test.com", Console.WriteLine, Console.WriteLine));
        }

        [Fact]
        public void GetAsync_With_Header_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<object>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.GetAsync("http://test.com", new NameValueCollection { { "foo", "bar" } }));
        }

        [Fact]
        public void GetAsync_With_Header_And_Callback_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.GetAsync("http://test.com", new NameValueCollection { { "foo", "bar" } }, Console.WriteLine, Console.WriteLine));
        }

        [Fact]
        public void GetAsync_Should_Throw_Exception_When_Invalid_Url_Is_Specified()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();

            request.Expect(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<WebException>();
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), false)).Returns(request.Object);

            bool exceptionOccurred = false;

            _httpForm.Object.GetAsync("http://test.com", null, e => exceptionOccurred = true);

            Assert.True(exceptionOccurred);
        }

        [Fact]
        public void Post_With_FormFields_Should_Return_Correct_Response()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            _httpForm.Expect(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            var result = _httpForm.Object.Post("http://www.test.com", new NameValueCollection{ { "foo", "bar" }});

            Assert.Equal(Response, result);
        }

        [Fact]
        public void Post_With_Header_And_FormFields_Should_Return_Correct_Response()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            _httpForm.Expect(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            var result = _httpForm.Object.Post("http://www.test.com", new NameValueCollection { { "foo", "bar" } }, new NameValueCollection { { "foo", "bar" } });

            Assert.Equal(Response, result);
        }

        [Fact]
        public void Post_With_RawData_Should_Return_Correct_Response()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            _httpForm.Expect(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            var result = _httpForm.Object.Post("http://www.test.com", "foo=bar");

            Assert.Equal(Response, result);
        }

        [Fact]
        public void Post_With_Header_And_RawData_Should_Return_Correct_Response()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            _httpForm.Expect(f => f.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            var result = _httpForm.Object.Post("http://www.test.com", new NameValueCollection { { "foo", "bar" } }, "foo=bar");

            Assert.Equal(Response, result);
        }

        [Fact]
        public void PostAsync_With_FormFields_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync("http://test.com", new NameValueCollection{ { "foo", "bar" }}));
        }

        [Fact]
        public void PostAsync_With_FormFields_And_Callbacks_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync("http://test.com", new NameValueCollection { { "foo", "bar" } }, Console.WriteLine, Console.WriteLine));
        }

        [Fact]
        public void PostAsync_With_Header_And_FormFields_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync("http://test.com", new NameValueCollection { { "foo", "bar" } }, new NameValueCollection { { "foo", "bar" } }));
        }

        [Fact]
        public void PostAsync_With_Header_FormFields_And_Callbacks_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync("http://test.com", new NameValueCollection { { "foo", "bar" } }, new NameValueCollection { { "foo", "bar" } }, Console.WriteLine, Console.WriteLine));
        }

        [Fact]
        public void PostAsync_With_RawData_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync("http://test.com", "foo=bar"));
        }

        [Fact]
        public void PostAsync_With_Header_And_RawData_Should_Not_Throw_Exception()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();
            Mock<IAsyncResult> result = new Mock<IAsyncResult>();

            request.Expect(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Returns(result.Object);
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>(), true)).Returns(request.Object);

            Assert.DoesNotThrow(() => _httpForm.Object.PostAsync("http://test.com", new NameValueCollection{{"foo", "bar"}},"foo=bar"));
        }

        [Fact]
        public void PostAsync_Should_Throw_Exception_When_Invalid_Url_Is_Specified()
        {
            Mock<WebRequest> request = new Mock<WebRequest>();

            request.Expect(r => r.BeginGetRequestStream(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<WebException>();
            _httpForm.Expect(h => h.CreateRequest(It.IsAny<string>(), It.IsAny<NameValueCollection>() ,true)).Returns(request.Object);

            bool exceptionOccurred = false;

            _httpForm.Object.PostAsync("http://test.com", new NameValueCollection{ { "foo", "bar" }}, null, e => exceptionOccurred = true);

            Assert.True(exceptionOccurred);
        }

        [Fact]
        public void CreateRequest_For_Get_Should_Return_Correct_Request()
        {
            var httpForm = CreateHttpForm();
            var request = (HttpWebRequest) httpForm.CreateRequest("http://www.test.com", new NameValueCollection() ,false);

            VerifyRequest(request, "GET");
        }

        [Fact]
        public void CreateRequest_For_Post_Should_Return_Correct_Request()
        {
            var httpForm = CreateHttpForm();
            var request = (HttpWebRequest) httpForm.CreateRequest("http://www.test.com", new NameValueCollection(), true);

            VerifyRequest(request, "POST");
        }

        [Fact]
        public void CreateRequest_Should_Return_Request_Which_AllowAutoRedirect_Is_Set_False_When_Zero_Is_Passed_As_MaximumRedirects_In_Constructor()
        {
            var httpForm = CreateHttpForm(0);
            var request = (HttpWebRequest) httpForm.CreateRequest("http://www.test.com", new NameValueCollection(), true);

            Assert.False(request.AllowAutoRedirect);
        }

        [Fact]
        public void ReadResponse_Should_Return_Null_For_Invalid_Url()
        {
            const string Response = "Test Response";

            Mock<WebRequest> request = MockRequest(Response);

            request.Expect(r => r.GetResponse()).Throws<WebException>();

            var result = HttpForm.ReadResponse(request.Object);

            Assert.Null(result);
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
                                RequesteStream = stream.Object,
                                RequestContent = content,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.ExpectGet(r => r.AsyncState).Returns(state);
            request.Expect(r => r.EndGetRequestStream(It.IsAny<IAsyncResult>())).Returns(stream.Object);
            stream.Expect(s => s.BeginWrite(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>()));

            HttpForm.RequestCallback(result.Object);
        }

        [Fact]
        public void RequestCallback_WithException()
        {
            var request = new Mock<WebRequest>();
            var stream = new Mock<Stream>();
            var content = new byte[512];

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                RequesteStream = stream.Object,
                                RequestContent = content,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.ExpectGet(r => r.AsyncState).Returns(state);
            request.Expect(r => r.EndGetRequestStream(It.IsAny<IAsyncResult>())).Returns(stream.Object);
            stream.Expect(s => s.BeginWrite(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<WebException>();

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
                                RequesteStream = stream.Object,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.ExpectGet(r => r.AsyncState).Returns(state);
            stream.Expect(s => s.EndWrite(It.IsAny<IAsyncResult>()));
            stream.Expect(s => s.Close());
            request.Expect(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<Exception>();

            HttpForm.WriteCallback(result.Object);
        }

        [Fact]
        public void WriteCallback_WithException()
        {
            var request = new Mock<WebRequest>();
            var stream = new Mock<Stream>();

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                RequesteStream = stream.Object,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.ExpectGet(r => r.AsyncState).Returns(state);
            stream.Expect(s => s.EndWrite(It.IsAny<IAsyncResult>()));
            stream.Expect(s => s.Close());
            request.Expect(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<HttpForm.StateContainer>())).Throws<WebException>();

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
            result.ExpectGet(r => r.AsyncState).Returns(state);

            var ms = new MemoryStream(Encoding.Default.GetBytes("This is a dummy response"));
            var response = new Mock<WebResponse>();
            response.Expect(r => r.GetResponseStream()).Returns(ms);

            request.Expect(r => r.EndGetResponse(It.IsAny<IAsyncResult>())).Returns(response.Object);

            HttpForm.ResponseCallback(result.Object);
        }

        [Fact]
        public void ResponseCallback_WithException()
        {
            var request = new Mock<WebRequest>();

            var state = new HttpForm.StateContainer
                            {
                                Request = request.Object,
                                OnComplete = delegate { },
                                OnError = delegate { }
                            };

            var result = new Mock<IAsyncResult>();

            result.ExpectGet(r => r.AsyncState).Returns(state);
            request.Expect(r => r.GetResponse()).Throws<WebException>();

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
            response.Expect(r => r.GetResponseStream()).Returns(ms);

            var request = new Mock<WebRequest>();
            request.Expect(r => r.GetRequestStream()).Returns(new MemoryStream());
            request.Expect(r => r.GetResponse()).Returns(response.Object);

            return request;
        }
    }
}