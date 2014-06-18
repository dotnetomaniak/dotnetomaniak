using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Kigg.DomainObjects;
using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Service;

    using Kigg.Test.Infrastructure;

    public class StoryServiceFixture : BaseFixture
    {
        private readonly Mock<IDomainObjectFactory> _factory;
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<ITagRepository> _tagRepository;
        private readonly Mock<IStoryRepository> _storyRepository;
        private readonly Mock<IMarkAsSpamRepository> _markAsSpamRepository;
        private readonly Mock<IEventAggregator> _eventAggregator;
        private readonly Mock<ISpamProtection> _spamProtection;
        private readonly Mock<ISpamPostprocessor> _spamPostProcessor;
        private readonly Mock<IContentService> _contentService;
        private readonly Mock<IHtmlSanitizer> _htmlSanitizer;

        private readonly Mock<IStoryWeightCalculator> _voteStrategy;
        private readonly Mock<IStoryWeightCalculator> _commentStrategy;
        private readonly Mock<IStoryWeightCalculator> _viewStrategy;
        private readonly Mock<IStoryWeightCalculator> _userScoreStrategy;
        private readonly Mock<IStoryWeightCalculator> _freshnessStrategy;
        private readonly Mock<IStoryWeightCalculator> _knownSourceStrategy;

        private Mock<StorySubmitEvent> _storySubmitEvent;
        private Mock<StoryDeleteEvent> _storyDeleteEvent;
        private Mock<StoryViewEvent> _storyViewEvent;
        private Mock<StoryPromoteEvent> _storyPromoteEvent;
        private Mock<StoryDemoteEvent> _storyDemoteEvent;
        private Mock<StoryMarkAsSpamEvent> _storyMarkAsSpamEvent;
        private Mock<StoryUnmarkAsSpamEvent> _storyUnmarkAsSpamEvent;
        private Mock<StorySpamEvent> _storySpamEvent;
        private Mock<CommentSpamEvent> _commentSpamEvent;
        private Mock<CommentMarkAsOffendedEvent> _commentMarkAsOffendedEvent;
        private Mock<StoryPublishEvent> _storyPublishEvent;
        private Mock<StoryApproveEvent> _storyApproveEvent;
        private Mock<StoryIncorrectlyMarkedAsSpamEvent> _storyIncorrectlyMarkedAsSpamEvent;

        private readonly StoryService _storyService;

        public StoryServiceFixture()
        {
            _factory = new Mock<IDomainObjectFactory>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _tagRepository = new Mock<ITagRepository>();
            _storyRepository = new Mock<IStoryRepository>();
            _markAsSpamRepository = new Mock<IMarkAsSpamRepository>();
            _eventAggregator = new Mock<IEventAggregator>();
            _spamProtection = new Mock<ISpamProtection>();
            _spamPostProcessor = new Mock<ISpamPostprocessor>();
            _contentService = new Mock<IContentService>();
            _htmlSanitizer = new Mock<IHtmlSanitizer>();

            _voteStrategy = new Mock<IStoryWeightCalculator>();
            _voteStrategy.SetupGet(s => s.Name).Returns("Vote");

            _commentStrategy = new Mock<IStoryWeightCalculator>();
            _commentStrategy.SetupGet(s => s.Name).Returns("Comment");

            _viewStrategy = new Mock<IStoryWeightCalculator>();
            _viewStrategy.SetupGet(s => s.Name).Returns("View");

            _userScoreStrategy = new Mock<IStoryWeightCalculator>();
            _userScoreStrategy.SetupGet(s => s.Name).Returns("User-Score");

            _knownSourceStrategy = new Mock<IStoryWeightCalculator>();
            _knownSourceStrategy.SetupGet(s => s.Name).Returns("Known-Source");

            _freshnessStrategy = new Mock<IStoryWeightCalculator>();
            _freshnessStrategy.SetupGet(s => s.Name).Returns("Freshness");

            _storyService = new StoryService(settings.Object, _factory.Object, _categoryRepository.Object, _tagRepository.Object, _storyRepository.Object, _markAsSpamRepository.Object, _eventAggregator.Object, _spamProtection.Object, _spamPostProcessor.Object, _contentService.Object, _htmlSanitizer.Object, thumbnail.Object, new []{ _voteStrategy.Object, _commentStrategy.Object, _viewStrategy.Object, _userScoreStrategy.Object, _knownSourceStrategy.Object, _freshnessStrategy.Object });
        }

        [Fact]
        public void Create_Should_Return_Null_Error_Message_When_Successfully_Submitted()
        {
            var story = new Mock<IStory>();

            var result = Create(story);

            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Also_Promote_The_Story()
        {
            var story = new Mock<IStory>();

            story.Setup(s => s.Promote(It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(true);

            Create(story);

            story.Verify();
        }

        [Fact]
        public void Create_Should_Request_Thumnbnail_To_Capture()
        {
            var story = new Mock<IStory>();

            thumbnail.Setup(t => t.Capture(It.IsAny<string>())).Verifiable();

            Create(story);

            thumbnail.Verify();
        }

        [Fact]
        public void Create_Should_Subscribe_User_For_New_Comments()
        {
            var story = new Mock<IStory>();

            story.Setup(s => s.SubscribeComment(It.IsAny<IUser>())).Verifiable();

            Create(story);

            story.Verify();
        }

        [Fact]
        public void Create_Should_Add_Tag_Using_Tag_Repository()
        {
            var story = new Mock<IStory>();

            _tagRepository.Setup(r => r.FindByName(It.IsAny<string>())).Returns((ITag) null);
            _tagRepository.Setup(r => r.Add(It.IsAny<ITag>())).Verifiable();

            Create(story);

            _tagRepository.Verify();
        }

        [Fact]
        public void Create_Should_Add_Tag_To_Story()
        {
            var story = new Mock<IStory>();
            story.Setup(s => s.AddTag(It.IsAny<ITag>())).Verifiable();

            Create(story);

            story.Verify();
        }

        [Fact]
        public void Create_Should_Add_Tag_To_User()
        {
            var user = new Mock<IUser>();
            var story = new Mock<IStory>();

            user.Setup(s => s.AddTag(It.IsAny<ITag>())).Verifiable();

            Create(story, user);

            user.Verify();
        }

        [Fact]
        public void Create_Should_Not_CheckForSpam_When_Possible_Spam_Is_Not_Allowed_And_User_CanModerate()
        {
            settings.SetupGet(s => s.AllowPossibleSpamStorySubmit).Returns(false);

            var user = new Mock<IUser>();

            user.SetupGet(u => u.Role).Returns(Roles.Moderator);

            var story = new Mock<IStory>();

            //_storyRepository.Setup().Never();
            
            Create(story, user);

            _storyRepository.Verify(r => r.CountPostedByUser(It.IsAny<Guid>()),Times.Never());
        }

        [Fact]
        public void Create_Should_Not_CheckForSpam_When_Possible_Spam_Is_Allowed_And_User_CanModerate()
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Role).Returns(Roles.Moderator);

            var story = new Mock<IStory>();

            //_storyRepository.Setup(r => r.CountPostedByUser(It.IsAny<Guid>())).Never();
            Create(story, user);
            
            _storyRepository.Verify(r => r.CountPostedByUser(It.IsAny<Guid>()), Times.Never());

            _storyRepository.Verify();
        }

        [Fact]
        public void Create_Should_Use_SpamPostProcesor_When_Spam_Is_Detected_And_Possible_Spam_Is_Allowed()
        {
            var story = new Mock<IStory>();

            _storyRepository.Setup(r => r.CountPostedByUser(It.IsAny<Guid>())).Returns(settings.Object.StorySumittedThresholdOfUserToSpamCheck -1);
            _spamProtection.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>(), It.IsAny<Action<string, bool>>())).Callback((SpamCheckContent sp, Action<string, bool> onComplete) => onComplete("foo", true));
            _spamPostProcessor.Setup(sp => sp.Process(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IStory>())).Verifiable();

            Create(story);

            _spamPostProcessor.Verify();
        }

        [Fact]
        public void Create_Should_Approve_Story_When_Spam_Is_Not_Detected_And_Possible_Spam_Is_Not_Allowed()
        {
            settings.SetupGet(s => s.AllowPossibleSpamStorySubmit).Returns(false);

            var story = new Mock<IStory>();

            _spamProtection.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(false);
            story.Setup(s => s.Approve(It.IsAny<DateTime>())).Verifiable();

            Create(story);

            story.Verify();
        }

        [Fact]
        public void Create_Should_Publish_Story_Submit_Event_When_Spam_Is_Not_Detected_And_Possible_Spam_Is_Not_Allowed()
        {
            settings.SetupGet(s => s.AllowPossibleSpamStorySubmit).Returns(false);

            var story = new Mock<IStory>();

            _spamProtection.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(false);

            Create(story);

            _eventAggregator.Verify();
            _storySubmitEvent.Verify();
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Url_Is_Restricted()
        {
            _contentService.Setup(cs => cs.IsRestricted(It.IsAny<string>())).Returns(true);

            var result = Create();

            Assert.Equal("Specifed url has match with our banned url list.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Spam_Is_Detected_And_Possible_Spam_Is_Not_Allowed()
        {
            settings.SetupGet(s => s.AllowPossibleSpamStorySubmit).Returns(false);

            var story = new Mock<IStory>();

            _storyRepository.Setup(r => r.CountPostedByUser(It.IsAny<Guid>())).Returns(settings.Object.StorySumittedThresholdOfUserToSpamCheck - 1);
            _spamProtection.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(true);

            var result = Create(story);

            Assert.Equal("Your story appears to be a spam.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Url_Does_Not_Return_Any_Content()
        {
            _categoryRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Returns(new Mock<ICategory>().Object);
            _contentService.Setup(cs => cs.Get(It.IsAny<string>())).Returns(StoryContent.Empty);

            var result = Create();

            Assert.Equal("Specified url appears to be broken.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Category_Does_Not_Exist()
        {
            _categoryRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Returns((ICategory) null);

            var result = Create();

            Assert.Contains("category does not exist.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Url_Already_Exists()
        {
            _storyRepository.Setup(r => r.FindByUrl(It.IsAny<string>())).Returns(new Mock<IStory>().Object);

            var result = Create();

            Assert.Equal("Story with the same url already exists.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Null_User_Is_Passed()
        {
            var result = Create(null, "http://astory.com", "A dummy Story", "dummy", "Dummy description of Story", "192.168.0.1", "A dummy agent");

            Assert.Equal("User cannot be null.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Blank_Url_Is_Passed()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, string.Empty, "A dummy Story", "dummy", "Dummy description of Story", "192.168.0.1", "A dummy agent");

            Assert.Equal("Url cannot be blank.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Invalid_Url_Is_Passed()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, "foo", "A dummy Story", "dummy", "Dummy description of Story", "192.168.0.1", "A dummy agent");

            Assert.Equal("Invalid web url.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Blank_Title_Is_Passed()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, "http://astory.com", string.Empty, "dummy", "Dummy description of Story", "192.168.0.1", "A dummy agent");

            Assert.Equal("Title cannot be blank.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Title_Exceeds_Max_Length()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, "http://astory.com", new string('x', 257), "dummy", "Dummy description of Story", "192.168.0.1", "A dummy agent");

            Assert.Equal("Title cannot be more than 256 character.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Blank_Category_Is_Passed()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, "http://astory.com", "a dummy story", string.Empty, "Dummy description of Story", "192.168.0.1", "A dummy agent");

            Assert.Equal("Category cannot be blank.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Blank_Description_Is_Passed()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, "http://astory.com", "a dummy story", "dummy", string.Empty, "192.168.0.1", "A dummy agent");

            Assert.Equal("Description cannot be blank.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Out_Of_Length_Description_Is_Passed()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, "http://astory.com", "A dummy story", "dummy", new string('x', 2049), "192.168.0.1", "A dummy agent");

            Assert.Equal("Description must be between 8 to 2048 character.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Blank_IpAddress_Is_Passed()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, "http://astory.com", "a dummy story", "dummy", "A dummy description", string.Empty, "A dummy agent");

            Assert.Equal("User Ip address cannot be blank.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Blank_UserAgent_Is_Passed()
        {
            var user = new Mock<IUser>();

            var result = Create(user.Object, "http://astory.com", "a dummy story", "dummy", "A dummy description", "192.168.0.1", string.Empty);

            Assert.Equal("User agent cannot be empty.", result.ErrorMessage);
        }

        [Fact]
        public void Update_Should_Use_The_Story()
        {
            var story = new Mock<IStory>();

            Update(story);

            story.Verify();
        }

        [Fact]
        public void Update_Should_Use_The_CategoryRepository()
        {
            Update(new Mock<IStory>());

            _categoryRepository.Verify();
        }

        [Fact]
        public void Delete_Should_Publish_Story_Delete_Event()
        {
            Delete();

            _eventAggregator.Verify();
            _storyDeleteEvent.Verify();
        }

        [Fact]
        public void Delete_Should_Use_StoryRepository()
        {
            Delete();

            _storyRepository.Verify();
        }

        [Fact]
        public void View_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            View(story);

            story.Verify();
        }

        [Fact]
        public void View_Should_Publish_Story_View_Event()
        {
            View(new Mock<IStory>());

            _eventAggregator.Verify();
            _storyViewEvent.Verify();
        }

        [Fact]
        public void Promote_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            Promote(new Mock<IStory>());

            story.Verify();
        }

        [Fact]
        public void Promote_Should_Publish_Story_Promote_Event()
        {
            Promote(new Mock<IStory>());

            _eventAggregator.Verify();
            _storyPromoteEvent.Verify();
        }

        [Fact]
        public void Demote_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            Demote(story);

            story.Verify();
        }

        [Fact]
        public void Demote_Should_Publish_Story_Demote_Event()
        {
            Demote(new Mock<IStory>());

            _eventAggregator.Verify();
            _storyDemoteEvent.Verify();
        }

        [Fact]
        public void MarkAsSpam_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            MarkAsSpam(story);

            story.Verify();
        }

        [Fact]
        public void MarkAsSpam_Should_Publish_Story_Mark_As_Spam_Event()
        {
            MarkAsSpam(new Mock<IStory>());

            _eventAggregator.Verify();
            _storyMarkAsSpamEvent.Verify();
        }

        [Fact]
        public void UnmarkAsSpam_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            UnmarkAsSpam(story);

            story.Verify();
        }

        [Fact]
        public void UnmarkAsSpam_Should_Publish_Story_Unmark_As_Spam_Event()
        {
            UnmarkAsSpam(new Mock<IStory>());

            _eventAggregator.Verify();
            _storyUnmarkAsSpamEvent.Verify();
        }

        [Fact]
        public void Comment_Should_Return_Null_Error_Message_When_Successfully_Posted()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Setup(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);

            var result = Comment(story.Object, user.Object, true);

            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public void Comment_Should_Use_Story()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Setup(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object).Verifiable();
            Comment(story.Object, user.Object, true);

            story.Verify();
        }

        [Fact]
        public void Comment_Should_Subscribe_Via_Email_When_Passing_True()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Setup(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);
            story.Setup(s => s.SubscribeComment(It.IsAny<IUser>())).Verifiable();

            Comment(story.Object, user.Object, true);

            story.Verify();
        }

        [Fact]
        public void Comment_Should_Unsubscribe_from_Email_When_Passing_False()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Setup(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);
            story.Setup(s => s.UnsubscribeComment(It.IsAny<IUser>())).Verifiable();

            Comment(story.Object, user.Object, false);

            story.Verify();
        }

        [Fact]
        public void Comment_Should_Publish_Comment_Submit_Event_When_Spam_Is_Not_Detected_And_Possible_Spam_Is_Not_Allowed()
        {
            settings.SetupGet(s => s.AllowPossibleSpamCommentSubmit).Returns(false);

            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var subscriber = new Mock<IUser>();

            var comment = new Mock<IComment>();

            _spamProtection.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(false);

            story.Setup(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);
            story.SetupGet(s => s.Subscribers).Returns(new List<IUser>{ subscriber.Object });

            var commentSubmitEvent = new Mock<CommentSubmitEvent>();
            _eventAggregator.Setup(ea => ea.GetEvent<CommentSubmitEvent>()).Returns(commentSubmitEvent.Object).Verifiable();
            commentSubmitEvent.Setup(e => e.Publish(It.IsAny<CommentSubmitEventArgs>())).Verifiable();

            Comment(story.Object, user.Object, true);

            _eventAggregator.Verify();
            commentSubmitEvent.Verify();
        }

        [Fact]
        public void Comment_Should_Use_SpamPostProcessor_When_Possible_Spam_Is_Allowed()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Setup(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);
            _spamProtection.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>(), It.IsAny<Action<string, bool>>())).Callback((SpamCheckContent sp, Action<string, bool> onComplete) => onComplete("foo", true));
            _spamPostProcessor.Setup(sp => sp.Process(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IComment>())).Verifiable();

            Comment(story.Object, user.Object, true);

            _spamPostProcessor.Verify();
        }

        [Fact]
        public void Comment_Should_Return_Error_Message_When_Spam_Is_Detected_And_Possible_Spam_Is_Not_Allowed()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();

            settings.SetupGet(s => s.AllowPossibleSpamCommentSubmit).Returns(false);
            _spamProtection.Setup(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(true);

            var result = Comment(story.Object, user.Object, true);

            Assert.Equal("Your comment appears to be a spam.", result.ErrorMessage);
        }

        [Fact]
        public void Comment_Should_Return_Error_Message_When_Null_Story_Is_Passed()
        {
            var result = Comment(null, new Mock<IUser>().Object, false);

            Assert.Equal("Story cannot be null.", result.ErrorMessage);
        }

        [Fact]
        public void Comment_Should_Return_Error_Message_When_Null_User_Is_Passed()
        {
            var result = Comment(new Mock<IStory>().Object, null, false);

            Assert.Equal("User cannot be null.", result.ErrorMessage);
        }

        [Fact]
        public void Comment_Should_Return_Error_Message_When_Blank_Content_Is_Passed()
        {
            var result = Comment(new Mock<IStory>().Object, new Mock<IUser>().Object, string.Empty, false, null, null);

            Assert.Equal("Comment cannot be blank.", result.ErrorMessage);
        }

        [Fact]
        public void Comment_Should_Return_Error_Message_When_Blank_Content_Exceeds_Maximum_Length()
        {
            var result = Comment(new Mock<IStory>().Object, new Mock<IUser>().Object, new string('x', 2049), false, null, null);

            Assert.Equal("Comment cannot be more than 2048 character.", result.ErrorMessage);
        }

        [Fact]
        public void Comment_Should_Return_Error_Message_When_Blank_IpAddress_Is_Passed()
        {
            var result = Comment(new Mock<IStory>().Object, new Mock<IUser>().Object, new string('x', 2048), false, string.Empty, null);

            Assert.Equal("User ip address cannot be blank.", result.ErrorMessage);
        }

        [Fact]
        public void Comment_Should_Return_Error_Message_When_Blank_UserAgent_Is_Passed()
        {
            var result = Comment(new Mock<IStory>().Object, new Mock<IUser>().Object, new string('x', 2048), false, "192.168.0.1", null);

            Assert.Equal("User agent cannot be empty.", result.ErrorMessage);
        }

        [Fact]
        public void Spam_Should_Use_StoryRepository_For_Story()
        {
            SpamStory(new Mock<IStory>());

            _storyRepository.Verify();
        }

        [Fact]
        public void Spam_Should_Publish_Story_Spam_Event()
        {
            SpamStory(new Mock<IStory>());

            _eventAggregator.Verify();
            _storySpamEvent.Verify();
        }

        [Fact]
        public void Spam_Should_Use_Story_For_Comment()
        {
            var story = new Mock<IStory>();

            SpamComment(story);

            story.Verify();
        }

        [Fact]
        public void Spam_Should_Publish_Comment_Spam_Event()
        {
            SpamComment(new Mock<IStory>());

            _eventAggregator.Verify();
            _commentSpamEvent.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Use_Comment()
        {
            var comment = new Mock<IComment>();

            MarkAsOffended(comment);

            comment.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Publish_Comment_Mark_As_Offended_Event()
        {
            MarkAsOffended(new Mock<IComment>());

            _eventAggregator.Verify();
            _commentMarkAsOffendedEvent.Verify();
        }

        [Fact]
        public void Publish_Should_Use_StoryRepository()
        {
            Publish();

            _storyRepository.Verify();
        }

        [Fact]
        public void Publish_Should_Use_Weight_Calculators()
        {
            Publish();

            _voteStrategy.Verify();
            _commentStrategy.Verify();
            _viewStrategy.Verify();
            _userScoreStrategy.Verify();
            _freshnessStrategy.Verify();
            _knownSourceStrategy.Verify();
        }

        [Fact]
        public void Publish_Should_Use_MarkAsSpamRepository()
        {
            Publish();

            _markAsSpamRepository.Verify();
        }

        [Fact]
        public void Publish_Should_Publish_Story_Publish_Event()
        {
            Publish();

            _eventAggregator.Verify();
            _storyPublishEvent.Verify();
        }

        [Fact]
        public void Publish_Should_Publish_Story_Incorrectly_Marked_As_Spam_Event()
        {
            Publish();

            _eventAggregator.Verify();
            _storyIncorrectlyMarkedAsSpamEvent.Verify();
        }

        [Fact]
        public void Approve_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            Approve(story);

            story.Verify();
        }

        [Fact]
        public void Approve_Should_Publish_Story_AApprove_Event()
        {
            Approve(new Mock<IStory>());

            _eventAggregator.Verify();
            _storyApproveEvent.Verify();
        }

        private StoryCreateResult Create(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.SetupGet(s => s.Url).Returns("http://astory.com");

            return Create(story, user);
        }

        private StoryCreateResult Create(Mock<IStory> story, Mock<IUser> user)
        {
            _storyRepository.Setup(r => r.FindByUrl(It.IsAny<string>())).Returns((IStory)null);
            _categoryRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Returns(new Mock<ICategory>().Object);
            _contentService.Setup(s => s.Get(It.IsAny<string>())).Returns(new StoryContent("A dummy story", "Dummy description", "http://trackbackurl.com"));
            _factory.Setup(f => f.CreateStory(It.IsAny<ICategory>(), It.IsAny<IUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(story.Object);

            _storySubmitEvent = new Mock<StorySubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySubmitEvent>()).Returns(_storySubmitEvent.Object).Verifiable();
            _storySubmitEvent.Setup(e => e.Publish(It.IsAny<StorySubmitEventArgs>())).Verifiable();

            return Create(user.Object, "http://astory.com", "A dummy Story", "dummy", "Dummy description of Story", "192.168.0.1", "A dummy agent");
        }

        private StoryCreateResult Create()
        {
            var user = new Mock<IUser>();

            return Create(user.Object, "http://astory.com", "A dummy Story", "dummy", "Dummy description of Story", "192.168.0.1", "A dummy agent");
        }

        private StoryCreateResult Create(IUser user, string url, string title, string category, string desriptions, string ipAddress, string userAgent)
        {
            return _storyService.Create(user, url, title, category, desriptions, "dummy, C#", ipAddress, userAgent, null, new NameValueCollection { { "foo", "bar" } }, s => "http://dotnetshoutout.com/A-Dummy-Story");
        }

        private void Update(Mock<IStory> story)
        {
            var category = new Mock<ICategory>();
            category.Setup(c => c.UniqueName).Returns("Dummy");

            story.SetupGet(s => s.BelongsTo).Returns(category.Object);

            story.Setup(s => s.ChangeNameAndCreatedAt(It.IsAny<string>(), It.IsAny<DateTime>())).Verifiable();
            story.SetupSet(s => s.Title = "This is a title").Verifiable();
            story.Setup(s => s.ChangeCategory(It.IsAny<ICategory>())).Verifiable();
            story.SetupSet(s => s.HtmlDescription = "This is the description").Verifiable();
            story.Setup(s => s.AddTag(It.IsAny<ITag>())).Verifiable();

            var updatedCategory = new Mock<ICategory>();
            _categoryRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Returns(updatedCategory.Object).Verifiable();

            _storyService.Update(story.Object, string.Empty, DateTime.MinValue, "This is a title", "foobar", "This is the description", "foo,bar");
        }

        private void Delete()
        {
            _storyRepository.Setup(r => r.Remove(It.IsAny<IStory>())).Verifiable();

            _storyDeleteEvent = new Mock<StoryDeleteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryDeleteEvent>()).Returns(_storyDeleteEvent.Object).Verifiable();
            _storyDeleteEvent.Setup(e => e.Publish(It.IsAny<StoryDeleteEventArgs>())).Verifiable();


            _storyService.Delete(new Mock<IStory>().Object, new Mock<IUser>().Object);
        }

        private void View(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Setup(s => s.View(It.IsAny<DateTime>(), It.IsAny<string>())).Verifiable();

            _storyViewEvent = new Mock<StoryViewEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryViewEvent>()).Returns(_storyViewEvent.Object).Verifiable();
            _storyViewEvent.Setup(e => e.Publish(It.IsAny<StoryViewEventArgs>())).Verifiable();

            _storyService.View(story.Object, user.Object, "192.168.0.1");
        }

        private void Promote(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Setup(s => s.Promote(It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(true).Verifiable();

            _storyPromoteEvent = new Mock<StoryPromoteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryPromoteEvent>()).Returns(_storyPromoteEvent.Object).Verifiable();
            _storyPromoteEvent.Setup(e => e.Publish(It.IsAny<StoryPromoteEventArgs>())).Verifiable();

            _storyService.Promote(story.Object, user.Object, "192.168.0.1");
        }

        private void Demote(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Setup(s => s.Demote(It.IsAny<DateTime>(), It.IsAny<IUser>())).Returns(true).Verifiable();

            _storyDemoteEvent = new Mock<StoryDemoteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryDemoteEvent>()).Returns(_storyDemoteEvent.Object).Verifiable();
            _storyDemoteEvent.Setup(e => e.Publish(It.IsAny<StoryDemoteEventArgs>())).Verifiable();

            _storyService.Demote(story.Object, user.Object);
        }

        private void MarkAsSpam(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Setup(s => s.MarkAsSpam(It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(true).Verifiable();

            _storyMarkAsSpamEvent = new Mock<StoryMarkAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryMarkAsSpamEvent>()).Returns(_storyMarkAsSpamEvent.Object).Verifiable();
            _storyMarkAsSpamEvent.Setup(e => e.Publish(It.IsAny<StoryMarkAsSpamEventArgs>())).Verifiable();

            _storyService.MarkAsSpam(story.Object, "http://dotnetshoutout.com/A-Dummy-Story", user.Object, "192.168.0.1");
        }

        private void UnmarkAsSpam(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Setup(s => s.UnmarkAsSpam(It.IsAny<DateTime>(), It.IsAny<IUser>())).Returns(true).Verifiable();

            _storyUnmarkAsSpamEvent = new Mock<StoryUnmarkAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryUnmarkAsSpamEvent>()).Returns(_storyUnmarkAsSpamEvent.Object).Verifiable();
            _storyUnmarkAsSpamEvent.Setup(e => e.Publish(It.IsAny<StoryUnmarkAsSpamEventArgs>())).Verifiable();

            _storyService.UnmarkAsSpam(story.Object, user.Object);
        }

        private CommentCreateResult Comment(IStory story, IUser user, bool subscribe)
        {
            return Comment(story, user, "This is a dummy comment", subscribe, "192.168.0.1", "A dummy browser");
        }

        private CommentCreateResult Comment(IStory story, IUser user, string content, bool subscribe, string ipAddress, string userAgent)
        {
            return _storyService.Comment(story, "http://dotnetshoutout.com/A-Dummy-Story", user, content, subscribe, ipAddress, userAgent, null, new NameValueCollection { { "foo", "bar" } });
        }

        private void SpamStory(Mock<IStory> story)
        {
            var postedBy = new Mock<IUser>();

            story.SetupGet(s => s.PostedBy).Returns(postedBy.Object);

            _storyRepository.Setup(r => r.Remove(It.IsAny<IStory>())).Verifiable();

            _storySpamEvent = new Mock<StorySpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySpamEvent>()).Returns(_storySpamEvent.Object).Verifiable();
            _storySpamEvent.Setup(e => e.Publish(It.IsAny<StorySpamEventArgs>())).Verifiable();

            _storyService.Spam(story.Object, "http://dotnetshoutout.com/Dummy-Story", new Mock<IUser>().Object);
        }

        private void SpamComment(Mock<IStory> story)
        {
            var postedBy = new Mock<IUser>();

            var comment = new Mock<IComment>();
            comment.SetupGet(c => c.ByUser).Returns(postedBy.Object);
            comment.SetupGet(c => c.ForStory).Returns(story.Object);

            story.Setup(s => s.DeleteComment(It.IsAny<IComment>())).Verifiable();

            _commentSpamEvent = new Mock<CommentSpamEvent>();
            _eventAggregator.Setup(ea => ea.GetEvent<CommentSpamEvent>()).Returns(_commentSpamEvent.Object).Verifiable();
            _commentSpamEvent.Setup(e => e.Publish(It.IsAny<CommentSpamEventArgs>())).Verifiable();

            _storyService.Spam(comment.Object, "http://dotnetshoutout.com/Dummy-Story", new Mock<IUser>().Object);
        }

        private void MarkAsOffended(Mock<IComment> comment)
        {
            var postedBy = new Mock<IUser>();

            comment.SetupGet(c => c.ByUser).Returns(postedBy.Object);

            comment.Setup(c => c.MarkAsOffended()).Verifiable();

            _commentMarkAsOffendedEvent = new Mock<CommentMarkAsOffendedEvent>();
            _eventAggregator.Setup(ea => ea.GetEvent<CommentMarkAsOffendedEvent>()).Returns(_commentMarkAsOffendedEvent.Object).Verifiable();
            _commentMarkAsOffendedEvent.Setup(e => e.Publish(It.IsAny<CommentMarkAsOffendedEventArgs>())).Verifiable();

            _storyService.MarkAsOffended(comment.Object, "http://dotnetshoutout.com", new Mock<IUser>().Object);
        }

        private void Publish()
        {
            const int PublishableCount = 40;
            var rnd = new Random();

            var stories = new List<IStory>();

            for (var i = 1; i <= PublishableCount; i++ )
            {
                stories.Add(new Mock<IStory>().Object);
            }

            _storyRepository.Setup(r => r.CountByPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(PublishableCount).Verifiable();
            _storyRepository.Setup(r => r.FindPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>(stories, PublishableCount)).Verifiable();

            _voteStrategy.Setup(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(2, 100)).Verifiable();
            _commentStrategy.Setup(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(0, 20)).Verifiable();
            _viewStrategy.Setup(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.NextDouble()).Verifiable();
            _userScoreStrategy.Setup(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(1, 300)).Verifiable();
            _freshnessStrategy.Setup(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(0, 3)).Verifiable();
            _knownSourceStrategy.Setup(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(0, 5)).Verifiable();

            var markAsSpam = new Mock<IMarkAsSpam>();

            markAsSpam.Setup(m => m.ByUser).Returns(new Mock<IUser>().Object);
            _markAsSpamRepository.Setup(s => s.FindAfter(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(new[] { markAsSpam.Object }).Verifiable();

            _storyPublishEvent = new Mock<StoryPublishEvent>();
            _eventAggregator.Setup(ea => ea.GetEvent<StoryPublishEvent>()).Returns(_storyPublishEvent.Object).Verifiable();
            _storyPublishEvent.Setup(e => e.Publish(It.IsAny<StoryPublishEventArgs>())).Verifiable();

            _storyIncorrectlyMarkedAsSpamEvent = new Mock<StoryIncorrectlyMarkedAsSpamEvent>();
            _eventAggregator.Setup(ea => ea.GetEvent<StoryIncorrectlyMarkedAsSpamEvent>()).Returns(_storyIncorrectlyMarkedAsSpamEvent.Object).Verifiable();
            _storyIncorrectlyMarkedAsSpamEvent.Setup(e => e.Publish(It.IsAny<StoryIncorrectlyMarkedAsSpamEventArgs>())).Verifiable();

            _storyService.Publish();
        }

        private void Approve(Mock<IStory> story)
        {
            var postedBy = new Mock<IUser>();

            story.SetupGet(s => s.PostedBy).Returns(postedBy.Object);

            story.Setup(s => s.Approve(It.IsAny<DateTime>())).Verifiable();

            _storyApproveEvent = new Mock<StoryApproveEvent>();
            _eventAggregator.Setup(ea => ea.GetEvent<StoryApproveEvent>()).Returns(_storyApproveEvent.Object).Verifiable();
            _storyApproveEvent.Setup(e => e.Publish(It.IsAny<StoryApproveEventArgs>())).Verifiable();

            var byUser = new Mock<IUser>();

            _storyService.Approve(story.Object, "http://dotnetshoutout.com/Dummy-Story", byUser.Object);
        }
    }
}