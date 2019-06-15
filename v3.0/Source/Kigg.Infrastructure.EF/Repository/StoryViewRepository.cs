using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class StoryViewRepository: BaseRepository<StoryView>, IStoryViewRepository
    {
        private readonly DotnetomaniakContext _context;
        private static object _lockObj = new Object();

        public StoryViewRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(IStoryView entity)
        {
            base.Add((StoryView)entity);
        }

        public void Remove(IStoryView entity)
        {
            base.Remove((StoryView)entity);
        }

        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            var views = Cache.Get<Dictionary<Guid, int>>("storyViewCount");
            if (views == null)
            {
                lock (_lockObj)
                {
                    views = Cache.Get<Dictionary<Guid, int>>("storyViewCount");
                    if (views == null)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        views = _context.StoryViews.GroupBy(x => x.StoryId)
                            .Select(x => new { StoryId = x.Key, Count = x.Count() })
                            .ToDictionary(x => x.StoryId, x => x.Count);

                        Cache.Set("storyViewCount", views, DateTime.Now.AddMinutes(15));
                        stopwatch.Stop();
                        Log.Info("Created cache storyViewCount in " + stopwatch.ElapsedMilliseconds);
                    }
                }
            }

            return views.ContainsKey(storyId) ? views[storyId] : 0;
        }

        public virtual ICollection<IStoryView> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return _context.StoryViews.Where(v => v.StoryId == storyId && v.Timestamp >= timestamp)
                                               .Cast<IStoryView>()
                                               .ToList()
                                                .AsReadOnly();
        }
    }
}