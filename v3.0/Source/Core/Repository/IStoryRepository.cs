namespace Kigg.Repository
{
    using System;

    using DomainObjects;

    public interface IStoryRepository : IUniqueNameEntityRepository<IStory>
    {
        IStory FindByUrl(string url);

        PagedResult<IStory> FindPublished(int start, int max);

        PagedResult<IStory> FindPublishedByCategory(Guid categoryId, int start, int max);

        PagedResult<IStory> FindPublishedByCategory(string category, int start, int max);

        PagedResult<IStory> FindUpcoming(int start, int max);

        PagedResult<IStory> FindNew(int start, int max);

        PagedResult<IStory> FindUnapproved(int start, int max);

        PagedResult<IStory> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max);

        PagedResult<IStory> FindByTag(Guid tagId, int start, int max);

        PagedResult<IStory> FindByTag(string tag, int start, int max);

        PagedResult<IStory> Search(string query, int start, int max);

        PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max);

        PagedResult<IStory> FindPostedByUser(string userName, int start, int max);

        PagedResult<IStory> FindPromotedByUser(Guid userId, int start, int max);

        PagedResult<IStory> FindPromotedByUser(string userName, int start, int max);

        PagedResult<IStory> FindCommentedByUser(Guid userId, int start, int max);

        int CountByPublished();

        int CountByUpcoming();

        int CountByCategory(Guid categoryId);

        int CountByTag(Guid tagId);

        int CountByNew();

        int CountByUnapproved();

        int CountByPublishable(DateTime minimumDate, DateTime maximumDate);

        int CountPostedByUser(Guid userId);
    }
}