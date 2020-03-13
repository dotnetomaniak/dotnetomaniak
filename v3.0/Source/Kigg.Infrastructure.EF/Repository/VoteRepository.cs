using System;
using System.Collections.Generic;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class VoteRepository: BaseRepository<StoryVote>, IVoteRepository
    {
        private readonly DotnetomaniakContext _context;
        private readonly ICache _cache;

        public VoteRepository(DotnetomaniakContext context, ICache cache) : base(context)
        {
            _context = context;
            _cache = cache;
        }

        public void Add(IVote entity)
        {
            base.Add((StoryVote)entity);
        }

        public void Remove(IVote entity)
        {
            base.Remove((StoryVote)entity);
        }

        public void InvalidateCacheForStory(Guid storyId)
        {
            var cachedVotes = _cache.Get<Dictionary<Guid, int>>("storyVoteCount");
            if (cachedVotes != null)
            {
                var count = _context.StoryVotes.Count(v => v.StoryId == storyId);
                cachedVotes[storyId] = count;
            }
        }
        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            var cachedVotes = _cache.Get<Dictionary<Guid, int>>("storyVoteCount");
            if (cachedVotes == null)
            {
                cachedVotes = _context.StoryVotes.GroupBy(x => x.StoryId)
                    .Select(x => new { StoryId = x.Key, Count = x.Count() })
                    .ToDictionary(x => x.StoryId, x => x.Count);
                _cache.Set("storyVoteCount", cachedVotes, DateTime.Now.AddMinutes(3));
            }

            return cachedVotes.ContainsKey(storyId) ? cachedVotes[storyId] : 0;

            //return _context.StoryVotes.Count(v => v.StoryId == storyId);
        }

        public virtual IVote FindById(Guid storyId, Guid userId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return _context.StoryVotes.SingleOrDefault(v => v.StoryId == storyId && v.UserId == userId);
        }

        public virtual ICollection<IVote> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return _context.StoryVotes.Where(v => v.StoryId == storyId && v.Timestamp >= timestamp)
                                          .Cast<IVote>()
                                          .ToList()
                                          .AsReadOnly();
        }
    }
}