using System;
using System.Collections.Generic;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class MarkAsSpamRepository: BaseRepository<StoryMarkAsSpam>, IMarkAsSpamRepository
    {
        private readonly DotnetomaniakContext _context;

        public MarkAsSpamRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(IMarkAsSpam entity)
        {
            base.Add((StoryMarkAsSpam)entity);
        }

        public void Remove(IMarkAsSpam entity)
        {
            base.Remove((StoryMarkAsSpam)entity);
        }

        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return _context.StoryMarkAsSpams.Count(m => m.StoryId == storyId);
        }

        public virtual IMarkAsSpam FindById(Guid storyId, Guid userId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return _context.StoryMarkAsSpams.SingleOrDefault(m => m.StoryId == storyId && m.UserId == userId);
        }

        public virtual ICollection<IMarkAsSpam> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return _context.StoryMarkAsSpams.Where(m => m.StoryId == storyId && m.Timestamp >= timestamp)
                                                .Cast<IMarkAsSpam>()
                                                .ToList()
                                                .AsReadOnly();
        }
    }
}