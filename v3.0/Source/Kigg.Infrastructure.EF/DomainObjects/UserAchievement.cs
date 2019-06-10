using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class UserAchievement: Entity, IUserAchievement
    {
        public System.DateTime DateAchieved { get; set; }

        public System.Guid AchievementId { get; set; }
        public Achievement Achievement { get; set; }

        public System.Guid UserId { get; set; }
        public User User { get; set; }

        public bool Displayed { get; set; }

        [NotMapped]
        IAchievement IUserAchievement.Achievement => (IAchievement) this.Achievement;
    }
}