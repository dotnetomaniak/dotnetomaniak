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

        public EndRecommendationNotificationJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
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
                using (var databaseFactory = new DatabaseFactory(IoC.Resolve<IConnectionString>()))
                {
                    var _recommendationRepository = new RecommendationRepository(databaseFactory);

                    var recommendations = _recommendationRepository.FindRecommendationToSendNotification();


                    foreach (IRecommendation recommendation in recommendations)
                    {
                        _emailSender.NotifyRecommendationEnds(recommendation);

                        _recommendationRepository.EditAd(recommendation, recommendation.RecommendationLink, recommendation.RecommendationLink, recommendation.ImageLink,
                                                         recommendation.ImageTitle, recommendation.StartTime, recommendation.EndTime, recommendation.Email,
                                                         recommendation.Position, true);
                        
                    }
                }
            }
            finally
            {
                Insert();
            }
        }


        protected TimeSpan Interval
        {
            get { return new TimeSpan(0, 0, 3); }
        }

        public void Execute()
        {
        }
    }
}