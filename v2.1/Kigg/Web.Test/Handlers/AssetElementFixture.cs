using Xunit;

namespace Kigg.Web.Test
{
    public class AssetElementFixture
    {
        private readonly AssetElement _element;

        public AssetElementFixture()
        {
            _element = new AssetElement();
        }

        [Fact]
        public void Setting_And_Getting_Name_Should_Return_The_Same_Value()
        {
            _element.Name = "JS";

            Assert.Equal("JS", _element.Name);
        }

        [Fact]
        public void Setting_And_Getting_Content_Type_Should_Return_The_Same_Value()
        {
            _element.ContentType = "application/javascript";

            Assert.Equal("application/javascript", _element.ContentType);
        }

        [Fact]
        public void Setting_And_Getting_Compress_Should_Return_The_Same_Value()
        {
            _element.Compress = false;

            Assert.False(_element.Compress);
        }

        [Fact]
        public void Setting_And_Getting_GenerateETag_Should_Return_The_Same_Value()
        {
            _element.GenerateETag = false;

            Assert.False(_element.GenerateETag);
        }

        [Fact]
        public void Setting_And_Getting_Version_Should_Return_The_Same_Value()
        {
            _element.Version = "1.0.5.1";

            Assert.Equal("1.0.5.1", _element.Version);
        }

        [Fact]
        public void Setting_And_Getting_CacheDurationInDays_Should_Return_The_Same_Value()
        {
            _element.CacheDurationInDays = 10;

            Assert.Equal(10, _element.CacheDurationInDays);
        }

        [Fact]
        public void Setting_And_Getting_Directory_Should_Return_The_Same_Value()
        {
            _element.Directory = "Assests";

            Assert.Equal("Assests", _element.Directory);
        }

        [Fact]
        public void Setting_And_Getting_Files_Should_Return_The_Same_Value()
        {
            _element.Files = "foo.js;bar.js";

            Assert.Equal("foo.js;bar.js", _element.Files);
        }
    }
}