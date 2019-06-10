using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kigg.Infrastructure.EF.POCO
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