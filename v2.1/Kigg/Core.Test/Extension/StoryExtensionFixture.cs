using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class StoryExtensionFixture : BaseFixture
    {
        private readonly Mock<IStory> _story;

        public StoryExtensionFixture()
        {
            _story = new Mock<IStory>();
        }

        [Fact]
        public void IsNew_Should_Be_True_When_LastProcessedAt_Is_Null()
        {
            _story.ExpectGet(s => s.LastProcessedAt).Returns((DateTime?) null);

            Assert.True(_story.Object.IsNew());
        }

        [Fact]
        public void IsPublished_Should_Be_True_When_PublishedAt_Is_Not_Null()
        {
            _story.ExpectGet(s => s.PublishedAt).Returns(SystemTime.Now());

            Assert.True(_story.Object.IsPublished());
        }

        [Fact]
        public void HasExpired_Should_Be_True_When_CreatedAt_Is_Greater_Than_MaximumAgeToPublish()
        {
            var date = SystemTime.Now().AddHours(- (settings.Object.MaximumAgeOfStoryInHoursToPublish + 1));

            _story.ExpectGet(s => s.CreatedAt).Returns(date);

            Assert.True(_story.Object.HasExpired());
        }

        [Fact]
        public void IsApproved_Should_Be_True_When_Aapproved_Is_Not_Null()
        {
            _story.ExpectGet(s => s.ApprovedAt).Returns(SystemTime.Now());

            Assert.True(_story.Object.IsApproved());
        }

        [Fact]
        public void HasComments_Should_Be_True_When_CommentCount_Is_Greater_Than_Zero()
        {
            _story.ExpectGet(s => s.CommentCount).Returns(1);

            Assert.True(_story.Object.HasComments());
        }

        [Fact]
        public void IsPostedBy_Should_Be_True_When_PostedBy_User_Id_Is_Same()
        {
            var userId = Guid.NewGuid();

            var postedByUser = new Mock<IUser>();

            postedByUser.ExpectGet(u => u.Id).Returns(userId);
            _story.ExpectGet(s => s.PostedBy).Returns(postedByUser.Object);

            var checkingUser = new Mock<IUser>();
            checkingUser.ExpectGet(u => u.Id).Returns(userId);

            Assert.True(_story.Object.IsPostedBy(checkingUser.Object));
        }

        [Fact]
        public void Host_Returns_The_Domain_Name_From_The_Story_Url()
        {
            _story.ExpectGet(s => s.Url).Returns("http://weblogs.asp.net/rashid");

            Assert.Equal("weblogs.asp.net", _story.Object.Host());
        }

        [Fact]
        public void SmallThumbnail_Uses_Thumbnail_To_Build_Url()
        {
            thumbnail.Expect(t => t.For(It.IsAny<string>(), ThumbnailSize.Small)).Returns("http://thumbnail.com").Verifiable();

            _story.ExpectGet(s => s.Url).Returns("http://dotnetshoutout.com");

            _story.Object.SmallThumbnail();

            thumbnail.Verify();
        }

        [Fact]
        public void MediumThumbnail_Uses_Thumbnail_To_Build_Url()
        {
            thumbnail.Expect(t => t.For(It.IsAny<string>(), ThumbnailSize.Medium)).Returns("http://thumbnail.com").Verifiable();

            _story.ExpectGet(s => s.Url).Returns("http://dotnetshoutout.com");

            _story.Object.MediumThumbnail();

            thumbnail.Verify();
        }
    }
}