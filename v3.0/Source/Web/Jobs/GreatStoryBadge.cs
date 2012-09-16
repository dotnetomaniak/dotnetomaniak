using System;
using Kigg.LinqToSql.Repository;

namespace Kigg.Web.Jobs
{
    public class GreatStoryBadge : AwardBadgeJob
    {
        public Guid Id = new Guid("4FAFEC17-5069-46BA-A0EB-217D49C889B7");
        protected override void AwardBadges(IDatabaseFactory databaseFactory)
        {
            databaseFactory.Get().GreatStory();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 17, 0); }
        }
    }
}