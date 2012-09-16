using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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
        private readonly Mock<IUserScoreService> _userScoreService;
        private readonly Mock<IDomainObjectFactory> _factory;
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<ITagRepository> _tagRepository;
        private readonly Mock<IStoryRepository> _storyRepository;
        private readonly Mock<IMarkAsSpamRepository> _markAsSpamRepository;
        private readonly Mock<ISpamProtection> _spamProtection;
        private readonly Mock<ISpamPostprocessor> _spamPostProcessor;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<IContentService> _contentService;
        private readonly Mock<IHtmlSanitizer> _htmlSanitizer;

        private readonly Mock<IStoryWeightCalculator> _voteStrategy;
        private readonly Mock<IStoryWeightCalculator> _commentStrategy;
        private readonly Mock<IStoryWeightCalculator> _viewStrategy;
        private readonly Mock<IStoryWeightCalculator> _userScoreStrategy;
        private readonly Mock<IStoryWeightCalculator> _freshnessStrategy;
        private readonly Mock<IStoryWeightCalculator> _knownSourceStrategy;

        private readonly StoryService _storyService;

        public StoryServiceFixture()
        {
            _userScoreService = new Mock<IUserScoreService>();
            _factory = new Mock<IDomainObjectFactory>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _tagRepository = new Mock<ITagRepository>();
            _storyRepository = new Mock<IStoryRepository>();
            _markAsSpamRepository = new Mock<IMarkAsSpamRepository>();
            _spamProtection = new Mock<ISpamProtection>();
            _spamPostProcessor = new Mock<ISpamPostprocessor>();
            _emailSender = new Mock<IEmailSender>();
            _contentService = new Mock<IContentService>();
            _htmlSanitizer = new Mock<IHtmlSanitizer>();

            _voteStrategy = new Mock<IStoryWeightCalculator>();
            _voteStrategy.ExpectGet(s => s.Name).Returns("Vote");

            _commentStrategy = new Mock<IStoryWeightCalculator>();
            _commentStrategy.ExpectGet(s => s.Name).Returns("Comment");

            _viewStrategy = new Mock<IStoryWeightCalculator>();
            _viewStrategy.ExpectGet(s => s.Name).Returns("View");

            _userScoreStrategy = new Mock<IStoryWeightCalculator>();
            _userScoreStrategy.ExpectGet(s => s.Name).Returns("User-Score");

            _knownSourceStrategy = new Mock<IStoryWeightCalculator>();
            _knownSourceStrategy.ExpectGet(s => s.Name).Returns("Known-Source");

            _freshnessStrategy = new Mock<IStoryWeightCalculator>();
            _freshnessStrategy.ExpectGet(s => s.Name).Returns("Freshness");

            _storyService = new StoryService(settings.Object, _userScoreService.Object, _factory.Object, _categoryRepository.Object, _tagRepository.Object, _storyRepository.Object, _markAsSpamRepository.Object, _spamProtection.Object, _spamPostProcessor.Object, _emailSender.Object, _contentService.Object, _htmlSanitizer.Object, thumbnail.Object, new []{ _voteStrategy.Object, _commentStrategy.Object, _viewStrategy.Object, _userScoreStrategy.Object, _knownSourceStrategy.Object, _freshnessStrategy.Object });
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

            story.Expect(s => s.Promote(It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(true);

            Create(story);

            story.Verify();
        }

        [Fact]
        public void Create_Should_Request_Thumnbnail_To_Capture()
        {
            var story = new Mock<IStory>();

            thumbnail.Expect(t => t.Capture(It.IsAny<string>())).Verifiable();

            Create(story);

            thumbnail.Verify();
        }

        [Fact]
        public void Create_Should_Subscribe_User_For_New_Comments()
        {
            var story = new Mock<IStory>();

            story.Expect(s => s.SubscribeComment(It.IsAny<IUser>())).Verifiable();

            Create(story);

            story.Verify();
        }

        [Fact]
        public void Create_Should_Add_Tag_Using_Tag_Repository()
        {
            var story = new Mock<IStory>();

            _tagRepository.Expect(r => r.FindByName(It.IsAny<string>())).Returns((ITag) null);
            _tagRepository.Expect(r => r.Add(It.IsAny<ITag>())).Verifiable();

            Create(story);

            _tagRepository.Verify();
        }

        [Fact]
        public void Create_Should_Add_Tag_To_Story()
        {
            var story = new Mock<IStory>();
            story.Expect(s => s.AddTag(It.IsAny<ITag>())).Verifiable();

            Create(story);

            story.Verify();
        }

        [Fact]
        public void Create_Should_Add_Tag_To_User()
        {
            var user = new Mock<IUser>();
            var story = new Mock<IStory>();

            user.Expect(s => s.AddTag(It.IsAny<ITag>())).Verifiable();

            Create(story, user);

            user.Verify();
        }

        [Fact]
        public void Create_Should_Notify_User_Score_Service()
        {
            var story = new Mock<IStory>();

            _userScoreService.Expect(us => us.StorySubmitted(It.IsAny<IUser>())).Verifiable();

            Create(story);

            _userScoreService.Verify();
        }

        [Fact]
        public void Create_Should_Ping_Story()
        {
            var story = new Mock<IStory>();

            _contentService.Expect(cs => cs.Ping(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            Create(story);

            _contentService.Verify();
        }

        [Fact]
        public void Create_Should_Not_CheckForSpam_When_Possible_Spam_Is_Not_Allowed_And_User_CanModerate()
        {
            settings.ExpectGet(s => s.AllowPossibleSpamStorySubmit).Returns(false);

            var user = new Mock<IUser>();

            user.ExpectGet(u => u.Role).Returns(Roles.Moderator);

            var story = new Mock<IStory>();

            _storyRepository.Expect(r => r.CountPostedByUser(It.IsAny<Guid>())).Never();

            Create(story, user);

            _storyRepository.Verify();
        }

        [Fact]
        public void Create_Should_Not_CheckForSpam_When_Possible_Spam_Is_Allowed_And_User_CanModerate()
        {
            var user = new Mock<IUser>();

            user.ExpectGet(u => u.Role).Returns(Roles.Moderator);

            var story = new Mock<IStory>();

            _storyRepository.Expect(r => r.CountPostedByUser(It.IsAny<Guid>())).Never();

            Create(story, user);

            _storyRepository.Verify();
        }

        [Fact]
        public void Create_Should_Use_SpamPostProcesor_When_Spam_Is_Detected_And_Possible_Spam_Is_Allowed()
        {
            var story = new Mock<IStory>();

            _storyRepository.Expect(r => r.CountPostedByUser(It.IsAny<Guid>())).Returns(settings.Object.StorySumittedThresholdOfUserToSpamCheck -1);
            _spamProtection.Expect(sp => sp.IsSpam(It.IsAny<SpamCheckContent>(), It.IsAny<Action<string, bool>>())).Callback((SpamCheckContent sp, Action<string, bool> onComplete) => onComplete("foo", true));
            _spamPostProcessor.Expect(sp => sp.Process(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IStory>())).Verifiable();

            Create(story);

            _spamPostProcessor.Verify();
        }

        [Fact]
        public void Create_Should_Approve_Story_When_Spam_Is_Not_Detected_And_Possible_Spam_Is_Not_Allowed()
        {
            settings.ExpectGet(s => s.AllowPossibleSpamStorySubmit).Returns(false);

            var story = new Mock<IStory>();

            _spamProtection.Expect(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(false);
            story.Expect(s => s.Approve(It.IsAny<DateTime>())).Verifiable();

            Create(story);

            story.Verify();
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Spam_Is_Detected_And_Possible_Spam_Is_Not_Allowed()
        {
            settings.ExpectGet(s => s.AllowPossibleSpamStorySubmit).Returns(false);

            var story = new Mock<IStory>();

            _storyRepository.Expect(r => r.CountPostedByUser(It.IsAny<Guid>())).Returns(settings.Object.StorySumittedThresholdOfUserToSpamCheck - 1);
            _spamProtection.Expect(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(true);

            var result = Create(story);

            Assert.Equal("Your story appears to be a spam.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Url_Does_Not_Return_Any_Content()
        {
            _categoryRepository.Expect(r => r.FindByUniqueName(It.IsAny<string>())).Returns(new Mock<ICategory>().Object);
            _contentService.Expect(cs => cs.Get(It.IsAny<string>())).Returns(StoryContent.Empty);

            var result = Create();

            Assert.Equal("Specified url appears to be broken.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Category_Does_Not_Exist()
        {
            _categoryRepository.Expect(r => r.FindByUniqueName(It.IsAny<string>())).Returns((ICategory) null);

            var result = Create();

            Assert.Contains("category does not exist.", result.ErrorMessage);
        }

        [Fact]
        public void Create_Should_Return_Error_Message_When_Url_Already_Exists()
        {
            _storyRepository.Expect(r => r.FindByUrl(It.IsAny<string>())).Returns(new Mock<IStory>().Object);

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
        public void Delete_Should_Use_UserScoreService()
        {
            Delete();

            _userScoreService.Verify();
        }

        [Fact]
        public void Delete_Should_Use_StoryRepository()
        {
            Delete();

            _storyRepository.Verify();
        }

        [Fact]
        public void Delete_Should_Use_EmailSender()
        {
            Delete();

            _emailSender.Verify();
        }

        [Fact]
        public void View_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            View(story);

            story.Verify();
        }

        [Fact]
        public void View_Should_Use_UserScoreService()
        {
            View(new Mock<IStory>());

            _userScoreService.Verify();
        }

        [Fact]
        public void Promote_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            Promote(new Mock<IStory>());

            story.Verify();
        }

        [Fact]
        public void Promote_Should_Use_UserScoreService()
        {
            Promote(new Mock<IStory>());

            _userScoreService.Verify();
        }

        [Fact]
        public void Demote_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            Demote(story);

            story.Verify();
        }

        [Fact]
        public void Demote_Should_Use_UserScoreService()
        {
            Demote(new Mock<IStory>());

            _userScoreService.Verify();
        }

        [Fact]
        public void MarkAsSpam_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            MarkAsSpam(story);

            story.Verify();
        }

        [Fact]
        public void MarkAsSpam_Should_Use_UserScoreService()
        {
            MarkAsSpam(new Mock<IStory>());

            _userScoreService.Verify();
        }

        [Fact]
        public void MarkAsSpam_Should_Use_EmailSender()
        {
            MarkAsSpam(new Mock<IStory>());

            _emailSender.Verify();
        }

        [Fact]
        public void UnmarkAsSpam_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            UnmarkAsSpam(story);

            story.Verify();
        }

        [Fact]
        public void UnmarkAsSpam_Should_Use_UserScoreService()
        {
            UnmarkAsSpam(new Mock<IStory>());

            _userScoreService.Verify();
        }

        [Fact]
        public void Comment_Should_Return_Null_Error_Message_When_Successfully_Posted()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Expect(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);

            var result = Comment(story.Object, user.Object, true);

            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public void Comment_Should_Use_UserScoreService_And_Story()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Expect(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object).Verifiable();
            _userScoreService.Expect(us => us.StoryCommented(It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();
            Comment(story.Object, user.Object, true);

            story.Verify();
            _userScoreService.Verify();
        }

        [Fact]
        public void Comment_Should_Subscribe_Via_Email_When_Passing_True()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Expect(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);
            story.Expect(s => s.SubscribeComment(It.IsAny<IUser>())).Verifiable();

            Comment(story.Object, user.Object, true);

            story.Verify();
        }

        [Fact]
        public void Comment_Should_Unsubscribe_from_Email_When_Passing_false()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Expect(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);
            story.Expect(s => s.UnsubscribeComment(It.IsAny<IUser>())).Verifiable();

            Comment(story.Object, user.Object, false);

            story.Verify();
        }

        [Fact]
        public void Comment_Should_Send_Mail_To_All_Subcribers()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var subscriber = new Mock<IUser>();

            var comment = new Mock<IComment>();

            story.Expect(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);
            story.ExpectGet(s => s.Subscribers).Returns(new List<IUser>{ subscriber.Object });
            _emailSender.Expect(es => es.SendComment(It.IsAny<string>(), It.IsAny<IComment>(), It.IsAny<IEnumerable<IUser>>())).Verifiable();

            Comment(story.Object, user.Object, true);

            _emailSender.Verify();
        }

        [Fact]
        public void Comment_Should_Use_SpamPostProcessor_When_Possible_Spam_Is_Allowed()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var comment = new Mock<IComment>();

            story.Expect(s => s.PostComment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(comment.Object);
            _spamProtection.Expect(sp => sp.IsSpam(It.IsAny<SpamCheckContent>(), It.IsAny<Action<string, bool>>())).Callback((SpamCheckContent sp, Action<string, bool> onComplete) => onComplete("foo", true));
            _spamPostProcessor.Expect(sp => sp.Process(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IComment>())).Verifiable();

            Comment(story.Object, user.Object, true);

            _spamPostProcessor.Verify();
        }

        [Fact]
        public void Comment_Should_Return_Error_Message_When_Spam_Is_Detected_And_Possible_Spam_Is_Not_Allowed()
        {
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();

            settings.ExpectGet(s => s.AllowPossibleSpamCommentSubmit).Returns(false);
            _spamProtection.Expect(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(true);

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
        public void Spam_Should_Use_UserScoreService_For_Story()
        {
            SpamStory(new Mock<IStory>());

            _userScoreService.Verify();
        }

        [Fact]
        public void Spam_Should_Use_EmailSender_For_Story()
        {
            SpamStory(new Mock<IStory>());

            _emailSender.Verify();
        }

        [Fact]
        public void Spam_Should_Use_Story_For_Comment()
        {
            var story = new Mock<IStory>();

            SpamComment(story);

            story.Verify();
        }

        [Fact]
        public void Spam_Should_Use_UserScoreService_For_Comment()
        {
            SpamComment(new Mock<IStory>());

            _userScoreService.Verify();
        }

        [Fact]
        public void Spam_Should_Use_EmailSender_For_Comment()
        {
            SpamComment(new Mock<IStory>());

            _emailSender.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Use_Comment()
        {
            var comment = new Mock<IComment>();

            MarkAsOffended(comment);

            comment.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Use_UserScoreService()
        {
            MarkAsOffended(new Mock<IComment>());

            _userScoreService.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Use_EmailSender()
        {
            MarkAsOffended(new Mock<IComment>());

            _emailSender.Verify();
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
        public void Publish_Should_Use_UserScoreService()
        {
            Publish();

            _userScoreService.Verify();
        }

        [Fact]
        public void Publish_Should_Use_EmailSender()
        {
            Publish();

            _emailSender.Verify();
        }

        [Fact]
        public void Approve_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            Approve(story);

            story.Verify();
        }

        [Fact]
        public void Approve_Should_EmailSender()
        {
            Approve(new Mock<IStory>());

            _emailSender.Verify();
        }

        private StoryCreateResult Create(IMock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.ExpectGet(s => s.Url).Returns("http://astory.com");

            return Create(story, user);
        }

        private StoryCreateResult Create(IMock<IStory> story, IMock<IUser> user)
        {
            _storyRepository.Expect(r => r.FindByUrl(It.IsAny<string>())).Returns((IStory)null);
            _categoryRepository.Expect(r => r.FindByUniqueName(It.IsAny<string>())).Returns(new Mock<ICategory>().Object);
            _contentService.Expect(s => s.Get(It.IsAny<string>())).Returns(new StoryContent("A dummy story", "Dummy description", "http://trackbackurl.com"));
            _factory.Expect(f => f.CreateStory(It.IsAny<ICategory>(), It.IsAny<IUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(story.Object);

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
            category.Expect(c => c.UniqueName).Returns("Dummy");

            story.ExpectGet(s => s.BelongsTo).Returns(category.Object);

            story.Expect(s => s.ChangeNameAndCreatedAt(It.IsAny<string>(), It.IsAny<DateTime>())).Verifiable();
            story.ExpectSet(s => s.Title).Verifiable();
            story.Expect(s => s.ChangeCategory(It.IsAny<ICategory>())).Verifiable();
            story.ExpectSet(s => s.HtmlDescription).Verifiable();
            story.Expect(s => s.AddTag(It.IsAny<ITag>())).Verifiable();

            var updatedCategory = new Mock<ICategory>();
            _categoryRepository.Expect(r => r.FindByUniqueName(It.IsAny<string>())).Returns(updatedCategory.Object).Verifiable();

            _storyService.Update(story.Object, string.Empty, DateTime.MinValue, "This is a title", "foobar", "This is the description", "foo,bar");
        }

        private void Delete()
        {
            _userScoreService.Expect(us => us.StoryDeleted(It.IsAny<IStory>())).Verifiable();
            _storyRepository.Expect(r => r.Remove(It.IsAny<IStory>())).Verifiable();
            _emailSender.Expect(es => es.NotifyStoryDelete(It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _storyService.Delete(new Mock<IStory>().Object, new Mock<IUser>().Object);
        }

        private void View(IMock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Expect(s => s.View(It.IsAny<DateTime>(), It.IsAny<string>())).Verifiable();
            _userScoreService.Expect(us => us.StoryViewed(It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _storyService.View(story.Object, user.Object, "192.168.0.1");
        }

        private void Promote(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Expect(s => s.Promote(It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(true).Verifiable();
            _userScoreService.Expect(us => us.StoryPromoted(It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _storyService.Promote(story.Object, user.Object, "192.168.0.1");
        }

        private void Demote(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Expect(s => s.Demote(It.IsAny<DateTime>(), It.IsAny<IUser>())).Returns(true).Verifiable();
            _userScoreService.Expect(us => us.StoryDemoted(It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _storyService.Demote(story.Object, user.Object);
        }

        private void MarkAsSpam(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Expect(s => s.MarkAsSpam(It.IsAny<DateTime>(), It.IsAny<IUser>(), It.IsAny<string>())).Returns(true).Verifiable();
            _userScoreService.Expect(us => us.StoryMarkedAsSpam(It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();
            _emailSender.Expect(es => es.NotifyStoryMarkedAsSpam(It.IsAny<string>(), It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _storyService.MarkAsSpam(story.Object, "http://dotnetshoutout.com/A-Dummy-Story", user.Object, "192.168.0.1");
        }

        private void UnmarkAsSpam(Mock<IStory> story)
        {
            var user = new Mock<IUser>();

            story.Expect(s => s.UnmarkAsSpam(It.IsAny<DateTime>(), It.IsAny<IUser>())).Returns(true).Verifiable();
            _userScoreService.Expect(us => us.StoryUnmarkedAsSpam(It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

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

            story.ExpectGet(s => s.PostedBy).Returns(postedBy.Object);

            _storyRepository.Expect(r => r.Remove(It.IsAny<IStory>())).Verifiable();
            _userScoreService.Expect(us => us.StorySpammed(It.IsAny<IStory>())).Verifiable();
            _emailSender.Expect(es => es.NotifyConfirmSpamStory(It.IsAny<string>(), It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            var byUser = new Mock<IUser>();

            _storyService.Spam(story.Object, "http://dotnetshoutout.com/Dummy-Story", byUser.Object);
        }

        private void SpamComment(IMock<IStory> story)
        {
            var postedBy = new Mock<IUser>();

            var comment = new Mock<IComment>();
            comment.ExpectGet(c => c.ByUser).Returns(postedBy.Object);
            comment.ExpectGet(c => c.ForStory).Returns(story.Object);

            story.Expect(s => s.DeleteComment(It.IsAny<IComment>())).Verifiable();
            _userScoreService.Expect(us => us.CommentSpammed(It.IsAny<IUser>())).Verifiable();
            _emailSender.Expect(es => es.NotifyConfirmSpamComment(It.IsAny<string>(), It.IsAny<IComment>(), It.IsAny<IUser>())).Verifiable();

            var byUser = new Mock<IUser>();

            _storyService.Spam(comment.Object, "http://dotnetshoutout.com/Dummy-Story", byUser.Object);
        }

        private void MarkAsOffended(Mock<IComment> comment)
        {
            var postedBy = new Mock<IUser>();

            comment.ExpectGet(c => c.ByUser).Returns(postedBy.Object);

            comment.Expect(c => c.MarkAsOffended()).Verifiable();
            _userScoreService.Expect(us => us.CommentMarkedAsOffended(It.IsAny<IUser>())).Verifiable();
            _emailSender.Expect(es => es.NotifyCommentAsOffended(It.IsAny<string>(), It.IsAny<IComment>(), It.IsAny<IUser>())).Verifiable();

            var byUser = new Mock<IUser>();

            _storyService.MarkAsOffended(comment.Object, "http://dotnetshoutout.com", byUser.Object);
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

            _storyRepository.Expect(r => r.CountByPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(PublishableCount).Verifiable();
            _storyRepository.Expect(r => r.FindPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>(stories, PublishableCount)).Verifiable();

            _voteStrategy.Expect(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(2, 100)).Verifiable();
            _commentStrategy.Expect(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(0, 20)).Verifiable();
            _viewStrategy.Expect(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.NextDouble()).Verifiable();
            _userScoreStrategy.Expect(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(1, 300)).Verifiable();
            _freshnessStrategy.Expect(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(0, 3)).Verifiable();
            _knownSourceStrategy.Expect(s => s.Calculate(It.IsAny<DateTime>(), It.IsAny<IStory>())).Returns(rnd.Next(0, 5)).Verifiable();

            var markAsSpam = new Mock<IMarkAsSpam>();

            markAsSpam.Expect(m => m.ByUser).Returns(new Mock<IUser>().Object);
            _markAsSpamRepository.Expect(s => s.FindAfter(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(new[] { markAsSpam.Object }).Verifiable();

            _userScoreService.Expect(s => s.StoryIncorrectlyMarkedAsSpam(It.IsAny<IUser>())).Verifiable();
            _userScoreService.Expect(s => s.StoryPublished(It.IsAny<IStory>())).Verifiable();

            _emailSender.Expect(es => es.NotifyPublishedStories(It.IsAny<DateTime>(), It.IsAny<IEnumerable<PublishedStory>>())).Verifiable();

            _storyService.Publish();
        }

        private void Approve(Mock<IStory> story)
        {
            var postedBy = new Mock<IUser>();

            story.ExpectGet(s => s.PostedBy).Returns(postedBy.Object);

            story.Expect(s => s.Approve(It.IsAny<DateTime>())).Verifiable();
            _emailSender.Expect(es => es.NotifyStoryApprove(It.IsAny<string>(), It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            var byUser = new Mock<IUser>();

            _storyService.Approve(story.Object, "http://dotnetshoutout.com/Dummy-Story", byUser.Object);
        }
    }
}