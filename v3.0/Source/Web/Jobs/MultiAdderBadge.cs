using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kigg.Web.Jobs
{
    public class MultiAdderBadge : AwardBadgeJob
    {
        public Guid Id = new Guid("8f7a7b7f-6a21-4ac1-b6b7-3b4a51c3f4a2");
        protected override void AwardBadges(LinqToSql.Repository.IDatabaseFactory databaseFactory)
        {
            databaseFactory.Get().MultiAdder();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 21, 0); }
        }
    }
}