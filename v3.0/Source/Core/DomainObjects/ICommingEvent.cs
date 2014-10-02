using Kigg.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kigg.Core.DomainObjects
{
    public partial interface class ICommingEvent : IEntity
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

        DateTime EventDate
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

        int Position
        {
            get;
            set;
        }
    }
}
