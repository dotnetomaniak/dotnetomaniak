namespace Kigg.Infrastructure
{
    public abstract class DecoratedContentService : IContentService
    {
        private readonly IContentService _innerService;

        protected DecoratedContentService(IContentService innerService)
        {
            _innerService = innerService;
        }

        public virtual StoryContent Get(string url)
        {
            return _innerService.Get(url);
        }

        public virtual void Ping(string url, string title, string fromUrl, string excerpt, string siteTitle)
        {
            _innerService.Ping(url, title, fromUrl, excerpt, siteTitle);
        }

        public virtual string ShortUrl(string url)
        {
            return _innerService.ShortUrl(url);
        }
    }
}