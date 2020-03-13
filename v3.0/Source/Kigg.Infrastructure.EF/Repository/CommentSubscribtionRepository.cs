using System;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class CommentSubscribtionRepository: BaseRepository<CommentSubscribtion>, ICommentSubscribtionRepository
    {
        private readonly DotnetomaniakContext _context;

        public CommentSubscribtionRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(ICommentSubscribtion entity)
        {
            base.Add((CommentSubscribtion)entity);
        }

        public void Remove(ICommentSubscribtion entity)
        {
            base.Remove((CommentSubscribtion)entity);
        }

        public int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return _context.CommentSubscribtions.Count(cs => cs.StoryId == storyId);
        }

        public ICommentSubscribtion FindById(Guid storyId, Guid userId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return _context.CommentSubscribtions.SingleOrDefault(cs => cs.StoryId == storyId && cs.UserId == userId);
        }
    }
}