using System.Collections.Specialized;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class ContentServiceFixture
    {
        private readonly Mock<IHttpForm> _httpForm;
        private readonly Mock<IHtmlToStoryContentConverter> _converter;

        private readonly ContentService _contentService;

        public ContentServiceFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _converter = new Mock<IHtmlToStoryContentConverter>();

            _contentService = new ContentService(_httpForm.Object, _converter.Object, "http://tinyurl.com/api-create.php?url={0}");
        }

        [Fact]
        public void Get_Should_Use_HttpForm()
        {
            Get();

            _httpForm.Verify();
        }

        [Fact]
        public void Get_Should_Use_Converter_To_Remove_Unsupported_Tags()
        {
            Get();

            _converter.Verify();
        }

        [Fact]
        public void Ping_Should_Use_HttpForm()
        {
            _httpForm.Expect(h => h.PostAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>())).Verifiable();

            _contentService.Ping("http://www.test.com/trackback", "A Story", "http://www.story.com", "Story Excerpt", "DotNetShoutout.com");

            _httpForm.Verify();
        }

        [Fact]
        public void ShortUrl_Should_Uses_HttpForm()
        {
            _httpForm.Expect(h => h.Get(It.IsAny<string>())).Returns("http://tinyurl.com/abcs").Verifiable();

            _contentService.ShortUrl("http://averylongurl.com/foo/bar.aspx");

            _httpForm.Verify();
        }

        private void Get()
        {
            _httpForm.Expect(h => h.Get(It.IsAny<string>())).Returns("html snippet").Verifiable();
            _converter.Expect(c => c.Convert(It.IsAny<string>(), It.IsAny<string>())).Returns((StoryContent) null).Verifiable();

            _contentService.Get("http://weblogs.asp.net/rashid");
        }
    }
}