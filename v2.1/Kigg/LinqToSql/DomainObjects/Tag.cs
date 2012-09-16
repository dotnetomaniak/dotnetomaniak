namespace Kigg.DomainObjects
{
    using Infrastructure;
    using Repository;

    public partial class Tag : ITag
    {
        private int _storyCount = -1;

        public int StoryCount
        {
            get
            {
                if (_storyCount == -1)
                {
                    _storyCount = IoC.Resolve<IStoryRepository>().CountByTag(Id);
                }

                return _storyCount;
            }
        }
    }
}