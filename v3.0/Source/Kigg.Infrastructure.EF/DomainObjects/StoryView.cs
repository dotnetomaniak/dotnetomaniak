using System;
using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class StoryView: Entity, IStoryView
    {
        public System.Guid StoryId { get; set; }
        public Story Story { get; set; }

        public System.DateTime Timestamp { get; set; }

        public string IPAddress { get; set; }

        [NotMapped]
        public IStory ForStory => Story;

        [NotMapped]
        public string FromIPAddress => IPAddress;

        [NotMapped]
        public DateTime ViewedAt => Timestamp;
    }
}