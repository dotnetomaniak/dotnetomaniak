using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class SpamPostprocessorFixture : BaseFixture
    {
        private readonly Mock<IStoryRepository> _storyRepository;
        private readonly Mock<IEmailSender> _emailSender;

        private readonly SpamPostprocessor _spamPostprocessor;

        public SpamPostprocessorFixture()
        {
            _storyRepository = new Mock<IStoryRepository>();
            _emailSender = new Mock<IEmailSender>();

            _spamPostprocessor = new SpamPostprocessor(unitOfWork.Object, _storyRepository.Object, _emailSender.Object);
        }

        [Fact]
        public void Process_For_Story_Should_Log_Warning_When_Story_Is_Spam()
        {
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            _spamPostprocessor.Process("foo", true, "http://test.com", new Mock<IStory>().Object);

            log.Verify();
        }

        [Fact]
        public void Process_For_Story_Should_Send_Mail_When_Story_Is_Spam()
        {
            _emailSender.Expect(es => es.NotifySpamStory(It.IsAny<string>(), It.IsAny<IStory>(), It.IsAny<string>())).Verifiable();

            _spamPostprocessor.Process("foo", true, "http://test.com", new Mock<IStory>().Object);

            _emailSender.Verify();
        }

        [Fact]
        public void Process_For_Story_Should_Approve_Story_When_Story_Is_Not_Spam()
        {
            var story = new Mock<IStory>();

            _storyRepository.Expect(r => r.FindById(It.IsAny<Guid>())).Returns(story.Object);
            story.Expect(s => s.Approve(It.IsAny<DateTime>())).Verifiable();

            _spamPostprocessor.Process("foo", false, "http://test.com", new Mock<IStory>().Object);

            story.Verify();
        }

        [Fact]
        public void Process_For_Story_Should_Use_StoryRepository_When_Story_Is_Not_Spam()
        {
            _storyRepository.Expect(r => r.FindById(It.IsAny<Guid>())).Returns(new Mock<IStory>().Object).Verifiable();

            _spamPostprocessor.Process("foo", false, "http://test.com", new Mock<IStory>().Object);

            _storyRepository.Verify();
        }

        [Fact]
        public void Process_For_Comment_Should_Log_Warning_When_Comment_Is_Spam()
        {
            Process_For_Comment();

            log.Verify();
        }

        [Fact]
        public void Process_For_Comment_Should_Send_Mail_When_Comment_Is_Spam()
        {
            Process_For_Comment();

            _emailSender.Verify();
        }

        private void Process_For_Comment()
        {
            var comment = new Mock<IComment>();

            comment.ExpectGet(c => c.ForStory).Returns(new Mock<IStory>().Object);
            comment.ExpectGet(c => c.ByUser).Returns(new Mock<IUser>().Object);

            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            _emailSender.Expect(es => es.NotifySpamComment(It.IsAny<string>(), It.IsAny<IComment>(), It.IsAny<string>())).Verifiable();

            _spamPostprocessor.Process("foo", true, "http://test.com", comment.Object);
        }
    }
}