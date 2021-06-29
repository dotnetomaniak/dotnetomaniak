using System;
using System.Collections.Generic;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;
using Microsoft.EntityFrameworkCore;


namespace Kigg.Infrastructure.EF.Repository
{
    public class StoryRepository: BaseRepository<Story>, IStoryRepository
    {
        public class CountCache
        {
            public int Count { get; set; }
        }

        private readonly DotnetomaniakContext _context;
        private IQueryable<Story> storiesWithIncludes =>
            _context.Stories
                .Include(x => x.CommentSubscribtions)
                .Include(x => x.StoryComments)
                .Include(x => x.StoryMarkAsSpams)
                .Include(x => x.StoryTags)
                    .ThenInclude(st=>st.Tag)
                .Include(x => x.StoryViews)
                .Include(x => x.StoryVotes);
        private readonly IConfigurationSettings _settings;

        public StoryRepository(DotnetomaniakContext context, IConfigurationSettings settings) : base(context)
        {
            _context = context;
            _settings = settings;
        }

        public void Add(IStory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var story = (Story)entity;

            if (_context.Stories.Any(s => s.UrlHash == story.UrlHash))
            {
                throw new ArgumentException("\"{0}\" story with the same url already exits. Specifiy a diffrent url.".FormatWith(story.Url), "entity");
            }

            story.UniqueName = UniqueNameGenerator.GenerateFrom(_context.Stories, story.Title);

            base.Add(story);
        }

        public void Remove(IStory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var story = (Story)entity;

            _context.RemoveRange(_context.StoryViews.Where(sv => sv.StoryId == story.Id));
            _context.RemoveRange(_context.CommentSubscribtions.Where(cs => cs.StoryId == story.Id));
            _context.RemoveRange(_context.StoryComments.Where(c => c.StoryId == story.Id));
            _context.RemoveRange(_context.StoryVotes.Where(v => v.StoryId == story.Id));
            _context.RemoveRange(_context.StoryMarkAsSpams.Where(sp => sp.StoryId == story.Id));
            _context.RemoveRange(_context.StoryTags.Where(st => st.StoryId == story.Id));

            base.Remove(story);
        }

        public virtual IStory FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return storiesWithIncludes.SingleOrDefault(s => s.Id == id);
        }

        public virtual IStory FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return storiesWithIncludes.SingleOrDefault(s => s.UniqueName == uniqueName);
        }

        public virtual IStory FindByUrl(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string hashedUrl = url.ToUpperInvariant().Hash();

            return storiesWithIncludes.SingleOrDefault(s => s.UrlHash == hashedUrl);
        }

        public virtual PagedResult<IStory> FindPublished(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByPublished();

            var stories = storiesWithIncludes
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

            var stories = _context.Stories
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

            var stories = _context.Stories
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

            var stories = storiesWithIncludes
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

            var stories = storiesWithIncludes
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

            var stories = storiesWithIncludes
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

            var stories = storiesWithIncludes
                                  .Where(s => (((s.ApprovedAt >= minimumDate) && (s.ApprovedAt <= maximumDate)) && s.PublishedAt == null))
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

            var stories = _context.Stories
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

            var stories = storiesWithIncludes
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

            throw new NotImplementedException();

            //StorySearch and CommentSearch SP are not mapped to EF
            //After fixing it everything should works


            //string ftQuery = "\"" + query + "*\"";

            //int total = _context.Stories
            //                    .Count(s => (s.ApprovedAt != null) && (_context.StorySearch(ftQuery).Any(result => s.Id == result.Id) || s.Category.Name.Contains(query) || s.StoryTags.Any(st => st.Tag.Name.Contains(query)) || _context.CommentSearch(ftQuery).Any(result => result.StoryId == s.Id)));

            //var stories = _context.Stories
            //                      .Where(s => (s.ApprovedAt != null) && (_context.StorySearch(ftQuery).Any(result => s.Id == result.Id) || s.Category.Name.Contains(query) || s.StoryTags.Any(st => st.Tag.Name.Contains(query)) || _context.CommentSearch(ftQuery).Any(result => result.StoryId == s.Id)))
            //                      .OrderByDescending(s => s.PublishedAt)
            //                      .ThenBy(s => s.Rank)
            //                      .ThenByDescending(s => s.CreatedAt)
            //                      .Skip(start)
            //                      .Take(max);

            //return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountPostedByUser(userId);

            List<Story> stories = storiesWithIncludes
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

            List<Story> stories = storiesWithIncludes
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

            int total = _context.Stories
                                .Count(s => ((s.ApprovedAt != null) && s.StoryVotes.Any(v => v.UserId == userId)));

            List<Story> stories = _context.StoryVotes
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

            int total = _context.Stories
                                .Count(s => ((s.ApprovedAt != null) && s.StoryVotes.Any(v => v.User.UserName == userName)));

            List<Story> stories = _context.StoryVotes
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

            int total = _context.Stories
                                .Count(s => ((s.ApprovedAt != null) && s.StoryComments.Any(c => c.UserId == userId)));

            var ids = _context.StoryComments
                                           .Where(c => ((c.UserId == userId) && (c.Story.ApprovedAt != null)))
                                           .Select(c => new { c.StoryId, c.CreatedAt })
                                           .Distinct()
                                           .OrderByDescending(c => c.CreatedAt)
                                           .Skip(start)
                                           .Take(max)
                                           .Select(c => c.StoryId)
                                           .ToList();

            List<Story> stories = storiesWithIncludes
                                          .Where(s => ids.Contains(s.Id))
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public ICollection<IStory> FindSimilar(IStory storyToFindSimilarTo)
        {
            var tags = storyToFindSimilarTo
                .Tags
                .Where(x => x.Name != "C#")
                .Where(x => x.Name != ".Net")
                .Select(x => x.Id)
                .ToList();

            var simmilarIds = _context.StoryTags
                .Where(x => x.StoryId != storyToFindSimilarTo.Id)
                .Where(st => tags.Contains(st.TagId))
                .GroupBy(x => x.StoryId)
                .Select(x => new { StoryId = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .Take(11)
                .Select(x=>x.StoryId)
                .ToList();


            var similarsByTags = storiesWithIncludes
                .Where(s => simmilarIds.Contains(s.Id))
                .Cast<IStory>()
                .ToList();

            if (similarsByTags.Count == 0)
            {
                var category = storyToFindSimilarTo.BelongsTo.Id;
                var similarsByCategory = storiesWithIncludes
                    .OrderByDescending(x => x.CreatedAt)
                    .Where(x => x.CategoryId == category)
                    .Take(11)
                    .Cast<IStory>()
                    .ToList();

                return similarsByCategory;
            }
            else
            {
                return similarsByTags;
            }
        }

        public ICollection<IStory> FindCreatedBetween(DateTime begin, DateTime end)
        {
            return storiesWithIncludes
                .Where(s => s.CreatedAt >= begin && s.CreatedAt <= end)
                .Cast<IStory>().ToList();
        }

        public ICollection<IStory> FindPublishedBetween(DateTime begin, DateTime end)
        {
            return storiesWithIncludes
                .Where(s => s.PublishedAt != null && s.PublishedAt >= begin && s.PublishedAt <= end)
                .Cast<IStory>().ToList();
        }

        public virtual int CountByPublished()
        {
            return _context.Stories
                           .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null));
        }

        private int GetOrAdd(string key, Func<int> value, int cacheTimeInMinutes = 5)
        {
            if (Cache.TryGet(key, out CountCache result))
            {
                return result.Count;
            }

            var count = value();
            Cache.Set(key, new CountCache { Count = count }, DateTime.Now.AddMinutes(cacheTimeInMinutes));
            return count;
        }

        public virtual int CountByUpcoming()
        {
            return GetOrAdd("StoryRepository.CountByUpcoming", () =>
            {
                var now = SystemTime.Now();
                return _context.Stories.Count(s =>
                    (s.ApprovedAt != null) && (s.PublishedAt == null) &&
                    (s.CreatedAt.AddHours(_settings.MaximumAgeOfStoryInHoursToPublish) > now));
            });
        }

        public virtual int CountByCategory(Guid categoryId)
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");

            return GetOrAdd("StoryRepository.CountByCategory." + categoryId.ToString("N"), () =>
            {
                return _context.Stories
                    .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.CategoryId == categoryId));
            });
        }

        public virtual int CountByCategory(string categoryName)
        {
            Check.Argument.IsNotEmpty(categoryName, "categoryName");

            return GetOrAdd("StoryRepository.CountByCategory." + categoryName, () =>
            {
                return _context.Stories
                    .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Category.Name == categoryName));
            });
        }

        public virtual int CountByTag(Guid tagId)
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");
            return GetOrAdd("StoryRepository.CountByTag." + tagId.ToString("N"), () =>
            {
                return _context.Stories.Count(s =>
                    (s.ApprovedAt != null) && s.StoryTags.Any(st => st.TagId == tagId));
            });
        }

        public virtual int CountByTag(string tagName)
        {
            Check.Argument.IsNotEmpty(tagName, "tagName");
            return GetOrAdd("StoryRepository.CountByTag." + tagName, () =>
            {
                return _context.Stories.Count(s =>
                    (s.ApprovedAt != null) && s.StoryTags.Any(st => st.Tag.Name == tagName));
            });
        }

        public virtual int CountByNew()
        {
            return GetOrAdd("StoryRepository.CountByNew", () =>
            {
                return _context.Stories.Count(s => ((s.ApprovedAt != null) && (s.LastProcessedAt == null)));
            });
        }

        public virtual int CountByUnapproved()
        {
            return GetOrAdd("StoryRepository.CountByUnapproved",
                () => { return _context.Stories.Count(s => s.ApprovedAt == null); });
        }

        public virtual int CountByPublishable(DateTime minimumDate, DateTime maximumDate)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            return GetOrAdd(
                "StoryRepository.CountByPublishable." + minimumDate.ToString("s") + "-" + maximumDate.ToString("s"),
                () =>
                {
                    return _context.Stories.Count(s => (((s.ApprovedAt >= minimumDate) && (s.ApprovedAt <= maximumDate)) && s.PublishedAt == null));
                });
        }

        public virtual int CountPostedByUser(Guid userId)
        {
            Check.Argument.IsNotEmpty(userId, "userId");

            return _context.Stories.Count(s => (s.ApprovedAt != null) && (s.UserId == userId));
        }

        public virtual int CountPostedByUser(string userName)
        {
            Check.Argument.IsNotEmpty(userName, "userName");

            return _context.Stories.Count(s => (s.ApprovedAt != null) && (s.User.UserName == userName));
        }
    }
}