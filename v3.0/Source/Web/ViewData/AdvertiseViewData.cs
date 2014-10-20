using System;

namespace Kigg.Web.ViewData
{
    public class AdvertiseViewData
    {
        public string Id { get; set; }
        public string RecommendationLink { get; set; }
        public string RecommendationTitle { get; set; }
        public string ImageLink { get; set; }
        public string ImageTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Email { get; set; }
        public int Position { get; set; }
        public bool NotificationIsSent { get; set; }
        public string IsBanner { get; set; }

        public AdvertiseViewData()
        {
            IsBanner = "";
            NotificationIsSent = false;
            Position = 999;
        }
    }
}