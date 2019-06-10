using System.Collections.Generic;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class Category: Entity
    {
        public string UniqueName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Story> Stories { get; set; }
    }
}