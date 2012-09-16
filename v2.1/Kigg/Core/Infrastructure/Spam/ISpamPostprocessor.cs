namespace Kigg.Infrastructure
{
    using DomainObjects;

    public interface ISpamPostprocessor
    {
        void Process(string source, bool isSpam, string storyUrl, IStory story);

        void Process(string source, bool isSpam, string storyUrl, IComment comment);
    }
}