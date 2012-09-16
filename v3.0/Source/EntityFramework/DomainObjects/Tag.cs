using System;

namespace Kigg.EF.DomainObjects
{
    using Infrastructure.DomainRepositoryExtensions;
    using Kigg.DomainObjects;

    public partial class Tag : ITag
    {
        private int _storyCount = -1;
        
        [NonSerialized]
        private EntityCollection<IStory, Story> _stories;

        [NonSerialized]
        private EntityCollection<IUser, User> _users;

        public int StoryCount
        {
            get
            {
                if (_storyCount == -1)
                {
                    _storyCount = this.GetStoryCount();
                }

                return _storyCount;
            }
        }

#if(DEBUG)
        internal IEntityCollection<IStory> Stories
#else
        private IEntityCollection<IStory> Stories
#endif        
        {
            get
            {
                EntityHelper.EnsureEntityCollection(ref _stories, StoriesInternal);
                EntityHelper.EnsureEntityCollectionLoaded(_stories);
                return _stories;
            }
            set
            {
                var stories = value as EntityCollection<IStory, Story>;
                if (stories == null)
                {
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<IStory, Story>");
                }
                _stories = stories;
            }
        }

#if(DEBUG)
        internal IEntityCollection<IUser> Users
#else
        private IEntityCollection<IUser> Users
#endif
        {
            get
            {
                EntityHelper.EnsureEntityCollection(ref _users, UsersInternal);
                EntityHelper.EnsureEntityCollectionLoaded(_users);
                return _users;
            }
            set
            {
                var users = value as EntityCollection<IUser, User>;
                if (users == null)
                {
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<IUser, User>");
                }
                _users = users;
            }
        }

        internal void RemoveAllStories()
        {
            Stories.Clear();
        }

        internal void RemoveAllUsers()
        {
            Users.Clear();
        }
        
    }
}
