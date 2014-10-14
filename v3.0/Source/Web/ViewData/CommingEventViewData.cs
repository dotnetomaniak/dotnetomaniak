using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kigg.Web.ViewData
{
    public class CommingEventViewData : BaseViewData
    {
        public string EventLink { get; set; }
        public string EventName { get; set; }        
        public string Id { get; set; }        
        public DateTime EventDate { get; set; }
    }
}