using System;
using System.Threading;
using System.Web;
using System.Web.Caching;
using Kigg.Infrastructure;
using Kigg.LinqToSql.Repository;
using Cache = System.Web.Caching.Cache;
using Kigg.Repository;
using Kigg.Core.DomainObjects;

namespace Kigg.Web.Jobs
{
    public class EndRecommendationNotificationJob : IBootstrapperTask
    {
        private readonly IEmailSender _emailSender;
        private int _intervalToCheckEndingRecommendationInDays;
        private int _beginBusinessHours;
        private int _finishBusinessHours;

        public EndRecommendationNotificationJob(IEmailSender emailSender, int intervalToCheckEndingRecommendationInDays, int beginBusinessHours, int finishBusinessHours)
        {
            _intervalToCheckEndingRecommendationInDays = intervalToCheckEndingRecommendationInDays;
            _emailSender = emailSender;
            _beginBusinessHours = beginBusinessHours;
            _finishBusinessHours = finishBusinessHours;
            Insert();
        }

        private void Insert()
        {
            HttpRuntime.Cache.Add(GetType().ToString(),
                                  this, null, Cache.NoAbsoluteExpiration,
                                  Interval, CacheItemPriority.Normal,
                                  Callback);
        }

        private void Callback(string key, object value, CacheItemRemovedReason reason)
        {         
            if (reason != CacheItemRemovedReason.Expired)
                return;
            try
            {
                if (WithinBusinessHours(_beginBusinessHours, _finishBusinessHours))
                {
                    CheckRecommendationsEndAndSendNotification();
                }
            }
            finally
            {
                Insert();
            }
        }

        private static bool WithinBusinessHours(int beginBusinessHours, int finishBusinessHours)
        {
            return DateTime.UtcNow.Hour >= beginBusinessHours && DateTime.UtcNow.Hour <= finishBusinessHours;
        }

        private void CheckRecommendationsEndAndSendNotification()
        {
            using (var databaseFactory = new DatabaseFactory(IoC.Resolve<IConnectionString>()))
            {
                using (IUnitOfWork unitOfWork = new Kigg.LinqToSql.Repository.UnitOfWork(databaseFactory))
                {
                    var _recommendationRepository = new RecommendationRepository(databaseFactory);
                    var recommendations = _recommendationRepository.FindRecommendationToSendNotification(_intervalToCheckEndingRecommendationInDays);

                    foreach (IRecommendation recommendation in recommendations)
                    {
                        _emailSender.NotifyRecommendationEnds(recommendation);
                        recommendation.NotificationIsSent = true;
                    }
                    unitOfWork.Commit();
                }
            }
        }

        protected TimeSpan Interval
        {
            get { return new TimeSpan(6, 0, 0); }
        }

        public void Execute()
        {
        }
    }
}