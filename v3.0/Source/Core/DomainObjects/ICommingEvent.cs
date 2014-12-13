using Kigg.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        DateTime EventDate
        {
            get;
            set;
        }

        DateTime CreatedAt
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
    }
}
