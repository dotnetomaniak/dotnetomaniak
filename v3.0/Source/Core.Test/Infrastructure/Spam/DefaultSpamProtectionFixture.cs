using System;
using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class DefaultSpamProtectionFixture : BaseFixture
    {
        private const int StoryThreshold = 3;
        private const int CommentThreshold = 3;

        private readonly Mock<IHttpForm> _httpForm;

        private readonly Mock<ISpamWeightCalculator> _calculator1;
        private readonly Mock<ISpamWeightCalculator> _calculator2;
        private readonly Mock<ISpamWeightCalculator> _calculator3;

        private readonly ISpamProtection _spamProtection;

        public DefaultSpamProtectionFixture()
        {
            _httpForm = new Mock<IHttpForm>();

            _calculator1 = new Mock<ISpamWeightCalculator>();
            _calculator2 = new Mock<ISpamWeightCalculator>();
            _calculator3 = new Mock<ISpamWeightCalculator>();

            _spamProtection = new DefaultSpamProtection(settings.Object, _httpForm.Object, StoryThreshold, new[] { _calculator1.Object, _calculator2.Object, _calculator3.Object }, new[] { _calculator1.Object, _calculator2.Object, _calculator3.Object }, CommentThreshold, new[] { _calculator1.Object, _calculator2.Object, _calculator3.Object });
        }

        [Fact]
        public void IsSpam_Should_Return_True_For_Comment_When_Weight_Calculators_Return_More_Than_CommentThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);

            var result = _spamProtection.IsSpam(CreateDummyContent(settings.Object.RootUrl + "\\A-Story"));

            Assert.True(result);
        }

        [Fact]
        public void IsSpam_Should_Return_False_For_Comment_When_Weight_Calculators_Return_Less_Than_CommentThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);

            _spamProtection.NextHandler = new Mock<ISpamProtection>().Object;
            var result = _spamProtection.IsSpam(CreateDummyContent(settings.Object.RootUrl + "\\A-Story"));

            Assert.False(result);
        }

        [Fact]
        public void IsSpam_Should_Return_True_For_Story_When_Weight_Calculators_Return_More_Than_StoryThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(1);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(1);

            _httpForm.Setup(h => h.Get(It.IsAny<HttpFormGetRequest>())).Returns(new HttpFormResponse { Response = "Dummy Content"});

            var result = _spamProtection.IsSpam(CreateDummyContent("http://asp.net"));

            Assert.True(result);
        }

        [Fact]
        public void IsSpam_Async_Should_Return_True_For_Story_When_Weight_Calculators_Return_More_Than_StoryThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(1);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(1);

            _httpForm.Setup(h => h.GetAsync(It.IsAny<HttpFormGetRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormGetRequest response, Action<HttpFormResponse> onComplete, Action<Exception> onException) => onComplete(new HttpFormResponse{Response = "This is a dummy response"}));

            _spamProtection.IsSpam(CreateDummyContent("http://asp.net"), (source, isSpam) => Assert.True(isSpam));
        }

        [Fact]
        public void IsSpam_Async_Should_Forward_To_Next_Handler_For_Story_When_Weight_Calculators_Return_Less_Than_StoryThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);

            _httpForm.Setup(h => h.GetAsync(It.IsAny<HttpFormGetRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormGetRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onException) => onComplete(new HttpFormResponse{Response = "This is a dummy response"}));

            var nextHandler = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = nextHandler.Object;

            _spamProtection.IsSpam(CreateDummyContent("http://asp.net"), delegate{});

            nextHandler.Verify();
        }

        [Fact]
        public void IsSpam_Async_Should_Return_False_When_Weight_Calculators_Return_Less_Than_StoryThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);

            _httpForm.Setup(h => h.GetAsync(It.IsAny<HttpFormGetRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormGetRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onException) => onComplete(new HttpFormResponse { Response = "This is a dummy response" }));

            _spamProtection.IsSpam(CreateDummyContent("http://asp.net"), (respone, isSpam) => Assert.False(isSpam));
        }

        [Fact]
        public void IsSpam_Async_Should_Forward_To_Next_Handler_For_Story_When_Exception_Occurs()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);

            _httpForm.Setup(h => h.GetAsync(It.IsAny<HttpFormGetRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormGetRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onException) => onException(new Exception()));

            var nextHandler = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = nextHandler.Object;

            _spamProtection.IsSpam(CreateDummyContent("http://asp.net"), delegate { });

            nextHandler.Verify();
        }

        [Fact]
        public void IsSpam_Async_Should_Return_False_For_Story_When_Exception_Occurs()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);

            _httpForm.Setup(h => h.GetAsync(It.IsAny<HttpFormGetRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormGetRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onException) => onException(new Exception()));

            _spamProtection.IsSpam(CreateDummyContent("http://asp.net"), (source, isSpam) => Assert.False(isSpam));
        }

        [Fact]
        public void IsSpam_Async_Should_Not_Run_Remote_Weight_Calculators_For_Story_When_Local_Calculators_Return_More_Than_StoryThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);

            _spamProtection.IsSpam(CreateDummyContent("http://asp.net"), (source, isSpam) => Assert.True(isSpam));
        }

        [Fact]
        public void IsSpam_Async_Should_Return_True_For_Comment_When_Weight_Calculators_Return_More_Than_CommentThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(2);

            _spamProtection.IsSpam(CreateDummyContent(settings.Object.RootUrl + "\\A-Story"), (source, isSpam) => Assert.True(isSpam));
        }

        [Fact]
        public void IsSpam_Async_Should_Forward_To_Next_Handler_For_Comment_When_Weight_Calculators_Return_Less_Than_CommentThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);

            var nextHandler = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = nextHandler.Object;

            _spamProtection.IsSpam(CreateDummyContent(settings.Object.RootUrl + "\\A-Story"), delegate { });

            nextHandler.Verify();
        }

        [Fact]
        public void IsSpam_Async_Should_Return_False_For_Comment_When_Weight_Calculators_Return_Less_Than_CommentThresold()
        {
            _calculator1.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator2.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);
            _calculator3.Setup(c => c.Calculate(It.IsAny<string>())).Returns(0);

            _spamProtection.IsSpam(CreateDummyContent(settings.Object.RootUrl + "\\A-Story"), (source, isSpam) => Assert.False(isSpam));
        }

        private static SpamCheckContent CreateDummyContent(string url)
        {
            return new SpamCheckContent
                       {
                           ContentType = "social-news",
                           Url = url,
                           Content = "This is content to check spam",
                           UserAgent = "Firefox",
                           UserIPAddress = "192.168.0.1",
                           UserName = "dummy user"
                       };
        }
    }
}