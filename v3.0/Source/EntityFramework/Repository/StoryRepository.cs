namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public partial class StoryRepository : BaseRepository<IStory, Story>, IStoryRepository
    {

        public StoryRepository(IDatabase database)
            : base(database)
        {
        }

        public StoryRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public override void Add(IStory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var story = (Story)entity;

            if (Database.StoryDataSource.Any(s => s.UrlHash == story.UrlHash))
            {
                throw new ArgumentException("\"{0}\" story with the same url already exits. Specifiy a diffrent url.".FormatWith(story.Url), "entity");
            }

            story.UniqueName = UniqueNameGenerator.GenerateFrom(Database.StoryDataSource, story.Title);

            base.Add(story);
        }

        public override void Remove(IStory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var story = (Story)entity;

            story.RemoveAllTags();
            story.RemoveAllCommentSubscribers();
            Database.DeleteAllOnSubmit(Database.StoryViewDataSource.Where(sv => sv.Story.Id == story.Id));
            Database.DeleteAllOnSubmit(Database.CommentDataSource.Where(c => c.Story.Id == story.Id));
            Database.DeleteAllOnSubmit(Database.VoteDataSource.Where(v => v.StoryId == story.Id));
            Database.DeleteAllOnSubmit(Database.MarkAsSpamDataSource.Where(sp => sp.StoryId == story.Id));

            base.Remove(story);
        }

#if(DEBUG)
        public virtual IStory FindById(Guid id)
#else
        public IStory FindById(Guid id)
#endif
        {
            Check.Argument.IsNotEmpty(id, "id");

            return DataContext != null ? FindByIdQuery.Invoke(DataContext, id) : Database.StoryDataSource.FirstOrDefault(s => s.Id == id);
        }

#if(DEBUG)
        public virtual IStory FindByUniqueName(string uniqueName)
#else
        public IStory FindByUniqueName(string uniqueName)
#endif
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return DataContext != null ? FindByUniqueNameQuery.Invoke(DataContext, uniqueName) : Database.StoryDataSource.FirstOrDefault(s => s.UniqueName == uniqueName);
        }

#if(DEBUG)
        public virtual IStory FindByUrl(string url)
#else
        public IStory FindByUrl(string url)
#endif
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string hashedUrl = url.ToUpperInvariant().Hash();

            return DataContext != null ? FindByUrlQuery.Invoke(DataContext, hashedUrl) : Database.StoryDataSource.FirstOrDefault(s => s.UrlHash == hashedUrl);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindPublished(int start, int max)
#else
        public PagedResult<IStory> FindPublished(int start, int max)
#endif
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByPublished();

            var stories = DataContext != null
                          ? FindPublishedQuery.Invoke(DataContext, start, max)
                          : Database.StoryDataSource
                                    .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null))
                                    .OrderByDescending(s => s.PublishedAt)
                                    .ThenBy(s => s.Rank)
                                    .ThenByDescending(s => s.CreatedAt)
                                    .Skip(start)
                                    .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindPublishedByCategory(Guid categoryId, int start, int max)
#else
        public PagedResult<IStory> FindPublishedByCategory(Guid categoryId, int start, int max)
#endif
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByCategory(categoryId);

            var stories = (DataContext != null)
                ? FindPublishedByCategoryIdQuery.Invoke(DataContext, categoryId, start, max)
                : Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null) && (s.Category.Id == categoryId))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindPublishedByCategory(string category, int start, int max)
#else
        public PagedResult<IStory> FindPublishedByCategory(string category, int start, int max)
#endif

        {
            Check.Argument.IsNotEmpty(category, "categoryId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByCategory(category);

            var stories = (DataContext != null)
                ? FindPublishedByCategoryNameQuery.Invoke(DataContext, category, start, max)
                : Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null) && (s.Category.Name == category))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindUpcoming(int start, int max)
#else
        public PagedResult<IStory> FindUpcoming(int start, int max)
#endif
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByUpcoming();

            var stories = (DataContext != null)
                            ? FindUpcomingQuery.Invoke(DataContext, start, max)
                            : Database.StoryDataSource
                                      .Where(s => (s.ApprovedAt != null) && (s.PublishedAt == null) && (s.Rank == null))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start)
                                      .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindNew(int start, int max)
#else
        public PagedResult<IStory> FindNew(int start, int max)
#endif
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByNew();

            var stories = (DataContext != null)
                            ? FindNewQuery.Invoke(DataContext, start, max)
                            : Database.StoryDataSource
                                      .Where(s => (s.ApprovedAt != null) && (s.LastProcessedAt == null))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start)
                                      .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindUnapproved(int start, int max)
#else
        public PagedResult<IStory> FindUnapproved(int start, int max)
#endif
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByUnapproved();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt == null))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
#else
        public PagedResult<IStory> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
#endif
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");


            var param = new FindPublishableQueryParameter
            {
                minimumDate = minimumDate,
                maximumDate = maximumDate,
                start = start,
                max = max
            };

            int total = CountByPublishable(minimumDate, maximumDate);

            var stories = (DataContext != null)
                            ? FindPublishableQuery.Invoke(DataContext, param)
                            : Database.StoryDataSource
                                      .Where(s => (((s.ApprovedAt >= minimumDate) && (s.ApprovedAt <= maximumDate)) && ((s.LastProcessedAt == null) || (s.LastProcessedAt <= s.LastActivityAt))))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start).Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindByTag(Guid tagId, int start, int max)
#else
        public PagedResult<IStory> FindByTag(Guid tagId, int start, int max)
#endif
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByTag(tagId);

            var stories = (DataContext != null)
                            ? FindByTagIdQuery.Invoke(DataContext, tagId, start, max)
                            : Database.StoryDataSource
                                      .Where(s => (s.ApprovedAt != null) && s.StoryTagsInternal.Any(t => t.Id == tagId))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start)
                                      .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindByTag(string tagName, int start, int max)
#else
        public PagedResult<IStory> FindByTag(string tagName, int start, int max)
#endif
        {
            Check.Argument.IsNotEmpty(tagName, "tagName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByTag(tagName);

            var stories = (DataContext != null)
                            ? FindByTagNameQuery.Invoke(DataContext, tagName, start, max)
                            : Database.StoryDataSource
                                      .Where(s => (s.ApprovedAt != null) && s.StoryTagsInternal.Any(t => t.Name == tagName))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start)
                                      .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> Search(string query, int start, int max)
#else
        public PagedResult<IStory> Search(string query, int start, int max)
#endif
        {

            Check.Argument.IsNotEmpty(query, "query");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            string ftQuery = "\"" + query + "*\"";

            Database.SetSearchQuery(ftQuery);

            int total = Database.StoryDataSource
                .Count(s => (s.ApprovedAt != null) && (Database.StorySearchResult.Any(r => r == s.Id) || s.Category.Name.Contains(query) || s.StoryTagsInternal.Any(t => t.Name.Contains(query))));

            var stories = Database.StoryDataSource
                                      .Where(s => (s.ApprovedAt != null) && (Database.StorySearchResult.Any(r => r == s.Id) || s.Category.Name.Contains(query) || s.StoryTagsInternal.Any(t => t.Name.Contains(query))))
                                      .OrderByDescending(s => s.PublishedAt)
                                      .ThenBy(s => s.Rank)
                                      .ThenByDescending(s => s.CreatedAt)
                                      .Skip(start)
                                      .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max)
#else
        public PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max)
#endif
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountPostedByUser(userId);

            var stories = (DataContext != null)
                            ? FindPostedByUserIdQuery.Invoke(DataContext, userId, start, max)
                            : Database.StoryDataSource
                                      .Where(s => ((s.ApprovedAt != null) && (s.User.Id == userId)))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start)
                                      .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindPostedByUser(string userName, int start, int max)
#else
        public PagedResult<IStory> FindPostedByUser(string userName, int start, int max)
#endif
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountPostedByUser(userName);

            var stories = (DataContext != null)
                            ? FindPostedByUserNameQuery.Invoke(DataContext, userName, start, max)
                            : Database.StoryDataSource
                                      .Where(s => ((s.ApprovedAt != null) && (s.User.UserName == userName)))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start)
                                      .Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindPromotedByUser(Guid userId, int start, int max)
#else
        public PagedResult<IStory> FindPromotedByUser(Guid userId, int start, int max)
#endif
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.StoryDataSource
                                .Count(s => ((s.ApprovedAt != null) && s.StoryVotesInternal.Any(v => v.User.Id == userId)));

            var stories = (DataContext != null)
                            ? FindPromotedByUserIdQuery.Invoke(DataContext, userId, start, max)
                            : Database.VoteDataSource
                                      .Where(v => ((v.User.Id == userId) && (v.Story.ApprovedAt != null)))
                                      .OrderByDescending(v => v.Timestamp)
                                      .Select(v => v.Story)
                                      .Skip(start).Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindPromotedByUser(string userName, int start, int max)
#else
        public PagedResult<IStory> FindPromotedByUser(string userName, int start, int max)
#endif
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.StoryDataSource
                                .Count(s => ((s.ApprovedAt != null) && s.StoryVotesInternal.Any(v => v.User.UserName == userName)));

            var stories = (DataContext != null)
                            ? FindPromotedByUserNameQuery.Invoke(DataContext, userName, start, max)
                            : Database.VoteDataSource
                                      .Where(v => ((v.User.UserName == userName) && (v.Story.ApprovedAt != null)))
                                      .OrderByDescending(v => v.Timestamp)
                                      .Select(v => v.Story)
                                      .Skip(start).Take(max);

            return BuildPagedResult<IStory>(stories.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual PagedResult<IStory> FindCommentedByUser(Guid userId, int start, int max)
#else
        public PagedResult<IStory> FindCommentedByUser(Guid userId, int start, int max)
#endif
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");


            var ids = Database.StoryDataSource
                              .Where(s => ((s.ApprovedAt != null) && s.StoryCommentsInternal.Any(c => c.User.Id == userId)))
                              .Select(s => s.Id);
            int total = ids.Count();

            var stories = (DataContext != null)
                            ? FindCommentedByUserIdQuery.Invoke(DataContext, userId, start, max)
                            : Database.CommentDataSource
                                      .Where(c => ids.Any(id => id == c.Story.Id))
                                      .OrderByDescending(c => c.CreatedAt)
                                      .Select(c => c.Story)
                                      .Skip(start).Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

#if(DEBUG)
        public virtual int CountByPublished()
#else
        public int CountByPublished()
#endif
        {
            return (DataContext != null)
                       ? CountByPublishedQuery.Invoke(DataContext)
                       : Database.StoryDataSource
                                 .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null));
        }

#if(DEBUG)
        public virtual int CountByUpcoming()
#else
        public int CountByUpcoming()
#endif
        {
            return (DataContext != null)
                       ? CountByUpcomingQuery.Invoke(DataContext)
                       : Database.StoryDataSource
                                 .Count(s => (s.ApprovedAt != null) && (s.PublishedAt == null));
        }

#if(DEBUG)
        public virtual int CountByCategory(Guid categoryId)
#else
        public int CountByCategory(Guid categoryId)
#endif
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");

            return (DataContext != null)
                ? CountPublishedByCategoryIdQuery.Invoke(DataContext, categoryId)
                : Database.StoryDataSource
                          .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Category.Id == categoryId));
        }

#if(DEBUG)
        public virtual int CountByCategory(string categoryName)
#else
        public int CountByCategory(string categoryName)
#endif
        {
            Check.Argument.IsNotEmpty(categoryName, "categoryName");

            return (DataContext != null)
                ? CountPublishedByCategoryNameQuery.Invoke(DataContext, categoryName)
                : Database.StoryDataSource
                          .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Category.Name == categoryName));
        }

#if(DEBUG)
        public virtual int CountByTag(Guid tagId)
#else
        public int CountByTag(Guid tagId)
#endif
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");

            return (DataContext != null)
                ? CountByTagIdQuery.Invoke(DataContext, tagId)
                : Database.StoryDataSource.Count(s => (s.ApprovedAt != null) && s.StoryTagsInternal.Any(t => t.Id == tagId));
        }

#if(DEBUG)
        public virtual int CountByTag(string tagName)
#else
        public int CountByTag(string tagName)
#endif
        {
            Check.Argument.IsNotEmpty(tagName, "tagName");

            return (DataContext != null)
                ? CountByTagNameQuery.Invoke(DataContext, tagName)
                : Database.StoryDataSource.Count(s => (s.ApprovedAt != null) && s.StoryTagsInternal.Any(t => t.Name == tagName));
        }

#if(DEBUG)
        public virtual int CountByNew()
#else
        public int CountByNew()
#endif
        {
            return (DataContext != null)
                    ? CountByNewQuery.Invoke(DataContext)
                    : Database.StoryDataSource
                              .Count(s => ((s.ApprovedAt != null) && (s.LastProcessedAt == null)));
        }

#if(DEBUG)
        public virtual int CountByUnapproved()
#else
        public int CountByUnapproved()
#endif
        {
            return Database.StoryDataSource.Count(s => s.ApprovedAt == null);
        }

#if(DEBUG)
        public virtual int CountByPublishable(DateTime minimumDate, DateTime maximumDate)
#else
        public int CountByPublishable(DateTime minimumDate, DateTime maximumDate)
#endif
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");

            return (DataContext != null)
                ? CountByPublishableQuery.Invoke(DataContext, minimumDate, maximumDate)
                : Database.StoryDataSource.Count(s => (((s.ApprovedAt >= minimumDate) && (s.ApprovedAt <= maximumDate)) && ((s.LastProcessedAt == null) || (s.LastProcessedAt <= s.LastActivityAt))));
        }

#if(DEBUG)
        public virtual int CountPostedByUser(Guid userId)
#else
        public int CountPostedByUser(Guid userId)
#endif
        {
            Check.Argument.IsNotEmpty(userId, "userId");

            return (DataContext != null)
                ? CountPostedByUserIdQuery.Invoke(DataContext, userId)
                : Database.StoryDataSource
                          .Count(s => (s.ApprovedAt != null) && (s.User.Id == userId));
        }

#if(DEBUG)
        public virtual int CountPostedByUser(string userName)
#else
        public int CountPostedByUser(string userName)
#endif
        {
            Check.Argument.IsNotEmpty(userName, "userName");

            return (DataContext != null)
                ? CountPostedByUserNameQuery.Invoke(DataContext, userName)
                : Database.StoryDataSource
                          .Count(s => (s.ApprovedAt != null) && (s.User.UserName == userName));
        }

    }
}