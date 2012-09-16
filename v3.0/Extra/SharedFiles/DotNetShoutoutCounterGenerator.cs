namespace Kigg
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;

    internal static class DotNetShoutoutCounterGenerator
    {
        public const string BaseUrl = "http://dotnetshoutout.com";
        private const string Template = "<div class=\"shoutIt\">" +
                                            "<a rev=\"vote-for\" href=\"{0}/Submit?url={1}{2}\">" +
                                                "<img alt=\"Shout it\" src=\"{3}\" style=\"border:0px\"/>" +
                                            "</a>" +
                                        "</div>";

        public const string DefaultBorderColor = "808080";
        public const string DefaultTextBackColor = "404040";
        public const string DefaultTextForeColor = "ffffff";
        public const string DefaultCountBackColor = "eb4c07";
        public const string DefaultCountForeColor = "ffffff";

        public const string Description = "Put DotNetShoutout.com counter in your blog posts.";

        private static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        public static string Generate(string url, string title, string borderColor, string textBackColor, string textForeColor, string countBackColor, string countForeColor)
        {
            string imageUrl = ImageSource(url, borderColor, textBackColor, textForeColor, countBackColor, countForeColor);

            if (!string.IsNullOrEmpty(title))
            {
                title = "&amp;title=" + HttpUtility.UrlEncode(title);
            }

            string result = string.Format(CurrentCulture, Template, BaseUrl, HttpUtility.UrlEncode(url), title, imageUrl);

            return result;
        }

        public static string ImageSource(string url, string borderColor, string textBackColor, string textForeColor, string countBackColor, string countForeColor)
        {
            StringBuilder colors = new StringBuilder();

            AddIfPresent(colors, "borderColor", borderColor, DefaultBorderColor);
            AddIfPresent(colors, "textBackColor", textBackColor, DefaultTextBackColor);
            AddIfPresent(colors, "textForeColor", textForeColor, DefaultTextForeColor);
            AddIfPresent(colors, "countBackColor", countBackColor, DefaultCountBackColor);
            AddIfPresent(colors, "countForeColor", countForeColor, DefaultTextForeColor);

            string imageSource = url;

            if (colors.Length > 0)
            {
                imageSource += colors.ToString();
            }

            string result = string.Format(CurrentCulture, "{0}/image.axd?url={1}", BaseUrl, imageSource);

            return result;
        }

        private static void AddIfPresent(StringBuilder output, string name, string value, string defaultValue)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.StartsWith(("#")))
                {
                    value = value.Substring(1);
                }

                if (string.Compare(value, defaultValue, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    output.AppendFormat("&{0}={1}", name, value);
                }
            }
        }
    }
}