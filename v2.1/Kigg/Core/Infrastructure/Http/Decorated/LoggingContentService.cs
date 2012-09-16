namespace Kigg.Infrastructure
{
    public class LoggingContentService : DecoratedContentService
    {
        public LoggingContentService(IContentService innerService) : base(innerService)
        {
        }

        public override StoryContent Get(string url)
        {
            Log.Info("Retrieving content of url: {0}", url);

            var result = base.Get(url);

            if (result == StoryContent.Empty)
            {
                Log.Warning("Unable to retrive the content for: {0}", url);
            }
            else
            {
                Log.Info("Content retrieved for: {0}", url);
            }

            return result;
        }

        public override void Ping(string url, string title, string fromUrl, string excerpt, string siteTitle)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");
            Check.Argument.IsNotEmpty(title, "title");
            Check.Argument.IsNotEmpty(fromUrl, "fromUrl");
            Check.Argument.IsNotEmpty(excerpt, "excerpt");
            Check.Argument.IsNotEmpty(siteTitle, "siteTitle");

            Log.Info("Pinging url: {0}", url);

            base.Ping(url, title, fromUrl, excerpt, siteTitle);

            Log.Info("Url pinged: {0}", url);
        }

        public override string ShortUrl(string url)
        {
            Check.Argument.IsNotEmpty(url, "url");

            Log.Info("Shortening url: {0}", url);

            var result = base.ShortUrl(url);

            if (string.IsNullOrEmpty(result))
            {
                Log.Warning("Unable to shorten Url: {0}", url);
            }
            else
            {
                Log.Info("Url shortened: {0}, {1}", url, result);
            }

            return result;
        }
    }
}