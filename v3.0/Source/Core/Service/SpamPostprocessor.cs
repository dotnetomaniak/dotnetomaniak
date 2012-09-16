namespace Kigg.Service
{
    using DomainObjects;
    using Infrastructure;
    using Repository;

    public class SpamPostprocessor : ISpamPostprocessor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventAggregator _eventAggregator;
        private readonly IStoryRepository _storyRepository;

        public SpamPostprocessor(IUnitOfWork unitOfWork, IEventAggregator eventAggregator, IStoryRepository storyRepository)
        {
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");
            Check.Argument.IsNotNull(eventAggregator, "eventAggregator");
            Check.Argument.IsNotNull(storyRepository, "storyRepository");

            _unitOfWork = unitOfWork;
            _eventAggregator = eventAggregator;
            _storyRepository = storyRepository;
        }

        public void Process(string source, bool isSpam, string detailUrl, IStory story)
        {
            Check.Argument.IsNotEmpty(source, "source");
            Check.Argument.IsNotEmpty(detailUrl, "detailUrl");
            Check.Argument.IsNotNull(story, "story");

            story = _storyRepository.FindById(story.Id);

            if (isSpam)
            {
                Log.Warning("Possible spam story submitted : {0}, {1}, {2}, {3}".FormatWith(detailUrl, story.Title, story.Url, story.PostedBy));
                _eventAggregator.GetEvent<PossibleSpamStoryEvent>().Publish(new PossibleSpamStoryEventArgs(story, source, detailUrl));
            }
            else
            {
                story.Approve(SystemTime.Now());

                _eventAggregator.GetEvent<StorySubmitEvent>().Publish(new StorySubmitEventArgs(story, detailUrl));

                _unitOfWork.Commit();
            }
        }

        public void Process(string source, bool isSpam, string detailUrl, IComment comment)
        {
            Check.Argument.IsNotEmpty(source, "source");
            Check.Argument.IsNotEmpty(detailUrl, "detailUrl");
            Check.Argument.IsNotNull(comment, "story");

            IStory story = _storyRepository.FindById(comment.ForStory.Id);
            comment = story.FindComment(comment.Id);

            if (isSpam)
            {
                Log.Warning("Possible spam comment submitted : {0}, {1}, {2}".FormatWith(detailUrl, comment.ForStory.Title, comment.ByUser.UserName));
                _eventAggregator.GetEvent<PossibleSpamCommentEvent>().Publish(new PossibleSpamCommentEventArgs(comment, source, detailUrl));
            }
            else
            {
                _eventAggregator.GetEvent<CommentSubmitEvent>().Publish(new CommentSubmitEventArgs(comment, detailUrl));
            }
        }
    }
}