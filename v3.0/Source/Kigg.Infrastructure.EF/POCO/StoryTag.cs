namespace Kigg.Infrastructure.EF.POCO
{
    public class StoryTag
    {
        public System.Guid StoryId { get; set; }
        public Story Story { get; set; }

        public System.Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}