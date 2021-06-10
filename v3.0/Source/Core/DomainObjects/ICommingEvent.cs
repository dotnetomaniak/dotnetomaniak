using Kigg.DomainObjects;
using System;

namespace Kigg.Core.DomainObjects
{
    public partial interface ICommingEvent : IEntity
    {
        string EventLink
        {
            get;
            set;
        }

        string EventName
        {
            get;
            set;
        }

        string GoogleEventId
        {
            get;
            set;
        }

        DateTime EventDate
        {
            get;
            set;
        }

        DateTime? EventEndDate
        {
            get;
            set;
        }

        string EventCity
        {
            get;
            set;
        }

        string EventPlace
        {
            get;
            set;
        }

        string EventLead
        {
            get;
            set;
        }

        string Email
        {
            get;
            set;
        }

        bool? IsOnline
        {
            get;
            set;
        }

        bool? IsApproved
        {
            get;
            set;
        }
    }
}
