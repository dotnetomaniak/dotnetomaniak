namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class StoryTag
    {
        public System.Guid StoryId { get; set; }
        public virtual Story Story { get; set; }

        public System.Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}