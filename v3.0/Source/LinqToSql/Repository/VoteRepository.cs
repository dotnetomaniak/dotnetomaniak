using System.Runtime.Caching;
using Kigg.Infrastructure;

namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;
    
    public class VoteRepository : BaseRepository<IVote, StoryVote>, IVoteRepository
    {
        //private static readonly MemoryCache Cache = new MemoryCache("VoteRepository");
        private ICache Cache;
        public VoteRepository(IDatabase database, ICache cache) : base(database)
        {
            Cache = cache;
        }

        public VoteRepository(IDatabaseFactory factory, ICache cache) : base(factory)
        {
            Cache = cache;
        }

        public void InvalidateCacheForStory(Guid storyId)
        {
            var cachedVotes = Cache.Get<Dictionary<Guid, int>>("storyVoteCount");
            if (cachedVotes != null)
            {
                var count = Database.VoteDataSource.Count(v => v.StoryId == storyId);
                cachedVotes[storyId] = count;
            }
        }
        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            var cachedVotes = Cache.Get<Dictionary<Guid, int>>("storyVoteCount");
            if (cachedVotes == null)
            {
                cachedVotes = Database.VoteDataSource.GroupBy(x => x.StoryId)
                    .Select(x => new {StoryId = x.Key, Count = x.Count()})
                    .ToDictionary(x => x.StoryId, x => x.Count);
                Cache.Set("storyVoteCount", cachedVotes, DateTime.Now.AddMinutes(3));
            }

            return cachedVotes.ContainsKey(storyId) ? cachedVotes[storyId] : 0;

            //return Database.VoteDataSource.Count(v => v.StoryId == storyId);
        }

        public virtual IVote FindById(Guid storyId, Guid userId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return Database.VoteDataSource.SingleOrDefault(v => v.StoryId == storyId && v.UserId == userId);
        }

        public virtual ICollection<IVote> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.VoteDataSource.Where(v => v.StoryId == storyId && v.Timestamp >= timestamp)
                                          .Cast<IVote>()
                                          .ToList()
                                          .AsReadOnly();
        }
    }
}