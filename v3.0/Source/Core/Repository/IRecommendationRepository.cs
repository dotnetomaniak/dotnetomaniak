using System;
using System.Linq;
using Kigg.Core.DomainObjects;

namespace Kigg.Repository
{
    public interface IRecommendationRepository : IRepository<IRecommendation>
    {
        IRecommendation FindById(Guid id);

        void EditAd(IRecommendation recommendation, string recommendationLink, string recommendationTitle, string imageLink, string imageTitle, DateTime startTime, DateTime endTime, string email, int position, bool notificationIsSent);

        IRecommendation FindByRecommendationTitle(string recommendationTitle);
        
        IQueryable<IRecommendation> GetAllVisible();

        IQueryable<IRecommendation> GetAll();

        IQueryable<IRecommendation> GetAllDefault(int howMany);

        IQueryable<IRecommendation> FindRecommendationToSendNotification(int intervalToCheckEndingRecommendationInDays);
        
    }
}
