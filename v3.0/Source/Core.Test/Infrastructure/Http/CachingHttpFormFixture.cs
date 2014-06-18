using System;
using System.Collections.Specialized;
using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class CachingHttpFormFixture : BaseFixture
    {
        private readonly Mock<IHttpForm> _httpForm;
        private readonly CachingHttpForm _cachingHttpForm;

        public CachingHttpFormFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _cachingHttpForm = new CachingHttpForm(_httpForm.Object, 10);
        }

        [Fact]
        public void Get_Should_Use_InnerHttpFrom_When_Url_Does_Not_Exists_In_Cache()
        {
            Get();

            _httpForm.Verify();
        }

        [Fact]
        public void Get_Should_Cache_Response_When_Url_Does_Not_Exist_In_Cache()
        {
            Get();

            cache.Verify();
        }

        [Fact]
        public void Get_Async_Should_Use_Use_InnerHttpFrom()
        {
            _httpForm.Setup(f => f.GetAsync(It.IsAny<HttpFormGetRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormGetRequest getRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError) => onComplete(new HttpFormResponse { Response = "This is a dummy response" })).Verifiable();

            _cachingHttpForm.GetAsync(new HttpFormGetRequest { Url = "http://www.test.com" });

            _httpForm.Verify();
        }

        [Fact]
        public void Get_Async_With_Callback_Should_Use_InnerHttpFrom_When_Url_Does_Not_Exists_In_Cache()
        {
            GetAsync();

            _httpForm.Verify();
        }

        [Fact]
        public void Get_Async_With_Callback_Should_Cache_Response_When_Url_Does_Not_Exist_In_Cache()
        {
            GetAsync();

            _httpForm.Verify();
        }

        [Fact]
        public void Get_Async_With_Callback_Should_Return_Cached_Response_When_Url_Exists_In_Cache()
        {
            // ReSharper disable RedundantAssignment
            HttpFormResponse httpResponse =  new HttpFormResponse{ Response = "This is dummy response"};
            // ReSharper restore RedundantAssignment

            cache.Setup(c => c.TryGet(It.IsAny<string>(), out httpResponse)).Returns(true);

            _cachingHttpForm.GetAsync(new HttpFormGetRequest { Url = "http://www.test.com"}, h => Assert.True(h.Response.Length > 0), delegate { });
        }

        private void Get()
        {
            _httpForm.Setup(f => f.Get(It.IsAny<HttpFormGetRequest>())).Returns(new HttpFormResponse { Response = "A dummy response" });
            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<HttpFormResponse>(), It.IsAny<DateTime>())).Verifiable();

            _cachingHttpForm.Get(BuildRequest());
        }

        private void GetAsync()
        {
            _httpForm.Setup(f => f.GetAsync(It.IsAny<HttpFormGetRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormGetRequest getRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError) => onComplete(new HttpFormResponse { Response = "This is a dummy response"})).Verifiable();

            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Verifiable();

            _cachingHttpForm.GetAsync(BuildRequest(), delegate { }, delegate { });
        }

        private static HttpFormGetRequest BuildRequest()
        {
            return new HttpFormGetRequest
                       {
                           Url = "http://www.test.com",
                           UserName = "foo",
                           Password = "bar",
                           ContentType = "text/xml",
                           Cookies = new NameValueCollection
                                         {
                                             {"hello", "world" }
                                         },
                           Headers = new NameValueCollection
                                         {
                                             {"foo", "bar" }
                                         }
                       };
        }
    }
}