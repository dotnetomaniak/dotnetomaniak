namespace Kigg.Infrastructure
{
    using DomainObjects;
    using Repository;

    public class SpamPostprocessor : ISpamPostprocessor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoryRepository _storyRepository;
        private readonly IEmailSender _emailSender;

        public SpamPostprocessor(IUnitOfWork unitOfWork, IStoryRepository storyRepository, IEmailSender emailSender)
        {
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(emailSender, "emailSender");

            _unitOfWork = unitOfWork;
            _storyRepository = storyRepository;
            _emailSender = emailSender;
        }

        public void Process(string source, bool isSpam, string storyUrl, IStory story)
        {
            Check.Argument.IsNotEmpty(source, "source");
            Check.Argument.IsNotEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(story, "story");

            if (isSpam)
            {
                Log.Warning("Possible spam story submitted : {0}, {1}, {2}, {3}".FormatWith(storyUrl, story.Title, story.Url, story.PostedBy));
                _emailSender.NotifySpamStory(storyUrl, story, source);
            }
            else
            {
                //IStory approvingStory = _storyRepository.FindById(story.Id);
                story.Approve(SystemTime.Now());

                _unitOfWork.Commit();
            }
        }

        public void Process(string source, bool isSpam, string storyUrl, IComment comment)
        {
            Check.Argument.IsNotEmpty(source, "source");
            Check.Argument.IsNotEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(comment, "story");

            if (isSpam)
            {
                Log.Warning("Possible spam comment submitted : {0}, {1}, {2}".FormatWith(storyUrl, comment.ForStory.Title, comment.ByUser.UserName));

                // Send Mail to Notify the Support that a Spam is submitted.
                _emailSender.NotifySpamComment(storyUrl, comment, source);
            }
        }
    }
}