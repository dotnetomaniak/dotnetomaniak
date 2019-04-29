namespace Kigg.Infrastructure.EF.POCO
{
    public class StoryComment
    {
        public System.Guid Id { get; set; }

        public string HtmlBody { get; set; }

        public System.Data.Linq.Link<string> TextBody { get; set; }

        public System.DateTime CreatedAt { get; set; }

        public System.Guid StoryId { get; set; }
        public Story Story { get; set; }

        public System.Guid UserId { get; set; }
        public User User { get; set; }

        public string IPAddress { get; set; }

        public bool IsOffended { get; set; }
    }
}