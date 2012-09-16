namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public class CommentSubscribtionRepository : ICommentSubscribtionRepository
    {
        private readonly IDatabase _database;

        public CommentSubscribtionRepository(IDatabase database)
        {
            Check.Argument.IsNotNull(database, "database");

            _database = database;
        }

        public CommentSubscribtionRepository(IDatabaseFactory factory)
            : this(factory.Create())
        {
        }

#if(DEBUG)
        public virtual int CountByStory(Guid storyId)
#else
        public int CountByStory(Guid storyId)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            return _database.UserDataSource.Count(u => u.CommentSubscriptionsInternal.Any()); //.Where(s=>s.Id==storyId)..Count(cs => cs.StoryId == storyId);
        }
        
#if(DEBUG)
        public virtual ICommentSubscribtion FindById(Guid storyId, Guid userId)
#else
        public ICommentSubscribtion FindById(Guid storyId, Guid userId)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            var story = _database.StoryDataSource.FirstOrDefault(s => s.Id == storyId);
            var user = _database.UserDataSource.FirstOrDefault(u => u.Id == userId);

            if (user != null && story != null && story.ContainsCommentSubscriber(user))
            {
                return new CommentSubscribtion(story, user);
            }
            return null;
        }

        public void Add(ICommentSubscribtion entity)
        {
            entity.ForStory.SubscribeComment(entity.ByUser);
        }

        public void Remove(ICommentSubscribtion entity)
        {
            entity.ForStory.UnsubscribeComment(entity.ByUser);
        }
    }
}