namespace Kigg.Infrastructure.EF.POCO
{
    public class Recommendation
    {
        public System.Guid Id { get; set; }

        public string RecommendationLink { get; set; }

        public string RecommendationTitle { get; set; }

        public string ImageLink { get; set; }

        public string ImageTitle { get; set; }

        public System.DateTime CreatedAt { get; set; }

        public System.DateTime StartTime { get; set; }

        public System.DateTime EndTime { get; set; }

        public int Position { get; set; }

        public string Email { get; set; }

        public bool NotificationIsSent { get; set; }

        public bool IsBanner { get; set; }
    }
}