using System.Linq;
using Kigg.Core.DomainObjects;

namespace Kigg.Web.ViewData
{
    public class RecommendationsViewData : BaseViewData
    {
        public IQueryable<RecommendationViewData> Recommendations { get; set; }
    }
}