using System;
using Kigg.LinqToSql.Repository;

namespace Kigg.Web.Jobs
{
    public class UpVoterBadge : AwardBadgeJob
    {
        public Guid Id = new Guid("76ACDC60-4D9F-4633-96CC-683CA6105F17");
        protected override void AwardBadges(IDatabaseFactory databaseFactory)
        {
            databaseFactory.Get().UpVoter();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 11, 0); }
        }
    }
}