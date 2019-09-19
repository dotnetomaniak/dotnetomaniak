namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class StoryTag: Entity
    {
        public System.Guid StoryId { get; set; }
        public virtual Story Story { get; set; }

        public System.Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}