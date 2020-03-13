using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class Achievement: IAchievement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public virtual ICollection<UserAchievement> UserAchievements { get; set; }

        [NotMapped]
        public string UniqueName => Name;

        [NotMapped]
        public Types AchievementType => (Types) this.Type;

        [NotMapped] 
        public DateTime CreatedAt => DateTime.UtcNow;

    }
}