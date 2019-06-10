using System;

namespace Kigg.Infrastructure.EF.POCO
{
    public class CommentSubscribtion: Entity
    {
        public Guid StoryId { get; set; }
        public Story Story { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}