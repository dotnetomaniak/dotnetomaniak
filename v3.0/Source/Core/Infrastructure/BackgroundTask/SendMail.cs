namespace Kigg.Infrastructure
{
    using Service;

    public class SendMail : BaseBackgroundTask
    {
        private readonly IEmailSender _emailSender;

        private SubscriptionToken _commentSubmitToken;
        private SubscriptionToken _commentMarkAsOffendedToken;
        private SubscriptionToken _commentSpamToken;
        private SubscriptionToken _storyApproveToken;
        private SubscriptionToken _storyDeleteToken;
        private SubscriptionToken _storyMarkAsSpamToken;
        private SubscriptionToken _storySpamToken;
        private SubscriptionToken _storyPublishToken;
        private SubscriptionToken _possibleStorySpamToken;
        private SubscriptionToken _possibleCommentSpamToken;

        public SendMail(IEventAggregator eventAggregator, IEmailSender emailSender) : base(eventAggregator)
        {
            Check.Argument.IsNotNull(emailSender, "emailSender");

            _emailSender = emailSender;
        }

        protected override void OnStart()
        {
            if (!IsRunning)
            {
                _commentSubmitToken = Subscribe<CommentSubmitEvent, CommentSubmitEventArgs>(CommentSubmitted);
                _commentMarkAsOffendedToken = Subscribe<CommentMarkAsOffendedEvent, CommentMarkAsOffendedEventArgs>(CommentMarkedAsOffended);
                _commentSpamToken = Subscribe<CommentSpamEvent, CommentSpamEventArgs>(CommentSpammed);
                _storyApproveToken = Subscribe<StoryApproveEvent, StoryApproveEventArgs>(StoryApproved);
                _storyDeleteToken = Subscribe<StoryDeleteEvent, StoryDeleteEventArgs>(StoryDeleted);
                _storyMarkAsSpamToken = Subscribe<StoryMarkAsSpamEvent, StoryMarkAsSpamEventArgs>(StoryMarkedAsSpam);
                _storySpamToken = Subscribe<StorySpamEvent, StorySpamEventArgs>(StorySpammed);
                _storyPublishToken = Subscribe<StoryPublishEvent, StoryPublishEventArgs>(StoryPublished);
                _possibleStorySpamToken = Subscribe<PossibleSpamStoryEvent, PossibleSpamStoryEventArgs>(PossibleSpamStoryDetected);
                _possibleCommentSpamToken = Subscribe<PossibleSpamCommentEvent, PossibleSpamCommentEventArgs>(PossibleSpamCommentDetected);
            }
        }

        protected override void OnStop()
        {
            if (IsRunning)
            {
                Unsubscribe<CommentSubmitEvent>(_commentSubmitToken);
                Unsubscribe<CommentMarkAsOffendedEvent>(_commentMarkAsOffendedToken);
                Unsubscribe<CommentSpamEvent>(_commentSpamToken);
                Unsubscribe<StoryApproveEvent>(_storyApproveToken);
                Unsubscribe<StoryDeleteEvent>(_storyDeleteToken);
                Unsubscribe<StoryMarkAsSpamEvent>(_storyMarkAsSpamToken);
                Unsubscribe<StorySpamEvent>(_storySpamToken);
                Unsubscribe<StoryPublishEvent>(_storyPublishToken);
                Unsubscribe<PossibleSpamStoryEvent>(_possibleStorySpamToken);
                Unsubscribe<PossibleSpamCommentEvent>(_possibleCommentSpamToken);
            }
        }

        internal void CommentSubmitted(CommentSubmitEventArgs eventArgs)
        {
            _emailSender.SendComment(eventArgs.DetailUrl, eventArgs.Comment, eventArgs.Comment.ForStory.Subscribers);
        }

        internal void CommentMarkedAsOffended(CommentMarkAsOffendedEventArgs eventArgs)
        {
            _emailSender.NotifyCommentAsOffended(eventArgs.DetailUrl, eventArgs.Comment, eventArgs.User);
        }

        internal void CommentSpammed(CommentSpamEventArgs eventArgs)
        {
            _emailSender.NotifyConfirmSpamComment(eventArgs.DetailUrl, eventArgs.Comment, eventArgs.User);
        }

        internal void StoryApproved(StoryApproveEventArgs eventArgs)
        {
            _emailSender.NotifyStoryApprove(eventArgs.DetailUrl, eventArgs.Story, eventArgs.User);
        }

        internal void StoryDeleted(StoryDeleteEventArgs eventArgs)
        {
            _emailSender.NotifyStoryDelete(eventArgs.Story, eventArgs.User);
        }

        internal void StoryMarkedAsSpam(StoryMarkAsSpamEventArgs eventArgs)
        {
            _emailSender.NotifyStoryMarkedAsSpam(eventArgs.DetailUrl, eventArgs.Story, eventArgs.User);
        }

        internal void StorySpammed(StorySpamEventArgs eventArgs)
        {
            _emailSender.NotifyConfirmSpamStory(eventArgs.DetailUrl, eventArgs.Story, eventArgs.User);
        }

        internal void StoryPublished(StoryPublishEventArgs eventArgs)
        {
            _emailSender.NotifyPublishedStories(eventArgs.Timestamp, eventArgs.PublishedStories);
        }

        internal void PossibleSpamStoryDetected(PossibleSpamStoryEventArgs eventArgs)
        {
            _emailSender.NotifySpamStory(eventArgs.DetailUrl, eventArgs.Story, eventArgs.Source);
        }

        internal void PossibleSpamCommentDetected(PossibleSpamCommentEventArgs eventArgs)
        {
            _emailSender.NotifySpamComment(eventArgs.DetailUrl, eventArgs.Comment, eventArgs.Source);
        }
    }
}