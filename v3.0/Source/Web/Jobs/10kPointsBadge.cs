using System;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.LinqToSql.Repository;
using Kigg.Repository;

namespace Kigg.Web.Jobs
{
    public class _10kPointsBadge : AwardBadgeJob
    {        
        public Guid Id = new Guid("0AC5293B-55F4-48F0-A8D5-CE427406BA2B");     

        protected override void AwardBadges(IDatabaseFactory database)
        {
            database.Get()._10kPoints();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 25, 0); }
        }
    }
}