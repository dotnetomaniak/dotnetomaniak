namespace Kigg.Infrastructure
{
    public interface IContentService
    {
        bool IsRestricted(string url);

        StoryContent Get(string url);

        string ShortUrl(string url);
    }
}