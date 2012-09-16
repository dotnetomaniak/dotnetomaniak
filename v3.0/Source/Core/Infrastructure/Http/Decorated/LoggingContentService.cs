namespace Kigg.Infrastructure
{
    public class LoggingContentService : DecoratedContentService
    {
        public LoggingContentService(IContentService innerService) : base(innerService)
        {
        }

        public override bool IsRestricted(string url)
        {
            Log.Info("Checking restriction of url: {0}", url);

            var result = base.IsRestricted(url);

            if (result)
            {
                Log.Warning("Url restricted: {0}", url);
            }
            else
            {
                Log.Warning("Url is not restricted: {0}", url);
            }

            return result;
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