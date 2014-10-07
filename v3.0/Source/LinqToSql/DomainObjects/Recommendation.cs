using System;
    using Kigg.Core.DomainObjects;
    using Kigg.DomainObjects;

namespace Kigg.LinqToSql.DomainObjects
{

    public partial class Recommendation : IRecommendation
    {
        public void Add(string recommendationLink, string recommendationTitle, string imageLink, string imageTitle, DateTime startTime, DateTime endTime, int position, string email, bool notificationIsSent)
        {
            Check.Argument.IsNotEmpty(recommendationLink, "RecommendationLink");
            Check.Argument.IsNotEmpty(recommendationTitle, "RecommendationTitle");
            Check.Argument.IsNotEmpty(imageLink, "ImageLink");
            Check.Argument.IsNotEmpty(imageTitle, "ImageTitle");
        }


        //public string Email
        //{
        //    get;
        //    set;
        //}

        //public bool NotificationIsSent
        //{
        //    get;
        //    set;
        //}
    }
}
