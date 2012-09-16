using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Service;
    using Kigg.Test.Infrastructure;

    public class EmailSenderFixture : BaseFixture
    {
        private readonly Mock<IFile> _file;
        private readonly EmailSender _emailSender;

        public EmailSenderFixture()
        {
            _file = new Mock<IFile>();
            _emailSender = new EmailSender("MailTemplates", true, settings.Object, _file.Object);
        }

        public override void Dispose()
        {
            _file.Verify();
            cache.Verify();
            log.Verify();
        }

        [Fact]
        public void SendRegistrationInfo_Should_Send_Registration_Details()
        {
            const string mailTemplate = "UserName:<%=userName%>" +
                                        "\r\n" +
                                        "Password:<%=password%>" +
                                        "\r\n" +
                                        "Url:<%=url%>";

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.SendRegistrationInfo("dummy@users.com", "dummyUser", "dummyPassword", "http://dotnetshoutout.com/Activate/JKgxzYZ2dEeRQz_D7XlRDw");
            Sleep();
        }

        [Fact]
        public void SendNewPassword_Should_Send_New_Password()
        {
            const string mailTemplate = "UserName:<%=userName%>" +
                                        "\r\n" +
                                        "Password:<%=password%>";

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.SendNewPassword("dummy@users.com", "dummyUser", "dummyPassword");
            Sleep();
        }

        [Fact]
        public void SendComment_Should_Send_Comment_To_Subscribed_Users()
        {
            const string mailTemplate = "The following comments has been submitted by <%=userName%> in <%=detailUrl%> :\r\n\r\n" +
                                        "<%=comment%>";

            var user = new Mock<IUser>();

            user.ExpectGet(u => u.UserName).Returns("Dummy user");
            user.ExpectGet(u => u.Email).Returns("dummy@users.com");

            var story = new Mock<IStory>();
            story.ExpectGet(s => s.Title).Returns("A dummy story");

            var comment = new Mock<IComment>();
            comment.ExpectGet(c => c.ByUser).Returns(user.Object);
            comment.ExpectGet(c => c.TextBody).Returns("The is a dummy comment");
            comment.ExpectGet(c => c.ForStory).Returns(story.Object);

            var subscriber = new Mock<IUser>();
            subscriber.ExpectGet(u => u.Email).Returns("seconddummy@users.com");

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.SendComment("http://dotnetshoutout.com/The-Dummy_Story", comment.Object, new[] { user.Object, subscriber.Object });

            Sleep();
        }

        [Fact]
        public void NotifySpamStory_Should_Send_Spamed_Story_Details()
        {
            const string mailTemplate = "Title: <%=title%>" +
                                        "Site Url: <%=siteUrl%>" +
                                        "Original Url: <%=originalUrl%>" +
                                        "User: <%=userName%>";

            var user = new Mock<IUser>();

            user.ExpectGet(u => u.UserName).Returns("Dummy user");

            var story = new Mock<IStory>();

            story.ExpectGet(s => s.Title).Returns("A dummy story");
            story.ExpectGet(s => s.Url).Returns("http://www.dummystory.com");
            story.ExpectGet(s => s.PostedBy).Returns(user.Object);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifySpamStory("http://dotnetshoutout.com/The-Dummy_Story", story.Object, "foo");

            Sleep();
        }

        [Fact]
        public void NotifyStoryMarkedAsSpam_Should_Send_Marked_Story_Details()
        {
            const string mailTemplate = "The following story has been marked as spam by <%=markedByUserName%>:\r\n\r\n" +
                                        "Title: <%=title%>" +
                                        "Site Url: <%=siteUrl%>" +
                                        "Original Url: <%=originalUrl%>" +
                                        "User: <%=postedByUserName%>";

            var markedBy = new Mock<IUser>();
            markedBy.ExpectGet(u => u.UserName).Returns("Marked By");

            var postedBy = new Mock<IUser>();
            postedBy.ExpectGet(u => u.UserName).Returns("Posted By");

            var story = new Mock<IStory>();

            story.ExpectGet(s => s.Title).Returns("A dummy story");
            story.ExpectGet(s => s.Url).Returns("http://www.dummystory.com");
            story.ExpectGet(s => s.PostedBy).Returns(postedBy.Object);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifyStoryMarkedAsSpam("http://dotnetshoutout.com/The-Dummy_Story", story.Object, markedBy.Object);

            Sleep();
        }

        [Fact]
        public void NotifySpamComment_Should_Send_Spamed_Comment_Details()
        {
            const string mailTemplate = "Site Url: <%=siteUrl%>" +
                                        "User: <%=userName%>" +
                                        "Comment: <%=comment%>";

            var user = new Mock<IUser>();

            user.ExpectGet(u => u.UserName).Returns("Dummy user");

            var comment = new Mock<IComment>();

            comment.ExpectGet(c => c.HtmlBody).Returns("<p>A dummy story");
            comment.ExpectGet(c => c.ByUser).Returns(user.Object);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifySpamComment("http://dotnetshoutout.com/The-Dummy_Story", comment.Object, "foo");

            Sleep();
        }

        [Fact]
        public void NotifyStoryApprove_Should_Send_Approved_Story_Details()
        {
            const string mailTemplate = "The following story has been approved by <%=approvedByUserName%>:\r\n\r\n" +
                                        "Title: <%=title%>" +
                                        "Site Url: <%=siteUrl%>" +
                                        "Original Url: <%=originalUrl%>" +
                                        "User: <%=postedByUserName%>";

            var approvedBy = new Mock<IUser>();
            approvedBy.ExpectGet(u => u.UserName).Returns("Approved By");

            var postedBy = new Mock<IUser>();
            postedBy.ExpectGet(u => u.UserName).Returns("Posted By");

            var story = new Mock<IStory>();

            story.ExpectGet(s => s.Title).Returns("A dummy story");
            story.ExpectGet(s => s.Url).Returns("http://www.dummystory.com");
            story.ExpectGet(s => s.PostedBy).Returns(postedBy.Object);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifyStoryApprove("http://dotnetshoutout.com/The-Dummy_Story", story.Object, approvedBy.Object);

            Sleep();
        }

        [Fact]
        public void NotifyConfirmSpamStory_Should_Send_Confirmed_Story_Details()
        {
            const string mailTemplate = "The following story has been confirmed as spam by <%=confirmedByUserName%>:\r\n\r\n" +
                                        "Title: <%=title%>" +
                                        "Site Url: <%=siteUrl%>" +
                                        "Original Url: <%=originalUrl%>" +
                                        "User: <%=postedByUserName%>";

            var confirmedBy = new Mock<IUser>();
            confirmedBy.ExpectGet(u => u.UserName).Returns("Marked By");

            var postedBy = new Mock<IUser>();
            postedBy.ExpectGet(u => u.UserName).Returns("Posted By");

            var story = new Mock<IStory>();

            story.ExpectGet(s => s.Title).Returns("A dummy story");
            story.ExpectGet(s => s.Url).Returns("http://www.dummystory.com");
            story.ExpectGet(s => s.PostedBy).Returns(postedBy.Object);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifyConfirmSpamStory("http://dotnetshoutout.com/The-Dummy_Story", story.Object, confirmedBy.Object);

            Sleep();
        }

        [Fact]
        public void NotifyConfirmSpamComment_Should_Send_Confirmed_Comment_Details()
        {
            const string mailTemplate = "The following comment has been confirmed as spam by <%=confirmedByUserName%>:\r\n\r\n" +
                                        "Site Url: <%=siteUrl%>" +
                                        "User: <%=postedByUserName%>" +
                                        "Comment: <%=comment%>";

            var confirmedBy = new Mock<IUser>();

            confirmedBy.ExpectGet(u => u.UserName).Returns("Confirmed By");

            var postedBy = new Mock<IUser>();

            postedBy.ExpectGet(u => u.UserName).Returns("Posted By");

            var comment = new Mock<IComment>();

            comment.ExpectGet(c => c.HtmlBody).Returns("<p>A dummy story");
            comment.ExpectGet(c => c.ByUser).Returns(postedBy.Object);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifyConfirmSpamComment("http://dotnetshoutout.com/The-Dummy_Story", comment.Object, confirmedBy.Object);

            Sleep();
        }

        [Fact]
        public void NotifyCommentAsOffended_Should_Send_Offended_Comment_Details()
        {
            const string mailTemplate = "The following comment has been marked as offended by <%=offendedByUserName%>:\r\n\r\n" +
                                        "Site Url: <%=siteUrl%>" +
                                        "User: <%=postedByUserName%>" +
                                        "Comment: <%=comment%>";

            var offendedBy = new Mock<IUser>();

            offendedBy.ExpectGet(u => u.UserName).Returns("Offended By");

            var postedBy = new Mock<IUser>();

            postedBy.ExpectGet(u => u.UserName).Returns("Posted By");

            var comment = new Mock<IComment>();

            comment.ExpectGet(c => c.HtmlBody).Returns("<p>A dummy story");
            comment.ExpectGet(c => c.ByUser).Returns(postedBy.Object);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifyCommentAsOffended("http://dotnetshoutout.com/The-Dummy_Story", comment.Object, offendedBy.Object);

            Sleep();
        }

        [Fact]
        public void NotifyStoryDelete_Should_Send_Deleted_Story_Details()
        {
            const string mailTemplate = "The following story has been deleted by <%=deletedByUserName%>:" +
                                        "Title: <%=title%>" +
                                        "Url: <%=url%>" +
                                        "User: <%=postedByUserName%>";

            var postedBy = new Mock<IUser>();
            postedBy.ExpectGet(u => u.UserName).Returns("Posted By");

            var story = new Mock<IStory>();

            story.ExpectGet(s => s.Title).Returns("A dummy story");
            story.ExpectGet(s => s.Url).Returns("http://www.dummystory.com");
            story.ExpectGet(s => s.PostedBy).Returns(postedBy.Object);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            var deletedBy = new Mock<IUser>();
            deletedBy.ExpectGet(u => u.UserName).Returns("Deleted By");

            _emailSender.NotifyStoryDelete(story.Object, deletedBy.Object);

            Sleep();
        }

        [Fact]
        public void Notify_Published_Stories_Should_Send_PublishStories()
        {
            const string mailTemplate = "Story Published: <%=timestamp%>\r\n";

            var category = new Mock<ICategory>();
            category.ExpectGet(c => c.Name).Returns("Dummy");

            var story = new Mock<IStory>();
            story.ExpectGet(s => s.Title).Returns("A dummy story");
            story.ExpectGet(s => s.BelongsTo).Returns(category.Object);
            story.ExpectGet(s => s.Url).Returns("http://dummystory.com");

            var publishedStory = new PublishedStory(story.Object){ Rank = 1 };

            publishedStory.Weights.Add("View", 10);
            publishedStory.Weights.Add("Vote", 300);
            publishedStory.Weights.Add("Comment", 130);
            publishedStory.Weights.Add("User-Score", 450);
            publishedStory.Weights.Add("Known-Source", 5);
            publishedStory.Weights.Add("Freshness", 50);

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifyPublishedStories(SystemTime.Now(), new[] { publishedStory });

            Sleep();
        }

        [Fact]
        public void NotifyFeedback_Should_Send_Feedback_Details()
        {
            const string mailTemplate = "<%=name%> (<%=email%>) wrote:" +
                                        "\r\n\r\n" +
                                        "<%=content%>";

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(mailTemplate).Verifiable();

            _emailSender.NotifyFeedback("dummy@users.com", "dummy name", "dummy feedback");
            Sleep();
        }

        private static void Sleep()
        {
            System.Threading.Thread.Sleep(200);
        }
    }
}