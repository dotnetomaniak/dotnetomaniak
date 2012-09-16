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
            _innerHttpForm.Expect(h => h.Get(It.IsAny<string>()));

            _httpForm.Get("http://aurl.com");
        }

        [Fact]
        public void Get_With_Header_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()));

            _httpForm.Get("http://aurl.com", new NameValueCollection{{"foo", "bar"}});
        }

        [Fact]
        public void GetAsync_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.GetAsync(It.IsAny<string>())).Verifiable();

            _httpForm.GetAsync("http://aurl.com");
        }

        [Fact]
        public void GetAsync_With_Callback_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.GetAsync(It.IsAny<string>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>())).Verifiable();

            _httpForm.GetAsync("http://aurl.com", delegate{}, delegate{});
        }

        [Fact]
        public void GetAsync_With_Header_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.GetAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>())).Verifiable();

            _httpForm.GetAsync("http://aurl.com", new NameValueCollection { { "foo", "bar" } });
        }

        [Fact]
        public void GetAsync_With_Header_And_Callback_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.GetAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>() ,It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>())).Verifiable();

            _httpForm.GetAsync("http://aurl.com", new NameValueCollection { { "foo", "bar" } }, delegate { }, delegate { });
        }

        [Fact]
        public void Post_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.Post(It.IsAny<string>(), It.IsAny<NameValueCollection>()));

            _httpForm.Post("http://astory.com", new NameValueCollection{ { "foo", "bar" } });
        }

        [Fact]
        public void Post_With_HeaderShould_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.Post(It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>()));

            _httpForm.Post("http://astory.com", new NameValueCollection { { "foo", "bar" } }, new NameValueCollection { { "hello", "world" } });
        }

        [Fact]
        public void Post_With_RawData_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.Post(It.IsAny<string>(), It.IsAny<string>()));

            _httpForm.Post("http://astory.com", "foo=bar");
        }

        [Fact]
        public void Post_With_Header_And_RawData_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.Post(It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<string>()));

            _httpForm.Post("http://astory.com", new NameValueCollection { { "hello", "world" } }, "foo=bar");
        }

        [Fact]
        public void PostAsync_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.PostAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>()));

            _httpForm.PostAsync("http://astory.com", new NameValueCollection { { "hello", "world" } });
        }

        [Fact]
        public void PostAsync_With_Callback_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.PostAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>()));

            _httpForm.PostAsync("http://astory.com", new NameValueCollection { { "hello", "world" } }, delegate{}, delegate{});
        }

        [Fact]
        public void PostAsync_With_Header_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.PostAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>()));

            _httpForm.PostAsync("http://astory.com", new NameValueCollection { { "foo", "bar" } }, new NameValueCollection { { "hello", "world" } });
        }

        [Fact]
        public void PostAsync_With_Header_And_Callback_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.PostAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<NameValueCollection>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>()));

            _httpForm.PostAsync("http://astory.com", new NameValueCollection { { "foo", "bar" } }, new NameValueCollection { { "hello", "world" } }, delegate{}, delegate{});
        }

        [Fact]
        public void PostAsync_With_RawData_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.PostAsync(It.IsAny<string>(), It.IsAny<string>()));

            _httpForm.PostAsync("http://astory.com", "foo=bar");
        }

        [Fact]
        public void PostAsync_With_Header_And_RawData_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.PostAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<string>()));

            _httpForm.PostAsync("http://astory.com", new NameValueCollection{ {"hello", "world"}},"foo=bar");
        }

        [Fact]
        public void PostAsync_With_Header_RawData_And_Callback_Should_Use_InnerHttpForm()
        {
            _innerHttpForm.Expect(h => h.PostAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>(), It.IsAny<string>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>()));

            _httpForm.PostAsync("http://astory.com", new NameValueCollection { { "hello", "world" } }, "foo=bar", delegate{}, delegate{});
        }
    }

    public class DecoratedHttpFormTestDouble : DecoratedHttpForm
    {
        public DecoratedHttpFormTestDouble(IHttpForm innerHttpForm) : base(innerHttpForm)
        {
        }
    }
}