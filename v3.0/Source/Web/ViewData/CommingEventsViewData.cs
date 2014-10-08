using System.Linq;
using Kigg.Core.DomainObjects;

namespace Kigg.Web.ViewData
{
    public class CommingEventsViewData : BaseViewData
    {
        public IQueryable<CommingEventViewData> CommingEvents { get; set; }
    }
}