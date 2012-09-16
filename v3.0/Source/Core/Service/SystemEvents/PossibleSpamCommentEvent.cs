namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class PossibleSpamCommentEventArgs
    {
        [DebuggerStepThrough]
        public PossibleSpamCommentEventArgs(IComment comment, string source, string detailUrl)
        {
            Comment = comment;
            Source = source;
            DetailUrl = detailUrl;
        }

        public IComment Comment
        {
            get;
            private set;
        }

        public string Source
        {
            get;
            private set;
        }

        public string DetailUrl
        {
            get;
            private set;
        }
    }

    public class PossibleSpamCommentEvent : BaseEvent<PossibleSpamCommentEventArgs>
    {
    }
}