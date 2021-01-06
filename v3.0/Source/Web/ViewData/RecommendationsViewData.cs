using System.Collections.Generic;

namespace Kigg.Web.ViewData
{
    public class RecommendationsViewData : BaseViewData
    {
        public IList<RecommendationViewData> Recommendations { get; set; }
    }
}