namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class CommentSubmitEventArgs
    {
        [DebuggerStepThrough]
        public CommentSubmitEventArgs(IComment comment, string detailUrl)
        {
            Comment = comment;
            DetailUrl = detailUrl;
        }

        public IComment Comment
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

    public class CommentSubmitEvent : BaseEvent<CommentSubmitEventArgs>
    {
    }
}