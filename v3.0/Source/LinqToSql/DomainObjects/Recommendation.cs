using System;
using Kigg.Core.DomainObjects;
using Kigg.DomainObjects;

namespace Kigg.LinqToSql.DomainObjects
{

    public partial class Recommendation : IRecommendation
    {
        public void Add(string recommendationLink, string recommendationTitle, string imageLink, string imageTitle, string startTime, string endTime)
        {
            Check.Argument.IsNotEmpty(recommendationLink, "RecommendationLink");
            Check.Argument.IsNotEmpty(recommendationTitle, "RecommendationTitle");
            Check.Argument.IsNotEmpty(imageLink, "ImageLink");
            Check.Argument.IsNotEmpty(imageTitle, "ImageTitle");
            Check.Argument.IsNotEmpty(startTime, "StartTime");
            Check.Argument.IsNotEmpty(endTime, "EndTime");
            
        }
    }
}
