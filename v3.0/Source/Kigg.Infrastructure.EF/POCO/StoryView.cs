namespace Kigg.Infrastructure.EF.POCO
{
    public class StoryView
    {
        public long Id { get; set; }

        public System.Guid StoryId { get; set; }
        public Story Story { get; set; }

        public System.DateTime Timestamp { get; set; }

        public string IPAddress { get; set; }
    }
}