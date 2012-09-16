using System.Diagnostics;
using System.Windows.Forms;

using WindowsLive.Writer.Api;

namespace Kigg.Extra.LiveWriter
{
    [WriterPlugin("F52AEAAB-00C3-44d6-BF66-37671A72F336", "DotNetShoutout", PublisherUrl = DotNetShoutoutCounterGenerator.BaseUrl, Description = DotNetShoutoutCounterGenerator.Description, ImagePath = "icon.png", HasEditableOptions = true)]
    public class DotNetShoutout : HeaderFooterSource
    {
        private Settings _settings;

        public override void Initialize(IProperties pluginOptions)
        {
            base.Initialize(pluginOptions);

            _settings = new Settings(pluginOptions);
        }

        public override bool RequiresPermalink
        {
            [DebuggerStepThrough]
            get
            {
                return true;
            }
        }

        public override void EditOptions(IWin32Window dialogOwner)
        {
            using (Options form = new Options(_settings))
            {
                form.ShowDialog(dialogOwner);
            }
        }

        public override string GeneratePreviewHtml(ISmartContent smartContent, IPublishingContext publishingContext, out Position position)
        {
            string url = publishingContext.PostInfo.Permalink;

            if (string.IsNullOrEmpty(url))
            {
                url = DotNetShoutoutCounterGenerator.BaseUrl;
            }

            return GenerateLink(url, publishingContext.PostInfo.Title, out position);
        }

        public override string GeneratePublishHtml(IWin32Window dialogOwner, ISmartContent smartContent, IPublishingContext publishingContext, bool publish, out Position position)
        {
            if (string.IsNullOrEmpty(publishingContext.PostInfo.Permalink))
            {
                Debug.Fail("No permalink!");
                position = Position.Footer;
                return string.Empty;
            }

            return GenerateLink(publishingContext.PostInfo.Permalink, publishingContext.PostInfo.Title, out position);
        }

        private string GenerateLink(string url, string title, out Position position)
        {
            string html = DotNetShoutoutCounterGenerator.Generate(url, title, _settings.BorderColor, _settings.ShoutItBackColor, _settings.ShoutItForeColor, _settings.CountBackColor, _settings.CountForeColor);

            _settings.Content = html;

            position = Position.Footer;

            return _settings.Content;
        }
    }
}