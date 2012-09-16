using System;

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
        public void Get_Async_Should_Use_InnerHttpFrom_When_Url_Does_Not_Exists_In_Cache()
        {
            GetAsync();

            _httpForm.Verify();
        }

        [Fact]
        public void Get_Async_Should_Cache_Response_When_Url_Does_Not_Exist_In_Cache()
        {
            GetAsync();

            _httpForm.Verify();
        }

        [Fact]
        public void Get_Async_Should_Return_Cached_Response_When_Url_Exists_In_Cache()
        {
            // ReSharper disable RedundantAssignment
            string response = "This is dummy response";
            // ReSharper restore RedundantAssignment

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out response)).Returns(true);

            _cachingHttpForm.GetAsync("http://www.test.com", r => Assert.True(r.Length > 0), delegate { });
        }

        private void Get()
        {
            _httpForm.Expect(f => f.Get(It.IsAny<string>())).Returns("A dummy response").Verifiable();
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Verifiable();

            _cachingHttpForm.Get("http://www.test.com");
        }

        private void GetAsync()
        {
            _httpForm.Expect(f => f.GetAsync(It.IsAny<string>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>())).Callback((string url, Action<string> onComplete, Action<Exception> onError) => onComplete("This is a dummy response")).Verifiable();
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Verifiable();

            _cachingHttpForm.GetAsync("http://www.test.com", delegate { }, delegate { });
        }
    }
}