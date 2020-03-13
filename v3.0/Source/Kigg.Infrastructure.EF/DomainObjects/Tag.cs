using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;
using Kigg.Infrastructure.DomainRepositoryExtensions;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class Tag: Entity, ITag
    {
        [NotMapped]
        private int _storyCount = -1;

        public string UniqueName { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserTag> UserTags { get; set; }
        public virtual ICollection<StoryTag> StoryTags { get; set; }

        [NotMapped]
        public int StoryCount { get{
                if (_storyCount == -1)
                {
                    _storyCount = this.GetStoryCount();
                }

                return _storyCount;
            }
        }
    }
}