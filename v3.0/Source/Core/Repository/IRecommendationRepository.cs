using System;
using System.Linq;
using Kigg.Core.DomainObjects;

namespace Kigg.Repository
{
    public interface IRecommendationRepository : IRepository<IRecommendation>
    {
        IRecommendation FindById(Guid id);

        void EditAd (IRecommendation recommendation, string recommendationLink, string recommendationTitle, string imageLink, string imageTitle, DateTime startTime, DateTime endTime, int position );

        IRecommendation FindByRecommendationTitle(string recommendationTitle);
        IQueryable<IRecommendation> GetAllVisible();

        IQueryable<IRecommendation> GetAll();
    }
}
