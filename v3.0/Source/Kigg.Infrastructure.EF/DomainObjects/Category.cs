using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;
using Kigg.Infrastructure.DomainRepositoryExtensions;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class Category: Entity, ICategory
    {
        [NotMapped]
        private int _storyCount = -1;

        public string UniqueName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Story> Stories { get; set; }

        [NotMapped]
        public int StoryCount => _storyCount == -1? this.GetStoryCount() : _storyCount;
    }
}