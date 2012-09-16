namespace Kigg.Infrastructure
{
    public interface IHtmlToStoryContentConverter
    {
        StoryContent Convert(string url, string html);
    }
}