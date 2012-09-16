using System.Collections.Specialized;
using System.Drawing;
using System.IO;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class ImageHandlerFixture : BaseFixture
    {
        private readonly ImageHandler _handler;
        private readonly HttpContextMock _httpContext;

        public ImageHandlerFixture()
        {
            _httpContext = MvcTestHelper.GetHttpContext();
            _httpContext.HttpResponse.Setup(r => r.OutputStream).Returns(new Mock<Stream>().Object);

            var story = new Mock<IStory>();

            story.SetupGet(s => s.VoteCount).Returns(10);
            story.SetupGet(s => s.CreatedAt).Returns(SystemTime.Now().AddDays(-8));

            var repository = new Mock<IStoryRepository>();
            repository.Setup(r => r.FindByUrl(It.IsAny<string>())).Returns(story.Object);

            _handler = new ImageHandler
                           {
                               Colors = new DefaultColors
                                            {
                                               BorderColor = "808080",
                                               TextBackColor = "404040",
                                               TextForeColor = "ffffff",
                                               CountBackColor = "eb4c07",
                                               CountForeColor = "ffffff"
                                            },
                               Width = 100,
                               Height = 22,
                               BorderWidth = 1,
                               FontName = "Tahoma",
                               FontSize = 12,
                               NewStoryCacheDurationInMinutes = 5,
                               ExpiredStoryCacheDurationInMinutes = 4320,
                               StoryRepository = repository.Object,
                               Settings = settings.Object
                           };
        }

        [Fact]
        public void GetInteger_Should_Return_Correct_Value_When_Key_Is_Present()
        {
            var result = ImageHandler.GetInteger(new NameValueCollection { { "key", "10" } }, "Key", 2);

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetInteger_Should_Return_Default_Value_When_Key_Is_Absent()
        {
            var result = ImageHandler.GetInteger(new NameValueCollection(), "Key", 2);

            Assert.Equal(2, result);
        }

        [Fact]
        public void GetInteger_Should_Return_Default_Value_When_Value_Against_The_Key_Is_Not_Valid_Int()
        {
            var result = ImageHandler.GetInteger(new NameValueCollection { { "key", "x" } }, "Key", 2);

            Assert.Equal(2, result);
        }

        [Fact]
        public void GetColor_Should_Return_Correct_Color_When_Key_Is_Present()
        {
            var result = ImageHandler.GetColor(new NameValueCollection { { "background", "ff0000" } }, "background", "000000");

            Assert.Equal(Color.Red.R, result.R);
            Assert.Equal(Color.Red.G, result.G);
            Assert.Equal(Color.Red.B, result.B);
        }

        [Fact]
        public void GetColor_Should_Return_Default_Color_When_Exception_Occurrs_In_Converting_Value()
        {
            var result = ImageHandler.GetColor(new NameValueCollection { { "background", "foobar" } }, "background", "000000");

            Assert.Equal(Color.Black.R, result.R);
            Assert.Equal(Color.Black.G, result.G);
            Assert.Equal(Color.Black.B, result.B);
        }

        [Fact]
        public void ProcessRequest_Should_Render_Cached_Image()
        {
            _httpContext.HttpRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection{{"url", "http://asp.net"}});
            _handler.ProcessRequest(_httpContext.Object);
        }

        [Fact]
        public void ProcessRequest_Should_Render_NonCached_Image()
        {
            _httpContext.HttpRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection { { "url", "http://asp.net" }, { "noCache", "true" } });
            _handler.ProcessRequest(_httpContext.Object);
        }
    }
}