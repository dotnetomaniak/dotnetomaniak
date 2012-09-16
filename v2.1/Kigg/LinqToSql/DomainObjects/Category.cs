namespace Kigg.DomainObjects
{
    using Infrastructure;
    using Repository;

    public partial class Category : ICategory
    {
        private int _storyCount = -1;

        public int StoryCount
        {
            get
            {
                if (_storyCount == -1)
                {
                    _storyCount = IoC.Resolve<IStoryRepository>().CountByCategory(Id);
                }

                return _storyCount;
            }
        }
    }
}