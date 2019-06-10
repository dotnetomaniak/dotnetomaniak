using System.Collections.Generic;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class Achievement: Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public ICollection<UserAchievement> UserAchievements { get; set; }
    }
}