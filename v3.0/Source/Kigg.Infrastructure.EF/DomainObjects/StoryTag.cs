﻿namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class StoryTag: Entity
    {
        public System.Guid StoryId { get; set; }
        public Story Story { get; set; }

        public System.Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}