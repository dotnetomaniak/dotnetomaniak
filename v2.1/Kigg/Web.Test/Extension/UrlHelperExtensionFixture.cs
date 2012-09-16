using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class UrlHelperExtensionFixture : BaseFixture
    {
        private readonly UrlHelper _urlHelper;

        public UrlHelperExtensionFixture()
        {
            var httpContext = MvcTestHelper.GetHttpContext("/Kigg", null, null);

            _urlHelper = new UrlHelper(new RequestContext(httpContext.Object, new RouteData()), new RouteCollection());
        }

        [Fact]
        public void Asset_Should_Return_Correct_Url()
        {
            var assetSettings = new AssetSettingsSection
                               {
                                   CacheDurationInDays = 10,
                                   Compress = true,
                                   Version = "1.5"
                               };

            assetSettings.Assets.Add(new AssetElement { Name = "js" });

            configurationManager.Expect(c => c.GetSection<AssetSettingsSection>(It.IsAny<string>())).Returns(assetSettings);

            var url = _urlHelper.Asset("js");

            Assert.Contains("asset.axd?name={0}".FormatWith("js"), url);
        }

        [Fact]
        public void Image_Should_Return_Correct_Url()
        {
            var url = _urlHelper.Image("rss.jpg");

            Assert.Contains("Assets/Images/{0}".FormatWith("rss.jpg"), url);
        }
    }
}