namespace Kigg.Infrastructure.DomainRepositoryExtensions
{
    using System;
    using System.Diagnostics;
    using System.Security.Permissions;

    using Repository;
    using DomainObjects;
    
    //[StrongNameIdentityPermissionAttribute(SecurityAction.Demand, PublicKey = "00240000048000009400000006020000002400005253413100040000010001007f9d35f7398744b708ea57288eb1911f9a46cad961be6baacb27e07d87809a20bf135f61833c121b541676fa95fd373d44ac4404ffae85e5199d0828c00991362b34f93002791f16d901f1714ba3abaa9208f8c41660f57ae0e7732e3655d5d4d9c53521cdb1b0636a78aac7407e194b7bee1a45b229e35559ee6c0a5b11b5b9")]
    public static class StoryExtension
    {
        [DebuggerStepThrough]
        public static int GetViewCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<IStoryViewRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        public static int GetVoteCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<IVoteRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        public static int GetMarkAsSpamCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<IMarkAsSpamRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        public static int GetCommentCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<ICommentRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        public static int GetSubscriberCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<ICommentSubscribtionRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        private static int GetCount<T>(Guid storyId) where T : class, ICountByStoryRepository
        {
            return IoC.Resolve<T>().CountByStory(storyId);
        }

        [DebuggerStepThrough]
        public static IStoryView AddView(this IStory forStory, DateTime at, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var view = IoC.Resolve<IDomainObjectFactory>().CreateStoryView(forStory, at, fromIpAddress);
            IoC.Resolve<IStoryViewRepository>().Add(view);
            
            return view;
        }

        [DebuggerStepThrough]
        public static IVote AddVote(this IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var vote = IoC.Resolve<IDomainObjectFactory>().CreateStoryVote(forStory, at, byUser, fromIpAddress);
            IoC.Resolve<IVoteRepository>().Add(vote);

            return vote;
        }

        [DebuggerStepThrough]
        public static void RemoveVote(this IStory fromStory, IVote vote)
        {
            Check.Argument.IsNotNull(fromStory, "fromStory");
            Check.Argument.IsNotNull(vote, "vote");
            IoC.Resolve<IVoteRepository>().Remove(vote);            
        }

        [DebuggerStepThrough]
        public static IVote GetVote(this IStory forStory,IUser byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");
            return IoC.Resolve<IVoteRepository>().FindById(forStory.Id, byUser.Id);
        }

        [DebuggerStepThrough]
        public static IMarkAsSpam MarkSpam(this IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var spamStory = IoC.Resolve<IDomainObjectFactory>().CreateMarkAsSpam(forStory, at, byUser, fromIpAddress);
            IoC.Resolve<IMarkAsSpamRepository>().Add(spamStory);
            
            return spamStory;
        }

        [DebuggerStepThrough]
        public static void UnmarkSpam(this IStory forStory, IMarkAsSpam spam)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(spam, "spam");
            IoC.Resolve<IMarkAsSpamRepository>().Remove(spam);
        }

        [DebuggerStepThrough]
        public static IMarkAsSpam GetMarkAsSpam(this IStory forStory, IUser byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");
            return IoC.Resolve<IMarkAsSpamRepository>().FindById(forStory.Id, byUser.Id);
        }

        [DebuggerStepThrough]
        public static IComment AddComment(this IStory forStory, string content, DateTime at, IUser byUser, string fromIpAddress)
        {
            var comment = IoC.Resolve<IDomainObjectFactory>().CreateComment(forStory, content, at, byUser, fromIpAddress);
            IoC.Resolve<ICommentRepository>().Add(comment);
            return comment;
        }

        [DebuggerStepThrough]
        public static IComment GetComment(this IStory forStory, Guid commentId)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotEmpty(commentId, "commentId");
            return IoC.Resolve<ICommentRepository>().FindById(forStory.Id, commentId);
        }

        [DebuggerStepThrough]
        public static void RemoveComment(this IStory forStory, IComment comment)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(comment, "comment");
            IoC.Resolve<ICommentRepository>().Remove(comment);
        }

        [DebuggerStepThrough]
        public static ICommentSubscribtion GetCommentSubscribtion(this IStory forStory, IUser theUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(theUser, "theUser");
            return IoC.Resolve<ICommentSubscribtionRepository>().FindById(forStory.Id, theUser.Id);
        }

        [DebuggerStepThrough]
        public static ICommentSubscribtion AddCommentSubscribtion(this IStory forStory, IUser byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            var subscribtion = forStory.GetCommentSubscribtion(byUser);

            if (subscribtion == null)
            {
                subscribtion = IoC.Resolve<IDomainObjectFactory>().CreateCommentSubscribtion(forStory, byUser);
                IoC.Resolve<ICommentSubscribtionRepository>().Add(subscribtion);
            }
            return subscribtion;
        }

        [DebuggerStepThrough]
        public static ICommentSubscribtion RemoveCommentSubscribtion(this IStory forStory, IUser byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            var subscribtion = forStory.GetCommentSubscribtion(byUser);

            if (subscribtion != null)
            {
                IoC.Resolve<ICommentSubscribtionRepository>().Remove(subscribtion);
            }
            return subscribtion;
        }

    }
}
