using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kigg.Infrastructure.EF.POCO
{
    public class Achievement: Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public ICollection<UserAchievement> UserAchievements { get; set; }
    }
}