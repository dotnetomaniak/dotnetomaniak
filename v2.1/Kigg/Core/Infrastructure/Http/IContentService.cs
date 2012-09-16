namespace Kigg.Infrastructure
{
    public interface IContentService
    {
        StoryContent Get(string url);

        void Ping(string url, string title, string fromUrl, string excerpt, string siteTitle);

        string ShortUrl(string url);
    }
}