using Xunit;

namespace Kigg.Web.Test
{
    public class AssetSettingsSectionFixture
    {
        private readonly AssetSettingsSection _settings;

        public AssetSettingsSectionFixture()
        {
            _settings = new AssetSettingsSection();
        }

        [Fact]
        public void Setting_And_Getting_Compress_Should_Return_The_Same_Value()
        {
            _settings.Compress = false;

            Assert.False(_settings.Compress);
        }

        [Fact]
        public void Setting_And_Getting_GenerateETag_Should_Return_The_Same_Value()
        {
            _settings.GenerateETag = false;

            Assert.False(_settings.GenerateETag);
        }

        [Fact]
        public void Setting_And_Getting_CacheDurationInDays_Should_Return_The_Same_Value()
        {
            _settings.CacheDurationInDays = 10;

            Assert.Equal(10, _settings.CacheDurationInDays);
        }

        [Fact]
        public void Setting_And_Getting_Version_Should_Return_The_Same_Value()
        {
            _settings.Version = "2.0.0.1";

            Assert.Equal("2.0.0.1", _settings.Version);
        }

        [Fact]
        public void Assets_Should_Not_Be_Null()
        {
            Assert.NotNull(_settings.Assets);
        }
    }
}