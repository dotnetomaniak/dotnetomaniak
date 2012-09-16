namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class CommentMarkAsOffendedEventArgs
    {
        [DebuggerStepThrough]
        public CommentMarkAsOffendedEventArgs(IComment comment, IUser user, string detailUrl)
        {
            Comment = comment;
            User = user;
            DetailUrl = detailUrl;
        }

        public IComment Comment
        {
            get;
            private set;
        }

        public IUser User
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

    public class CommentMarkAsOffendedEvent : BaseEvent<CommentMarkAsOffendedEventArgs>
    {
    }
}