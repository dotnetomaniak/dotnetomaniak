using System;
using System.Linq;
using Kigg.Core.DomainObjects;
using Kigg.Infrastructure;
using Kigg.LinqToSql.DomainObjects;
using Kigg.Repository;

namespace Kigg.LinqToSql.Repository
{
    public class RecommendationRepository : BaseRepository<IRecommendation, Recommendation>, IRecommendationRepository
    {
        public RecommendationRepository(IDatabase database)
            : base(database)
        {
        }

        public RecommendationRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public override void Add(IRecommendation entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Recommendation recommendation = (Recommendation) entity;

            base.Add(recommendation);
        }

        public override void Remove(IRecommendation entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Recommendation recommendation = (Recommendation) entity;

            base.Remove(recommendation);
        }

        public IRecommendation FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.RecommendationDataSource.SingleOrDefault(s => s.Id == id);
        }

        public IRecommendation FindByRecommendationTitle(string recommendationTitle)
        {
            Check.Argument.IsNotEmpty(recommendationTitle, "recommendationTitle");

            return Database.RecommendationDataSource.SingleOrDefault(r => r.RecommendationTitle == recommendationTitle.Trim());
        }

        public void EditAd(IRecommendation recommendation, string recommendationLink, string recommendationTitle, string imageLink, string imageTitle, DateTime startTime, DateTime endTime, string email, int position, bool notificationIsSent)
        {
            Check.Argument.IsNotNull(recommendation, "Recommendation");

            using (IUnitOfWork unitOfWork = Infrastructure.UnitOfWork.Begin())
            {
                if (!string.IsNullOrEmpty(recommendationLink))
                {
                    recommendation.RecommendationLink = recommendationLink;
                }

                if (!string.IsNullOrEmpty(recommendationTitle))
                {
                    recommendation.RecommendationTitle = recommendationTitle;
                }

                if (!string.IsNullOrEmpty(imageLink))
                {
                    recommendation.ImageLink = imageLink;
                } 

                if (!string.IsNullOrEmpty(imageTitle))
                {
                    recommendation.ImageTitle = imageTitle;
                } 
                
                if (position != recommendation.Position)
                {
                    recommendation.Position = position;
                }

                recommendation.StartTime = startTime;

                recommendation.EndTime = endTime;

                recommendation.Email = email;

                recommendation.NotificationIsSent = notificationIsSent;

                unitOfWork.Commit();
            }
        }

        public IQueryable<IRecommendation> GetAllVisible()
        {
            var now = SystemTime.Now();

            return Database.RecommendationDataSource
                .Where(r => r.StartTime < now && r.EndTime >= now)
                .OrderBy(r => r.Position);
        }

        public IQueryable<IRecommendation> GetAll()
        {
            return Database.RecommendationDataSource.OrderBy(r => r.Position);
        }

        public IQueryable<IRecommendation> GetAllDefault(int howMany)
        {
            return Database.RecommendationDataSource
                .Where(r => r.Position == 999)
                .OrderBy(r => r.StartTime).Take(howMany);
        }

        public IQueryable<IRecommendation> FindRecommendationToSendNotification()
        {
            var time = new TimeSpan(5, 0, 0, 0);
            return Database.RecommendationDataSource.Where(x=>x.NotificationIsSent == false)
                                                    .Where(x=>x.EndTime < DateTime.Now + time)
                                                    .Where(x=>x.Email != "");
        }
        
        public void SendNotifications(IQueryable<IRecommendation> recommendations)
        {
            throw new NotImplementedException();
        }
    }
}
