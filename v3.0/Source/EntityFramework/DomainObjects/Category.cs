namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;
    using Infrastructure.DomainRepositoryExtensions;

    public partial class Category : ICategory
    {
        private int _storyCount = -1;

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
    }
}
