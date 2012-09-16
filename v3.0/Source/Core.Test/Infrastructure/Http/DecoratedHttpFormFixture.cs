using System;
using System.Collections.Specialized;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class DecoratedHttpFormFixture : IDisposable
    {
        private readonly Mock<IHttpForm> _innerHttpForm;
        private readonly DecoratedHttpForm _httpForm;

        public DecoratedHttpFormFixture()
        {
            _innerHttpForm = new Mock<IHttpForm>();
            _httpForm = new DecoratedHttpFormTestDouble(_innerHttpForm.Object);
        }

        public void Dispose()
        {
            _innerHttpForm.Verify();
        }

        [Fact]
        public void Get_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.Get(It.IsAny<HttpFormGetRequest>())).Returns(new HttpFormResponse()).Verifiable();

            _httpForm.Get(new HttpFormGetRequest { Url = "http://aurl.com" });
        }

        [Fact]
        public void GetAsync_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.GetAsync(It.IsAny<HttpFormGetRequest>())).Verifiable();

            _httpForm.GetAsync(new HttpFormGetRequest{ Url = "http://aurl.com" });
        }

        [Fact]
        public void GetAsync_With_Callback_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.GetAsync(It.IsAny<HttpFormGetRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Verifiable();

            _httpForm.GetAsync(new HttpFormGetRequest { Url = "http://aurl.com" }, delegate { }, delegate { });
        }

        [Fact]
        public void Post_With_FormFields_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.Post(It.IsAny<HttpFormPostRequest>())).Returns(new HttpFormResponse()).Verifiable();

            _httpForm.Post(new HttpFormPostRequest{ Url = "http://astory.com", FormFields = new NameValueCollection { { "foo", "bar" } } });
        }

        [Fact]
        public void Post_With_RawData_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.Post(It.IsAny<HttpFormPostRawRequest>())).Returns(new HttpFormResponse()).Verifiable();

            _httpForm.Post(new HttpFormPostRawRequest { Url = "http://astory.com", Data = "foo=bar"});
        }

        [Fact]
        public void PostAsync_With_FormFields_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.PostAsync(It.IsAny<HttpFormPostRequest>())).Verifiable();

            _httpForm.PostAsync(new HttpFormPostRequest{Url = "http://astory.com"});
        }

        [Fact]
        public void PostAsync_With_FormFields_And_Callback_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.PostAsync(It.IsAny<HttpFormPostRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Verifiable();

            _httpForm.PostAsync(new HttpFormPostRequest { Url = "http://astory.com" }, delegate {}, delegate {});
        }

        [Fact]
        public void PostAsync_With_RawData_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.PostAsync(It.IsAny<HttpFormPostRawRequest>())).Verifiable();

            _httpForm.PostAsync(new HttpFormPostRawRequest { Url = "http://astory.com" });
        }

        [Fact]
        public void PostAsync_With_RawData_And_Callback_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Setup(h => h.PostAsync(It.IsAny<HttpFormPostRawRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Verifiable();

            _httpForm.PostAsync(new HttpFormPostRawRequest { Url = "http://astory.com" }, delegate { }, delegate { });
        }
    }

    public class DecoratedHttpFormTestDouble : DecoratedHttpForm
    {
        public DecoratedHttpFormTestDouble(IHttpForm innerHttpForm) : base(innerHttpForm)
        {
        }
    }
}