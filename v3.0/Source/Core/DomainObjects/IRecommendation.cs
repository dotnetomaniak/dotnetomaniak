using System;
using Kigg.DomainObjects;

namespace Kigg.Core.DomainObjects
{

    public partial interface IRecommendation : IEntity
    {
        string RecommendationLink
        {
            get; 
            set;
        }
        string RecommendationTitle
        {
            get; 
            set;
        }
        string ImageLink
        {
            get; 
            set;
        }
        string ImageTitle
        {
            get; 
            set;
        }
        DateTime CreatedAt
        {
            get; 
            set;
        }
        DateTime StartTime
        {
            get;
            set;
        }

        DateTime EndTime
        {
            get; 
            set;
        }
    }
}

