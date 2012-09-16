using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class ContentServiceFixture
    {
        private readonly Mock<IHttpForm> _httpForm;
        private readonly Mock<IHtmlToStoryContentConverter> _converter;
        private readonly Mock<IFile> _file;

        private readonly ContentService _contentService;

        public ContentServiceFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _converter = new Mock<IHtmlToStoryContentConverter>();
            _file = new Mock<IFile>();

            _file.Setup(f => f.ReadAllLine(It.IsAny<string>())).Returns(new[]
                                                                             {
                                                                                 "http://dotnetkicks.com",
                                                                                 "http://dzone.com",
                                                                                 "http://rsscode.net",
                                                                                 "http://silverlightshow.net/news/"
                                                                             });

            _contentService = new ContentService(_httpForm.Object, _converter.Object, _file.Object, "Foo","http://tinyurl.com/api-create.php?url={0}");
        }

        [Fact]
        public void IsRestricted_Should_Return_True_When_Prefix_Is_Matched_With_3W()
        {
            bool result = _contentService.IsRestricted("http://www.rsscode.net/Default.aspx/7765");

            Assert.True(result);
        }

        [Fact]
        public void IsRestricted_Should_Return_True_When_Prefix_Is_Matched_Without_3W()
        {
            bool result = _contentService.IsRestricted("http://silverlightshow.net/news/Adding-Silverlight-Toolkit-Controls-to-the-Visual-Studio-and-Blend-Toolbox.aspx");

            Assert.True(result);
        }

        [Fact]
        public void IsRestricted_Should_Return_False_When_Not_Matched()
        {
            bool result = _contentService.IsRestricted("http://weblogs.asp.net/scottgu/archive/2009/01/27/asp-net-mvc-1-0-release-candidate-now-available.aspx");

            Assert.False(result);
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
        public void ShortUrl_Should_Uses_HttpForm()
        {
            _httpForm.Setup(h => h.Get(It.IsAny<HttpFormGetRequest>())).Returns(new HttpFormResponse()).Verifiable();

            _contentService.ShortUrl("http://averylongurl.com/foo/bar.aspx");

            _httpForm.Verify();
        }

        private void Get()
        {
            _httpForm.Setup(h => h.Get(It.IsAny<HttpFormGetRequest>())).Returns(new HttpFormResponse{ Response = "html snippet"}).Verifiable();
            _converter.Setup(c => c.Convert(It.IsAny<string>(), It.IsAny<string>())).Returns((StoryContent) null).Verifiable();

            _contentService.Get("http://weblogs.asp.net/rashid");
        }
    }
}