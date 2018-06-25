using Kigg.Infrastructure;

namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public class StoryRepository : BaseRepository<IStory, Story>, IStoryRepository
    {
        public class CountCache
        {
            public int Count { get; set; }
        }
        private readonly IConfigurationSettings _settings;

        public StoryRepository(IDatabase database, IConfigurationSettings settings)
            : base(database)
        {
            _settings = settings;
        }

        public StoryRepository(IDatabaseFactory factory, IConfigurationSettings settings)
            : base(factory)
        {
            _settings = settings;
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

            Database.DeleteAll(Database.StoryViewDataSource.Where(sv => sv.StoryId == story.Id));
            Database.DeleteAll(Database.CommentSubscribtionDataSource.Where(cs => cs.StoryId == story.Id));
            Database.DeleteAll(Database.CommentDataSource.Where(c => c.StoryId == story.Id));
            Database.DeleteAll(Database.VoteDataSource.Where(v => v.StoryId == story.Id));
            Database.DeleteAll(Database.MarkAsSpamDataSource.Where(sp => sp.StoryId == story.Id));
            Database.DeleteAll(Database.StoryTagDataSource.Where(st => st.StoryId == story.Id));

            base.Remove(story);
        }

        public virtual IStory FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.StoryDataSource.SingleOrDefault(s => s.Id == id);
        }

        public virtual IStory FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return Database.StoryDataSource.SingleOrDefault(s => s.UniqueName == uniqueName);
        }

        public virtual IStory FindByUrl(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string hashedUrl = url.ToUpperInvariant().Hash();

            return Database.StoryDataSource.SingleOrDefault(s => s.UrlHash == hashedUrl);
        }

        public virtual PagedResult<IStory> FindPublished(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByPublished();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPublishedByCategory(Guid categoryId, int start, int max)
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByCategory(categoryId);

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null) && (s.CategoryId == categoryId))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPublishedByCategory(string category, int start, int max)
        {
            Check.Argument.IsNotEmpty(category, "category");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByCategory(category);

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null) && (s.Category.Name == category))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindUpcoming(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByUpcoming();
            var now = SystemTime.Now();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt == null) && (s.Rank == null) && (s.CreatedAt.AddHours(_settings.MaximumAgeOfStoryInHoursToPublish) > now))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindNew(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByNew();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.LastProcessedAt == null))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindUnapproved(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByUnapproved();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt == null))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByPublishable(minimumDate, maximumDate);

            var stories = Database.StoryDataSource
                                  .Where(s => (((s.ApprovedAt >= minimumDate) && (s.ApprovedAt <= maximumDate)) && ((s.LastProcessedAt == null) || (s.LastProcessedAt <= s.LastActivityAt))))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start).Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindByTag(Guid tagId, int start, int max)
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByTag(tagId);

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && s.StoryTags.Any(st => st.TagId == tagId))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindByTag(string tag, int start, int max)
        {
            Check.Argument.IsNotEmpty(tag, "tag");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByTag(tag);

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && s.StoryTags.Any(st => st.Tag.Name == tag))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> Search(string query, int start, int max)
        {
            Check.Argument.IsNotEmpty(query, "query");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            string ftQuery = "\"" + query + "*\"";

            int total = Database.StoryDataSource
                                .Count(s => (s.ApprovedAt != null) && (Database.StorySearch(ftQuery).Any(result => s.Id == result.Id) || s.Category.Name.Contains(query) || s.StoryTags.Any(st => st.Tag.Name.Contains(query)) || Database.CommentSearch(ftQuery).Any(result => result.StoryId == s.Id)));

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (Database.StorySearch(ftQuery).Any(result => s.Id == result.Id) || s.Category.Name.Contains(query) || s.StoryTags.Any(st => st.Tag.Name.Contains(query)) || Database.CommentSearch(ftQuery).Any(result => result.StoryId == s.Id)))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountPostedByUser(userId);

            List<Story> stories = Database.StoryDataSource
                                          .Where(s => ((s.ApprovedAt != null) && (s.UserId == userId)))
                                          .OrderByDescending(s => s.CreatedAt)
                                          .Skip(start)
                                          .Take(max)
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPostedByUser(string userName, int start, int max)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountPostedByUser(userName);

            List<Story> stories = Database.StoryDataSource
                                          .Where(s => ((s.ApprovedAt != null) && (s.User.UserName == userName)))
                                          .OrderByDescending(s => s.CreatedAt)
                                          .Skip(start)
                                          .Take(max)
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPromotedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.StoryDataSource
                                .Count(s => ((s.ApprovedAt != null) && s.StoryVotes.Any(v => v.UserId == userId)));

            List<Story> stories = Database.VoteDataSource
                                          .Where(v => ((v.UserId == userId) && (v.Story.ApprovedAt != null)))
                                          .OrderByDescending(v => v.Timestamp)
                                          .Select(v => v.Story)
                                          .Skip(start)
                                          .Take(max)
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPromotedByUser(string userName, int start, int max)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.StoryDataSource
                                .Count(s => ((s.ApprovedAt != null) && s.StoryVotes.Any(v => v.User.UserName == userName)));

            List<Story> stories = Database.VoteDataSource
                                          .Where(v => ((v.User.UserName == userName) && (v.Story.ApprovedAt != null)))
                                          .OrderByDescending(v => v.Timestamp)
                                          .Select(v => v.Story)
                                          .Skip(start)
                                          .Take(max)
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindCommentedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.StoryDataSource
                                .Count(s => ((s.ApprovedAt != null) && s.StoryComments.Any(c => c.UserId == userId)));

            IQueryable<Guid> ids = Database.CommentDataSource
                                           .Where(c => ((c.UserId == userId) && (c.Story.ApprovedAt != null)))
                                           .OrderByDescending(c => c.CreatedAt)
                                           .Select(c => c.StoryId)
                                           .Distinct()
                                           .Skip(start)
                                           .Take(max);

            List<Story> stories = Database.StoryDataSource
                                          .Where(s => ids.Contains(s.Id))
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public ICollection<IStory> FindSimilar(IStory storyToFindSimilarTo)
        {           
            var tags = storyToFindSimilarTo.Tags.Select(x => x.Name).ToList();
            tags = tags.Where(x => x != "C#").Where(x => x != ".Net").ToList();

            var similarsByTags = Database.StoryDataSource
                           .OrderByDescending(x => x.CreatedAt)
                           .SelectMany(x => x.StoryTags)
                           .Where(x => tags.Contains(x.Tag.Name))
                           .GroupBy(x => x.Story)
                           .Select(x => new { Element = x.Key, Count = x.Count() })
                           .OrderByDescending(x => x.Count)
                           .Select(x => x.Element)
                           .Where(x => x != storyToFindSimilarTo)
                           .Cast<IStory>()
                           .Take(11).ToList();

            if (similarsByTags.Count == 0)
            {
                var category = storyToFindSimilarTo.BelongsTo.Id;
                var similarsByCategory = Database.StoryDataSource
                    .OrderByDescending(x => x.CreatedAt)
                    .Where(x => x.CategoryId == category)
                    .Cast<IStory>()
                    .Take(11).ToList();

                return similarsByCategory;
            }
            else
            {
                return similarsByTags;
            }
        }

        public virtual int CountByPublished()
        {
            return Database.StoryDataSource
                           .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null));
        }

        private int GetOrAdd(string key, Func<int> value, int cacheTimeInMinutes = 5)
        {
            if (Cache.TryGet(key, out CountCache result))
            {
                return result.Count;
            }

            var count = value();
            Cache.Set(key, new CountCache {Count = count}, DateTime.Now.AddMinutes(cacheTimeInMinutes));
            return count;
        }

        public virtual int CountByUpcoming()
        {
            return GetOrAdd("StoryRepository.CountByUpcoming", () =>
            {
                var now = SystemTime.Now();
                return Database.StoryDataSource.Count(s =>
                    (s.ApprovedAt != null) && (s.PublishedAt == null) &&
                    (s.CreatedAt.AddHours(_settings.MaximumAgeOfStoryInHoursToPublish) > now));
            });
        }

        public virtual int CountByCategory(Guid categoryId)
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");

            return GetOrAdd("StoryRepository.CountByCategory." + categoryId.ToString("N"), () =>
            {
                return Database.StoryDataSource
                    .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.CategoryId == categoryId));
            });
        }

        public virtual int CountByCategory(string categoryName)
        {
            Check.Argument.IsNotEmpty(categoryName, "categoryName");

            return GetOrAdd("StoryRepository.CountByCategory." + categoryName, () =>
            {
                return Database.StoryDataSource
                    .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Category.Name == categoryName));
            });
        }

        public virtual int CountByTag(Guid tagId)
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");
            return GetOrAdd("StoryRepository.CountByTag." + tagId.ToString("N"), () =>
            {
                return Database.StoryDataSource.Count(s =>
                    (s.ApprovedAt != null) && s.StoryTags.Any(st => st.TagId == tagId));
            });
        }

        public virtual int CountByTag(string tagName)
        {
            Check.Argument.IsNotEmpty(tagName, "tagName");
            return GetOrAdd("StoryRepository.CountByTag." + tagName, () =>
            {
                return Database.StoryDataSource.Count(s =>
                    (s.ApprovedAt != null) && s.StoryTags.Any(st => st.Tag.Name == tagName));
            });
        }

        public virtual int CountByNew()
        {
            return GetOrAdd("StoryRepository.CountByNew", () =>
            {
                return Database.StoryDataSource.Count(s => ((s.ApprovedAt != null) && (s.LastProcessedAt == null)));
            });
        }

        public virtual int CountByUnapproved()
        {
            return GetOrAdd("StoryRepository.CountByUnapproved",
                () => { return Database.StoryDataSource.Count(s => s.ApprovedAt == null); });
        }

        public virtual int CountByPublishable(DateTime minimumDate, DateTime maximumDate)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            return GetOrAdd(
                "StoryRepository.CountByPublishable." + minimumDate.ToString("s") + "-" + maximumDate.ToString("s"),
                () =>
                {
                    return Database.StoryDataSource.Count(s =>
                        (((s.ApprovedAt >= minimumDate) && (s.ApprovedAt <= maximumDate)) &&
                         ((s.LastProcessedAt == null) || (s.LastProcessedAt <= s.LastActivityAt))));
                });
        }

        public virtual int CountPostedByUser(Guid userId)
        {
            Check.Argument.IsNotEmpty(userId, "userId");

            return Database.StoryDataSource.Count(s => (s.ApprovedAt != null) && (s.UserId == userId));
        }

        public virtual int CountPostedByUser(string userName)
        {
            Check.Argument.IsNotEmpty(userName, "userName");

            return Database.StoryDataSource.Count(s => (s.ApprovedAt != null) && (s.User.UserName == userName));
        }

    }
}