using System;

namespace Kigg.Web.Jobs
{
    public class GlobetrotterBadge : AwardBadgeJob
    {
        public Guid Id = new Guid("C264F476-ECF0-4CEE-91FF-7B08642A56AC");
        protected override void AwardBadges(LinqToSql.Repository.IDatabaseFactory databaseFactory)
        {
            databaseFactory.Get().Globetrotter();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 28, 0); }
        }
    }
}