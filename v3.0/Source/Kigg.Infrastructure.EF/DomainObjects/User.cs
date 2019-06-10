using System.Collections.Generic;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class User: Entity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public bool IsLockedOut { get; set; }

        public Kigg.DomainObjects.Roles Role { get; set; }

        public System.DateTime LastActivityAt { get; set; }

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