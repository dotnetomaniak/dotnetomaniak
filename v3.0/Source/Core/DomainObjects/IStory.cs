namespace Kigg.DomainObjects
{
    using System;
    using System.Collections.Generic;

    public interface IStory : IUniqueNameEntity, ITagContainer
    {
        string Title
        {
            get;
            set;
        }

        string HtmlDescription
        {
            get;
            set;
        }

        string TextDescription
        {
            get;
        }

        string Url
        {
            get;
        }

        ICategory BelongsTo
        {
            get;
        }

        IUser PostedBy
        {
            get;
        }

        string FromIPAddress
        {
            get;
        }

        DateTime LastActivityAt
        {
            get;
        }

        DateTime? ApprovedAt
        {
            get;
        }

        DateTime? PublishedAt
        {
            get;
        }

        int? Rank
        {
            get;
        }

        DateTime? LastProcessedAt
        {
            get;
        }

        ICollection<IVote> Votes
        {
            get;
        }

        ICollection<IMarkAsSpam> MarkAsSpams
        {
            get;
        }

        ICollection<IStoryView> Views
        {
            get;
        }

        ICollection<IComment> Comments
        {
            get;
        }

        ICollection<IUser> Subscribers
        {
            get;
        }

        int VoteCount
        {
            get;
        }

        int MarkAsSpamCount
        {
            get;
        }

        int ViewCount
        {
            get;
        }

        int CommentCount
        {
            get;
        }

        int SubscriberCount
        {
            get;
        }

        void ChangeCategory(ICategory category);

        void View(DateTime at, string fromIpAddress);

        bool CanPromote(IUser byUser);

        bool Promote(DateTime at, IUser byUser, string fromIpAddress);

        bool HasPromoted(IUser byUser);

        bool CanDemote(IUser byUser);

        bool Demote(DateTime at, IUser byUser);

        bool CanMarkAsSpam(IUser byUser);

        bool MarkAsSpam(DateTime at, IUser byUser, string fromIpAddress);

        bool HasMarkedAsSpam(IUser byUser);

        bool CanUnmarkAsSpam(IUser byUser);

        bool UnmarkAsSpam(DateTime at, IUser byUser);

        IComment PostComment(string content, DateTime at, IUser byUser, string fromIpAddress);

        IComment FindComment(Guid id);

        void DeleteComment(IComment comment);

        bool ContainsCommentSubscriber(IUser theUser);

        void SubscribeComment(IUser byUser);

        void UnsubscribeComment(IUser byUser);

        void Approve(DateTime at);

        void Publish(DateTime at, int rank);

        void LastProcessed(DateTime at);

        void ChangeNameAndCreatedAt(string name, DateTime createdAt);
    }
}