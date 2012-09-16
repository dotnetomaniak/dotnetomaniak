using System;
using System.Collections.Specialized;
using System.IO;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class AssetHandlerFixture : BaseFixture
    {
        private const string AssetContent =
                                            @"var Search={init:function()
                                                                            {var defaultSearchText=""Enter your text"";var " +
                                            @"defaultColor='#5a5a5a';var " +
                                            @"txtSearch=$('#txtSearch');if($.trim(txtSearch[0].value).length==0)
                                                                            {txtSearch.val(defaultSearchText)." +
                                            @"css({color:defaultColor});}
                                                                            txtSearch.focus(function()
                                                                            {this.style.color='';if(this.value==defaultSea" +
                                            @"rchText)
                                                                            {this.value='';}}).blur(function()
                                                                            {if(this.value.length==0)
                                                                            {this.style.color=defaultColor;" +
                                            @"this.value=defaultSearchText;}});$('#frmSearch').submit(function(e)
                                                                            {var " +
                                            @"q=$.trim(txtSearch[0].value);if(q.length==0)
                                                                            {txtSearch[0].focus();e.preventDefault();}
                                                                            if(q==default" +
                                            @"SearchText)
                                                                            {e.preventDefault();}});},dispose:function()
                                                                            {$('#txtSearch').unbind();$('#frmSearch').un" +
                                            @"bind();}}";

        [Fact]
        public void GetVersion_Should_Return_Correct_Version()
        {
            string version;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out version)).Returns(false);
            cache.Expect(c => c.Contains(It.IsAny<string>())).Returns(false);
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));

            var assetSettings = new AssetSettingsSection
                               {
                                   CacheDurationInDays = 10,
                                   Compress = true,
                                   Version = "1.5"
                               };

            assetSettings.Assets.Add(new AssetElement { Name = "js" });

            configurationManager.Expect(c => c.GetSection<AssetSettingsSection>(It.IsAny<string>())).Returns(assetSettings);

            string result = AssetHandler.GetVersion("js");

            Assert.Equal("1.5", result);
        }

        [Fact]
        public void ProcessRequest_Should_Write_Asset()
        {
            var configuration = new Mock<IConfigurationManager>();

            var assetSettings = new AssetSettingsSection
                               {
                                   CacheDurationInDays = 10,
                                   Version = "1.5",
                                   GenerateETag = false,
                                   Compress = false
                               };

            assetSettings.Assets.Add(new AssetElement { Name = "js", ContentType = "application/x-javascript", Directory = "~/Assets/JavaScript", Files = "Search.min.js" });

            var responseStream = new MemoryStream();
            var httpContext = MvcTestHelper.GetHttpContext();

            httpContext.HttpRequest.ExpectGet(r => r.QueryString).Returns(new NameValueCollection { { "name", "js" } });
            httpContext.HttpResponse.ExpectGet(r => r.OutputStream).Returns(responseStream);

            configuration.Expect(c => c.GetSection<AssetSettingsSection>(It.IsAny<string>())).Returns(assetSettings);

            string content;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out content)).Returns(false);
            file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(AssetContent);

            var handler = new AssetHandler{ Configuration = configuration.Object, FileReader = file.Object };

            handler.ProcessRequest(httpContext.Object);

            Assert.Throws<ObjectDisposedException>(() => responseStream.Seek(0, SeekOrigin.Begin));
        }

        [Fact]
        public void ProcessRequest_Should_Not_Write_Assert_When_Asset_Is_Not_Modified()
        {
            var configuration = new Mock<IConfigurationManager>();

            var assetSettings = new AssetSettingsSection
                                    {
                                        CacheDurationInDays = 10,
                                        Version = "1.5"
                                    };

            assetSettings.Assets.Add(new AssetElement { Name = "js", ContentType = "application/x-javascript", Directory = "~/Assets/JavaScript", Files = "Search.min.js" });

            var responseStream = new MemoryStream();
            var httpContext = MvcTestHelper.GetHttpContext();

            httpContext.HttpRequest.ExpectGet(r => r.QueryString).Returns(new NameValueCollection { { "name", "js" } });
            httpContext.HttpRequest.ExpectGet(r => r.Headers).Returns(new NameValueCollection { { "If-None-Match", (AssetContent + "\r\n\r\n").Hash() } });
            httpContext.HttpResponse.ExpectGet(r => r.OutputStream).Returns(responseStream);

            configuration.Expect(c => c.GetSection<AssetSettingsSection>(It.IsAny<string>())).Returns(assetSettings);

            string content;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out content)).Returns(false);
            file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(AssetContent);

            var handler = new AssetHandler { Configuration = configuration.Object, FileReader = file.Object };

            handler.ProcessRequest(httpContext.Object);

            Assert.True(responseStream.Length == 0);
        }

        [Fact]
        public void ProcessRequest_Should_Cache_Asset()
        {
            var configuration = new Mock<IConfigurationManager>();

            var assetSettings = new AssetSettingsSection
                                    {
                                        CacheDurationInDays = 10,
                                        Version = "1.5"
                                    };

            assetSettings.Assets.Add(new AssetElement { Name = "js", ContentType = "application/x-javascript", Directory = "~/Assets/JavaScript", Files = "Search.min.js" });

            var responseStream = new MemoryStream();
            var httpContext = MvcTestHelper.GetHttpContext();

            httpContext.HttpRequest.ExpectGet(r => r.QueryString).Returns(new NameValueCollection { { "name", "js" } });
            httpContext.HttpResponse.ExpectGet(r => r.OutputStream).Returns(responseStream);

            configuration.Expect(c => c.GetSection<AssetSettingsSection>(It.IsAny<string>())).Returns(assetSettings);

            string content;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out content)).Returns(false);
            cache.Expect(c => c.Contains(It.IsAny<string>())).Returns(false);
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<HandlerCacheItem>(), It.IsAny<DateTime>())).Verifiable();
            file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(AssetContent);

            var handler = new AssetHandler { Configuration = configuration.Object, FileReader = file.Object };

            handler.ProcessRequest(httpContext.Object);

            cache.Verify();
        }
    }
}