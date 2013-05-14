using System.Linq;

namespace Kigg.Web.ViewData
{
    public class RecommendationsViewData : BaseViewData
    {
        public RecommendationsViewData(IQueryable<RecommendationViewData> recommendations)
        {            
            Recommendations = recommendations;
        }

        public IQueryable<RecommendationViewData> Recommendations { get; private set; }
    }
}