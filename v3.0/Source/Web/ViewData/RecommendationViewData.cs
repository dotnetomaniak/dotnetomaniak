using System;
using System.Web.Mvc;
using System.Web.WebPages.Instrumentation;

namespace Kigg.Web.ViewData
{
    public class RecommendationViewData : BaseViewData
    {
        public string UrlLink { get; set; }
        public string UrlTitle { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
        public string Id { get; set; }
        public int Position { get; set; }
        public DateTime EndTime { get; set; }
        public string HowLongAdShows()
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