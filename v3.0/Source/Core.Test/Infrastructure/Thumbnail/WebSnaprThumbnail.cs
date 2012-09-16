using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class WebSnaprThumbnailFixture
    {
        private const string Key = "MyKey";
        private const string Url = "http://dotnetshoutout.com";

        private readonly Mock<IHttpForm> _httpForm;
        private readonly WebSnaprThumbnail _thumbnail;

        public WebSnaprThumbnailFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _thumbnail = new WebSnaprThumbnail(Key, "http://images.websnapr.com", _httpForm.Object);
        }

        [Fact]
        public void For_Should_Return_Correct_Thumbnail_Url()
        {
            var thumbnailUrl = _thumbnail.For(Url, ThumbnailSize.Small);

            Assert.Equal("http://images.websnapr.com/?key={0}&url={1}&size=T".FormatWith(Key, Url), thumbnailUrl);
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