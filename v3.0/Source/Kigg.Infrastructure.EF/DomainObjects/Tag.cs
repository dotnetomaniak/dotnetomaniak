using System.Collections.Generic;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class Tag: Entity
    {
        public string UniqueName { get; set; }
        public string Name { get; set; }

        public ICollection<UserTag> UserTags { get; set; }
        public ICollection<StoryTag> StoryTags { get; set; }
    }
}