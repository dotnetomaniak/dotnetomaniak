namespace Kigg.Service
{
    using DomainObjects;

    public interface ISpamPostprocessor
    {
        void Process(string source, bool isSpam, string detailUrl, IStory story);

        void Process(string source, bool isSpam, string detailUrl, IComment comment);
    }
}