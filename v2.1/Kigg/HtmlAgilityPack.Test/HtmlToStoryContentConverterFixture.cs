using System;
using System.IO;

using Xunit;

namespace Kigg.Infrastructure.HtmlAgilityPack.Test
{
    public class HtmlToStoryContentConverterFixture
    {
        private readonly HtmlToStoryContentConverter _converter;

        public HtmlToStoryContentConverterFixture()
        {
            string[] xPaths = LargeStrings.XPaths.Split(new[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            _converter = new HtmlToStoryContentConverter(new HtmlSanitizer(), xPaths);
        }

        [Fact]
        public void Constructor_Should_Throw_Exception_When_ContentNode_Text_File_Is_Missing()
        {
            Assert.Throws<FileNotFoundException>(() => new HtmlToStoryContentConverter(new HtmlSanitizer(), "contentNodes.txt"));
        }

        [Fact]
        public void Convert_Should_Return_Correct_StoryContent()
        {
            var result = _converter.Convert("http://asp.net", LargeStrings.Html1);

            Assert.Equal("The Official Microsoft ASP.NET Site", result.Title);
            Assert.Equal(LargeStrings.Text, result.Description);
            Assert.Null(result.TrackBackUrl);
        }

        [Fact]
        public void Convert_Should_Return_Correct_TrackbackUrl_When_Rdf_Is_Present()
        {
            var result = _converter.Convert("http://weblogs.asp.net/rashid/archive/2008/03/28/asp-net-mvc-action-filter-caching-and-compression.aspx", LargeStrings.Html2);

            Assert.Equal("http://weblogs.asp.net/rashid/trackback.ashx?PostID=6038280", result.TrackBackUrl);
        }

        [Fact]
        public void Convert_Should_Return_Correct_TrackbackUrl_For_TypePad()
        {
            var result = _converter.Convert("http://bradwilson.typepad.com/blog/2008/11/xunitnet-11-released.html", LargeStrings.Html3);

            Assert.Equal("http://www.typepad.com/t/trackback/195960/36302666", result.TrackBackUrl);
        }

        [Fact]
        public void Convert_Should_Return_Correct_TrackbackUrl_For_B2Evolution()
        {
            var result = _converter.Convert("http://blogs.lessthandot.com/index.php/DataMgmt/DataDesign/sql-friday-the-best-sql-server-links-of--1", LargeStrings.Html4);

            Assert.Equal("http://blogs.lessthandot.com/htsrv/trackback.php?tb_id=244", result.TrackBackUrl);
        }

        [Fact]
        public void Convert_Should_Return_Correct_TrackbackUrl_For_Wordpress()
        {
            var result = _converter.Convert("http://blog.stackoverflow.com/2008/09/then-a-miracle-occurs-public-beta/", LargeStrings.Html5);

            Assert.Equal("http://blog.stackoverflow.com/2008/09/then-a-miracle-occurs-public-beta/trackback/", result.TrackBackUrl);
        }
    }
}