using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Service;
    using Kigg.Test.Infrastructure;

    public class UserScoreServiceFixture : BaseFixture
    {
        private readonly UserScoreService _userScoreService;
        private readonly Mock<IUser> _user;

        public UserScoreServiceFixture()
        {
            Mock<IUserScoreTable> userScoreTable = new Mock<IUserScoreTable>();

            userScoreTable.ExpectGet(us => us.AccountActivated).Returns(5);
            userScoreTable.ExpectGet(us => us.StorySubmitted).Returns(20);
            userScoreTable.ExpectGet(us => us.StoryViewed).Returns(1);
            userScoreTable.ExpectGet(us => us.UpcomingStoryPromoted).Returns(3);
            userScoreTable.ExpectGet(us => us.PublishedStoryPromoted).Returns(2);
            userScoreTable.ExpectGet(us => us.StoryPublished).Returns(10);
            userScoreTable.ExpectGet(us => us.StoryCommented).Returns(2);
            userScoreTable.ExpectGet(us => us.StoryMarkedAsSpam).Returns(10);
            userScoreTable.ExpectGet(us => us.SpamStorySubmitted).Returns(50);
            userScoreTable.ExpectGet(us => us.StoryIncorrectlyMarkedAsSpam).Returns(1);
            userScoreTable.ExpectGet(us => us.SpamCommentSubmitted).Returns(5);

            _user = new Mock<IUser>();

            _user.ExpectGet(u => u.Id).Returns(Guid.NewGuid());
            _user.ExpectGet(u => u.Role).Returns(Roles.User);

            _userScoreService = new UserScoreService(settings.Object, userScoreTable.Object);
        }

        public override void Dispose()
        {
            _user.Verify();
        }

        [Fact]
        public void AccountActivated_Should_Increase_Score()
        {
            _user.Expect(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.AccountActivated)).Verifiable();
            _userScoreService.AccountActivated(_user.Object);
        }

        [Fact]
        public void StorySubmitted_Should_Increase_Score()
        {
            _user.Expect(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StorySubmitted)).Verifiable();
            _userScoreService.StorySubmitted(_user.Object);
        }

        [Fact]
        public void StoryViewed_Should_Increase_Score_When_Story_Has_Not_Expired()
        {
            var story = MockStory();

            _user.Expect(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryViewed)).Verifiable();
            _userScoreService.StoryViewed(story.Object, _user.Object);
        }

        [Fact]
        public void StoryPromoted_Should_Increase_Score_When_Story_Is_Published_And_Story_Has_Not_Expired()
        {
            var story = MockStory();

            story.ExpectGet(s => s.PublishedAt).Returns(SystemTime.Now().AddHours(-4));
            _user.Expect(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.PublishedStoryPromoted)).Verifiable();

            _userScoreService.StoryPromoted(story.Object, _user.Object);
        }

        [Fact]
        public void StoryPromoted_Should_Increase_Score_When_Story_Is_Not_Published_And_Story_Has_Not_Expired()
        {
            var story = MockStory();

            story.ExpectGet(s => s.PublishedAt).Returns((DateTime?) null);
            _user.Expect(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.UpcomingStoryPromoted)).Verifiable();

            _userScoreService.StoryPromoted(story.Object, _user.Object);
        }

        [Fact]
        public void StoryDemoted_Should_Decreases_Score_When_Story_Is_Published_And_Story_Has_Not_Expired()
        {
            var story = MockStory();

            story.ExpectGet(s => s.PublishedAt).Returns(SystemTime.Now().AddHours(-4));
            _user.Expect(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.PublishedStoryDemoted)).Verifiable();

            _userScoreService.StoryDemoted(story.Object, _user.Object);
        }

        [Fact]
        public void StoryDemoted_Should_Decreases_Score_When_Story_Is_Not_Published_And_Story_Has_Not_Expired()
        {
            var story = MockStory();

            story.ExpectGet(s => s.PublishedAt).Returns((DateTime?) null);
            _user.Expect(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.UpcomingStoryDemoted)).Verifiable();

            _userScoreService.StoryDemoted(story.Object, _user.Object);
        }

        [Fact]
        public void StoryMarkedAsSpam_Should_Increase_Score_When_Story_Has_Not_Expired()
        {
            var story = MockStory();

            _user.Expect(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryMarkedAsSpam)).Verifiable();
            _userScoreService.StoryMarkedAsSpam(story.Object, _user.Object);
        }

        [Fact]
        public void StoryUnmarkedAsSpam_Should_Decrease_Score_When_Story_Has_Not_Expired()
        {
            var story = MockStory();

            _user.Expect(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryUnmarkedAsSpam)).Verifiable();
            _userScoreService.StoryUnmarkedAsSpam(story.Object, _user.Object);
        }

        [Fact]
        public void StoryCommented_Should_Increase_Score_When_Story_Has_Not_Expired()
        {
            var story = MockStory();

            _user.Expect(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryCommented)).Verifiable();
            _userScoreService.StoryCommented(story.Object, _user.Object);
        }

        [Fact]
        public void StoryDeleted_Should_Decreasase_Score()
        {
            var story = new Mock<IStory>();

            PrepareStoryToRemove(story);
            story.ExpectGet(s => s.PostedBy).Returns(_user.Object);

            _user.Expect(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryDeleted)).Verifiable();

            _userScoreService.StoryDeleted(story.Object);
        }

        [Fact]
        public void StoryPublished_Should_Increase_Score()
        {
            var story = new Mock<IStory>();
            story.ExpectGet(s => s.PostedBy).Returns(_user.Object);

            _user.Expect(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryPublished)).Verifiable();
            _userScoreService.StoryPublished(story.Object);
        }

        [Fact]
        public void StorySpammed_Should_Decreasase_Score()
        {
            var story = new Mock<IStory>();

            PrepareStoryToRemove(story);
            story.ExpectGet(s => s.PostedBy).Returns(_user.Object);

            _user.Expect(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.SpamStorySubmitted)).Verifiable();

            _userScoreService.StorySpammed(story.Object);
        }

        [Fact]
        public void StoryIncorrectlyMarkedAsSpam_Should_Decrease_Score()
        {
            _user.Expect(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryIncorrectlyMarkedAsSpam)).Verifiable();
            _userScoreService.StoryIncorrectlyMarkedAsSpam(_user.Object);
        }

        [Fact]
        public void CommentSpammed_Should_Decrease_Score()
        {
            _user.Expect(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.SpamCommentSubmitted)).Verifiable();
            _userScoreService.CommentSpammed(_user.Object);
        }

        [Fact]
        public void CommentMarkedAsOffended_Should_Decrease_Score()
        {
            _user.Expect(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.CommentMarkedAsOffended)).Verifiable();
            _userScoreService.CommentMarkedAsOffended(_user.Object);
        }

        private static Mock<IStory> MockStory()
        {
            var story = new Mock<IStory>();
            story.ExpectGet(s => s.CreatedAt).Returns(SystemTime.Now().AddHours(-1));

            return story;
        }

        private static void PrepareStoryToRemove(Mock<IStory> story)
        {
            const int counter = 5;

            DateTime fakeDate = SystemTime.Now().AddDays(-1);

            List<IMarkAsSpam> markAsSpams = new List<IMarkAsSpam>();

            for(var i = 1; i <= counter; i++)
            {
                var markAsSpam = new Mock<IMarkAsSpam>();

                markAsSpam.ExpectGet(m => m.ByUser).Returns(new Mock<IUser>().Object);
                markAsSpam.ExpectGet(m => m.MarkedAt).Returns(fakeDate.AddHours(1));

                markAsSpams.Add(markAsSpam.Object);
            }

            story.ExpectGet(s => s.MarkAsSpams).Returns(markAsSpams);

            List<IComment> comments = new List<IComment>();

            for (var i = 1; i <= counter; i++)
            {
                var comment = new Mock<IComment>();

                comment.ExpectGet(c => c.ByUser).Returns(new Mock<IUser>().Object);
                comment.ExpectGet(c => c.CreatedAt).Returns(fakeDate.AddHours(1));

                comments.Add(comment.Object);
            }

            story.ExpectGet(s => s.Comments).Returns(comments);

            List<IVote> votes = new List<IVote>();

            for (var i = 1; i <= counter; i++)
            {
                var vote = new Mock<IVote>();

                vote.ExpectGet(v => v.ByUser).Returns(new Mock<IUser>().Object);
                vote.ExpectGet(v => v.PromotedAt).Returns(fakeDate.AddHours(1));

                votes.Add(vote.Object);
            }

            story.ExpectGet(s => s.Votes).Returns(votes);

            story.ExpectGet(s => s.CreatedAt).Returns(fakeDate);
        }
    }
}