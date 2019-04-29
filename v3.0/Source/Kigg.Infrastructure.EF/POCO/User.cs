using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kigg.Infrastructure.EF.POCO
{
    public class User
    {
        public System.Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public bool IsLockedOut { get; set; }

        public DomainObjects.Roles Role { get; set; }

        public System.DateTime LastActivityAt { get; set; }

        public System.DateTime CreatedAt { get; set; }

        public string FbId { get; set; }

        public ICollection<UserTag> UserTags { get; set; }

        public ICollection<CommentSubscribtion> CommentSubscribtions { get; set; }

        public ICollection<Story> Stories { get; set; }

        public ICollection<StoryComment> StoryComments { get; set; }

        public ICollection<StoryMarkAsSpam> StoryMarkAsSpams { get; set; }

        public ICollection<StoryVote> StoryVotes { get; set; }

        public ICollection<UserScore> UserScores { get; set; }

        public ICollection<UserAchievement> UserAchievements { get; set; }
    }
}