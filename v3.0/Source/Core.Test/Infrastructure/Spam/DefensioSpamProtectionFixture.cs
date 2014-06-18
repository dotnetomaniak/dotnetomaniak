using System;
using Kigg.Infrastructure;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class DefensioSpamProtectionFixture : BaseFixture
    {
        private const string Version = "1.0";
        private const string ApiKey = "a key";

        private static readonly string ApiKeyCheckUrl = "http://api.defensio.com/app/{1}/validate-key/{0}.xml".FormatWith(ApiKey, Version);
        private static readonly string CommentCheckUrl = "http://api.defensio.com/app/{1}/audit-comment/{0}.xml".FormatWith(ApiKey, Version);

        private const string ValidApiKeyResponse = "<defensio-result><status>success</status></defensio-result>";
        private const string SpamCommentResponse = "<defensio-result><spam>true</spam></defensio-result>";
        private const string NotSpamCommentResponse = "<defensio-result><spam>false</spam></defensio-result>";

        private readonly Mock<IHttpForm> _httpForm;

        private readonly ISpamProtection _spamProtection;

        public DefensioSpamProtectionFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _spamProtection = new DefensioSpamProtection(ApiKey, Version, settings.Object, _httpForm.Object);
        }

        [Fact]
        public void Api_Key_Should_Be_Valid()
        {
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl))).Returns(new HttpFormResponse { Response = NotSpamCommentResponse });

            Assert.DoesNotThrow(() => _spamProtection.IsSpam(CreateDummyContent()));
        }

        [Fact]
        public void Should_Throw_Exception_When_Api_Key_Is_Not_Valid()
        {
            _httpForm.Setup(h => h.Post(It.IsAny<HttpFormPostRequest>())).Returns(new HttpFormResponse{ Response = "<defensio-result><status>failure</status></defensio-result>" });

            Assert.Throws<InvalidOperationException>(() => _spamProtection.IsSpam(CreateDummyContent()));
        }

        [Theory]
        [InlineData(SpamCommentResponse, true)]
        [InlineData(NotSpamCommentResponse, false)]
        public void IsSpam_Should_Return_Correct_Result(string response, bool result)
        {
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl))).Returns(new HttpFormResponse { Response = response });

            var isSpam = _spamProtection.IsSpam(CreateDummyContent());

            Assert.Equal(result, isSpam);
        }

        [Fact]
        public void IsSpam_Should_Return_False_When_Null_Response_Is_Received()
        {
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl))).Returns(new HttpFormResponse());

            Assert.False(_spamProtection.IsSpam(CreateDummyContent()));
        }

        [Fact]
        public void IsSpam_Should_Should_Use_HttpForm()
        {
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse }).Verifiable();
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl))).Returns(new HttpFormResponse { Response = NotSpamCommentResponse }).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent());

            _httpForm.Verify();
        }

        [Fact]
        public void IsSpam_Should_Forward_To_Next_Handler_When_Content_Is_Not_Spam()
        {
            var next = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = next.Object;

            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl))).Returns(new HttpFormResponse { Response = NotSpamCommentResponse });

            next.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(false).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent());

            next.Verify();
        }

        [Theory]
        [InlineData(SpamCommentResponse, true)]
        [InlineData(NotSpamCommentResponse, false)]
        public void IsSpam_Async_Should_Return_Correct_Result(string response, bool result)
        {
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.PostAsync(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormPostRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onError) => onComplete(new HttpFormResponse { Response = response}));

            _spamProtection.IsSpam(CreateDummyContent(), (source, isSpam) => Assert.Equal(result, isSpam));
        }

        [Fact]
        public void IsSpam_Async_Should_Forward_To_Next_Handler_When_Contnt_Is_Not_Spam()
        {
            var next = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = next.Object;

            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.PostAsync(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormPostRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onError) => onComplete(new HttpFormResponse { Response = NotSpamCommentResponse }));

            next.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>(), It.IsAny<Action<string, bool>>())).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent(), delegate{});

            next.Verify();
        }

        [Fact]
        public void IsSpam_Should_Return_False_When_Exception_Occurrs()
        {
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.PostAsync(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormPostRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onError) => onError(new InvalidOperationException()));

            _spamProtection.IsSpam(CreateDummyContent(), (source, isSpam) => Assert.False(isSpam));
        }

        [Fact]
        public void IsSpam_Should_Forward_To_Next_Handler_When_Exception_Occurrs()
        {
            var next = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = next.Object;

            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.PostAsync(It.Is<HttpFormPostRequest>(r => r.Url == CommentCheckUrl), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormPostRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onError) => onError(new InvalidOperationException()));

            next.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>(), It.IsAny<Action<string, bool>>())).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent(), delegate { });

            next.Verify();
        }

        [Fact]
        public void IsSpam_Async_Should_Should_Use_HttpForm()
        {
            _httpForm.Setup(h => h.Post(It.Is<HttpFormPostRequest>(r => r.Url == ApiKeyCheckUrl))).Returns(new HttpFormResponse { Response = ValidApiKeyResponse });
            _httpForm.Setup(h => h.PostAsync(It.IsAny<HttpFormPostRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent(), delegate{});

            _httpForm.Verify();
        }

        private static SpamCheckContent CreateDummyContent()
        {
            return new SpamCheckContent
                       {
                           Content = "dummy content",
                           ContentType = "social-news",
                           Url = "http://dummystory.com",
                           UserAgent = "Firefox",
                           UserIPAddress = "192.168.0.1",
                           UserName = "dummy user"
                       };
        }
    } 
}