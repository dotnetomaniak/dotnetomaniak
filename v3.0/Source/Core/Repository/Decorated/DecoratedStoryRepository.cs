namespace Kigg.Repository
{
    using System;
    using System.Diagnostics;

    using DomainObjects;

    public abstract class DecoratedStoryRepository : IStoryRepository
    {
        private readonly IStoryRepository _innerRepository;

        protected DecoratedStoryRepository(IStoryRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        [DebuggerStepThrough]
        public virtual void Add(IStory entity)
        {
            _innerRepository.Add(entity);
        }

        [DebuggerStepThrough]
        public virtual void Remove(IStory entity)
        {
            _innerRepository.Remove(entity);
        }

        [DebuggerStepThrough]
        public virtual IStory FindById(Guid id)
        {
            return _innerRepository.FindById(id);
        }

        [DebuggerStepThrough]
        public virtual IStory FindByUniqueName(string uniqueName)
        {
            return _innerRepository.FindByUniqueName(uniqueName);
        }

        [DebuggerStepThrough]
        public virtual IStory FindByUrl(string url)
        {
            return _innerRepository.FindByUrl(url);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindPublished(int start, int max)
        {
            return _innerRepository.FindPublished(start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindPublishedByCategory(Guid categoryId, int start, int max)
        {
            return _innerRepository.FindPublishedByCategory(categoryId, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindPublishedByCategory(string category, int start, int max)
        {
            return _innerRepository.FindPublishedByCategory(category, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindUpcoming(int start, int max)
        {
            return _innerRepository.FindUpcoming(start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindNew(int start, int max)
        {
            return _innerRepository.FindNew(start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindUnapproved(int start, int max)
        {
            return _innerRepository.FindUnapproved(start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
        {
            return _innerRepository.FindPublishable(minimumDate, maximumDate, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindByTag(Guid tagId, int start, int max)
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            return _innerRepository.FindByTag(tagId, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindByTag(string tag, int start, int max)
        {
            return _innerRepository.FindByTag(tag, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> Search(string query, int start, int max)
        {
            return _innerRepository.Search(query, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max)
        {
            return _innerRepository.FindPostedByUser(userId, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindPostedByUser(string userName, int start, int max)
        {
            return _innerRepository.FindPostedByUser(userName, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindPromotedByUser(Guid userId, int start, int max)
        {
            return _innerRepository.FindPromotedByUser(userId, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindPromotedByUser(string userName, int start, int max)
        {
            return _innerRepository.FindPromotedByUser(userName, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IStory> FindCommentedByUser(Guid userId, int start, int max)
        {
            return _innerRepository.FindCommentedByUser(userId, start, max);
        }

        [DebuggerStepThrough]
        public virtual int CountByPublished()
        {
            return _innerRepository.CountByPublished();
        }

        [DebuggerStepThrough]
        public virtual int CountByUpcoming()
        {
            return _innerRepository.CountByUpcoming();
        }

        [DebuggerStepThrough]
        public virtual int CountByCategory(Guid categoryId)
        {
            return _innerRepository.CountByCategory(categoryId);
        }

        [DebuggerStepThrough]
        public virtual int CountByTag(Guid tagId)
        {
            return _innerRepository.CountByTag(tagId);
        }

        [DebuggerStepThrough]
        public virtual int CountByNew()
        {
            return _innerRepository.CountByNew();
        }

        [DebuggerStepThrough]
        public virtual int CountByUnapproved()
        {
            return _innerRepository.CountByUnapproved();
        }

        [DebuggerStepThrough]
        public virtual int CountByPublishable(DateTime minimumDate, DateTime maximumDate)
        {
            return _innerRepository.CountByPublishable(minimumDate, maximumDate);
        }

        [DebuggerStepThrough]
        public virtual int CountPostedByUser(Guid userId)
        {
            return _innerRepository.CountPostedByUser(userId);
        }

        [DebuggerStepThrough]
        public System.Collections.Generic.ICollection<IStory> FindSimilar(IStory story)
        {
            return _innerRepository.FindSimilar(story);
        }
    }
}