using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class UserAchievement: IUserAchievement
    {
        public System.DateTime DateAchieved { get; set; }

        public System.Guid AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; }

        public System.Guid UserId { get; set; }
        public virtual User User { get; set; }

        public bool Displayed { get; set; }

        [NotMapped]
        IAchievement IUserAchievement.Achievement => (IAchievement) this.Achievement;
    }
}