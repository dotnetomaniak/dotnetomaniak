using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;

namespace Kigg.Extra.BlogEngine
{
    [Extension(DotNetShoutoutCounterGenerator.Description, "1.0", DotNetShoutoutCounterGenerator.BaseUrl)]
    public class DotNetShoutout
    {
        private static string _borderColor;
        private static string _textBackColor;
        private static string _textForeColor;
        private static string _countBackColor;
        private static string _countForeColor;

        static DotNetShoutout()
        {
            Post.Serving += OnPostServing;

            EnsureSettings();
            LoadSettings();
        }

        private static void EnsureSettings()
        {
            ExtensionSettings settings = new ExtensionSettings("DotNetShoutout");

            settings.AddParameter("borderColor", "Border Color", 6, false, false, ParameterType.String);
            settings.AddParameter("textBackColor", "Shout It Backcolor", 6, false, false, ParameterType.String);
            settings.AddParameter("textForeColor", "Shout It Forecolor", 6, false, false, ParameterType.String);
            settings.AddParameter("countBackColor", "Count Backcolor", 6, false, false, ParameterType.String);
            settings.AddParameter("countForeColor", "Count Forcolor", 6, false, false, ParameterType.String);

            settings.AddValue("borderColor", DotNetShoutoutCounterGenerator.DefaultBorderColor);
            settings.AddValue("textBackColor", DotNetShoutoutCounterGenerator.DefaultTextBackColor);
            settings.AddValue("textForeColor", DotNetShoutoutCounterGenerator.DefaultTextForeColor);
            settings.AddValue("countBackColor", DotNetShoutoutCounterGenerator.DefaultCountBackColor);
            settings.AddValue("countForeColor", DotNetShoutoutCounterGenerator.DefaultCountForeColor);

            settings.Help = DotNetShoutoutCounterGenerator.Description;
            settings.IsScalar = true;

            ExtensionManager.ImportSettings(settings);
        }

        private static void LoadSettings()
        {
            ExtensionSettings settings = ExtensionManager.GetSettings("DotNetShoutout");

            _borderColor = settings.GetSingleValue("borderColor");
            _textBackColor = settings.GetSingleValue("textBackColor");
            _textForeColor = settings.GetSingleValue("textForeColor");
            _countBackColor = settings.GetSingleValue("countBackColor");
            _countForeColor = settings.GetSingleValue("countForeColor");
        }

        private static void OnPostServing(object sender, ServingEventArgs e)
        {
            Post post = sender as Post;

            if (post != null)
            {
                e.Body = string.Concat(e.Body, Generate(post));
            }
        }

        private static string Generate(IPublishable post)
        {
            return DotNetShoutoutCounterGenerator.Generate(post.AbsoluteLink.ToString(), post.Title, _borderColor, _textBackColor, _textForeColor, _countBackColor, _countForeColor);
        }
    }
}