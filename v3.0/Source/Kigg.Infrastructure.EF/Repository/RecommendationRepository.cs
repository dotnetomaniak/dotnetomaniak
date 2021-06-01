using System;
using System.Linq;
using Kigg.Core.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class RecommendationRepository: BaseRepository<Recommendation>, IRecommendationRepository
    {
        private readonly DotnetomaniakContext _context;

        public RecommendationRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(IRecommendation entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Recommendation recommendation = (Recommendation)entity;

            base.Add(recommendation);
        }

        public void Remove(IRecommendation entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Recommendation recommendation = (Recommendation)entity;

            base.Remove(recommendation);
        }

        public IRecommendation FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return _context.Recommendations.SingleOrDefault(s => s.Id == id);
        }

        public IRecommendation FindByRecommendationTitle(string recommendationTitle)
        {
            Check.Argument.IsNotEmpty(recommendationTitle, "recommendationTitle");

            return _context.Recommendations.SingleOrDefault(r => r.RecommendationTitle == recommendationTitle.Trim());
        }

        public void EditAd(IRecommendation recommendation, string recommendationLink, string recommendationTitle, string imageLink, string imageTitle, DateTime startTime, DateTime endTime, string email, int position, bool notificationIsSent, bool isBanner)
        {
            Check.Argument.IsNotNull(recommendation, "Recommendation");

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

            recommendation.IsBanner = isBanner;
        }

        public IQueryable<IRecommendation> GetAllVisible()
        {
            var now = SystemTime.Now();

            return _context.Recommendations
                .Where(r => r.StartTime < now && r.EndTime >= now)
                .OrderBy(r => r.Position);
        }

        public IQueryable<IRecommendation> GetAll()
        {
            return _context.Recommendations.OrderBy(r => r.Position);
        }

        public IQueryable<IRecommendation> GetAllDefault(int howMany)
        {
            return _context.Recommendations
                .Where(r => r.Position == 999)
                .OrderBy(r => r.StartTime).Take(howMany);
        }

        public IQueryable<IRecommendation> FindRecommendationToSendNotification(int intervalToCheckEndingRecommendationInDays)
        {

            var time = new TimeSpan(intervalToCheckEndingRecommendationInDays, 0, 0, 0);
            return _context.Recommendations.Where(x => x.NotificationIsSent == false)
                                                    .Where(x => x.EndTime < DateTime.Now + time)
                                                    .Where(x => x.Email != "");
        }
    }
}