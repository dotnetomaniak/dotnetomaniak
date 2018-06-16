using System.Diagnostics;
using Kigg.Infrastructure;

namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;
    
    public class StoryViewRepository : BaseRepository<IStoryView, StoryView>, IStoryViewRepository
    {
        private static object _lockObj = new Object();
        private readonly ICache _cache;

        public StoryViewRepository(IDatabase database, ICache cache) : base(database)
        {
            _cache = cache;
        }

        public StoryViewRepository(IDatabaseFactory factory, ICache cache) : base(factory)
        {
            _cache = cache;
        }

        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            var views = Cache.Get<Dictionary<Guid, int>>("storyViewCount");
            if (views == null)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                views = Database.StoryViewDataSource.GroupBy(x => x.StoryId)
                    .Select(x => new {StoryId = x.Key, Count = x.Count()})
                    .ToDictionary(x => x.StoryId, x => x.Count);
                
                Cache.Set("storyViewCount", views, DateTime.Now.AddMinutes(15));
                stopwatch.Stop();
                Log.Info("Created cache storyViewCount in " + stopwatch.ElapsedMilliseconds);
            }

            return views.ContainsKey(storyId) ? views[storyId] : 0;
            //return Database.StoryViewDataSource.Count(v => v.StoryId == storyId);
        }

        public virtual ICollection<IStoryView> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.StoryViewDataSource.Where(v => v.StoryId == storyId && v.Timestamp >= timestamp)
                                               .Cast<IStoryView>()
                                               .ToList()
                                                .AsReadOnly();
        }
    }
}