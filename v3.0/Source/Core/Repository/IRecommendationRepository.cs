using System;
using Kigg.Core.DomainObjects;

namespace Kigg.Repository
{
    public interface IRecommendationRepository : IRepository<IRecommendation>
    {
        IRecommendation FindById(Guid id);

        IRecommendation FindByRecommendationTitle(string recommendationTitle);
    }
}
