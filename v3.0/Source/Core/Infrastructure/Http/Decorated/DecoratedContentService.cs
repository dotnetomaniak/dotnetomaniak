namespace Kigg.Infrastructure
{
    using System.Diagnostics;

    public abstract class DecoratedContentService : IContentService
    {
        private readonly IContentService _innerService;

        protected DecoratedContentService(IContentService innerService)
        {
            _innerService = innerService;
        }

        [DebuggerStepThrough]
        public virtual bool IsRestricted(string url)
        {
            return _innerService.IsRestricted(url);
        }

        [DebuggerStepThrough]
        public virtual StoryContent Get(string url)
        {
            return _innerService.Get(url);
        }

        [DebuggerStepThrough]
        public virtual string ShortUrl(string url)
        {
            return _innerService.ShortUrl(url);
        }
    }
}