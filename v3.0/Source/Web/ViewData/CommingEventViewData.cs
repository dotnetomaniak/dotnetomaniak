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
        public string ImageTitle { get; set; }
        public string ImageLink { get; set; }
        public string Id { get; set; }
        public int Position { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EndTime { get; set; }

        public string HowLongEventShows()
        {
            string tableRowClass = "Visible";
            DateTime now = SystemTime.Now();

            if (EndTime > now)
            {
                if (EndTime < now.AddDays(5))
                {
                    tableRowClass = EndTime < now.AddDays(1) ? "OverdueTommorow" : "OverdueDuringFiveDays";
                }
            }
            else
            {
                tableRowClass = Position == 999 ? "Default" : "Overdued";
            }
            return tableRowClass;
        }
    }
}