using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class PageGlimpseThumbnailFixture : BaseFixture
    {
        private const string Key = "MyDevkey";
        private const string Url = "http://dotnetshoutout.com";

        private readonly Mock<IHttpForm> _httpForm;
        private readonly PageGlimpseThumbnail _thumbnail;

        public PageGlimpseThumbnailFixture()
        {
            _httpForm = new Mock<IHttpForm>();

            _thumbnail = new PageGlimpseThumbnail(Key, "http://images.pageglimpse.com/v1/thumbnails", _httpForm.Object);
        }

        [Fact]
        public void For_Should_Return_Correct_Thumbnail_Url()
        {
            var thumbnailUrl = _thumbnail.For(Url, ThumbnailSize.Small);

            Assert.Equal("http://images.pageglimpse.com/v1/thumbnails?devkey={0}&url={1}&size=small&root=no&nothumb={2}/Assets/Images/pg-preview-na.jpg".FormatWith(Key, Url, settings.Object.RootUrl), thumbnailUrl);
        }

        [Fact]
        public void Capture_Should_Use_HttpForm()
        {
            _httpForm.Setup(h => h.GetAsync(It.IsAny<HttpFormGetRequest>())).Verifiable();

            _thumbnail.Capture(Url);

            _httpForm.Verify();
        }
    }
}