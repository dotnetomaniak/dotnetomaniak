using System;
using Kigg.DomainObjects;

namespace Kigg.LinqToSql.DomainObjects
{
    partial class Achievement : IAchievement
    {
        public Types AchievementType
        {
            get { return (Types) this.Type; }
        }

        public string UniqueName
        {
            get { return Name; }
        }


        public DateTime CreatedAt
        {
            get { return DateTime.UtcNow; }
        }
    }
}
