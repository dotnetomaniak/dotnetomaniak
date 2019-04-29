namespace Kigg.Infrastructure.EF.POCO
{
    public class StoryMarkAsSpam
    {
        public System.Guid StoryId { get; set; }
        public Story Story { get; set; }

        public System.Guid UserId { get; set; }
        public User User { get; set; }

        public string IPAddress { get; set; }

        public System.DateTime Timestamp { get; set; }
    }
}