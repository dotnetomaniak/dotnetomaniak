using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;
    using Service;

    public class UserScoreWeightFixture
    {
        private const float ScorePercent = 0.01f;
        private const float AdminMultiply = 4f;
        private const float ModaratorMultiply = 2f;

        private readonly Mock<IVoteRepository> _repository;
        private readonly UserScoreWeight _strategy;

        public UserScoreWeightFixture()
        {
            _repository = new Mock<IVoteRepository>();
            _strategy = new UserScoreWeight(_repository.Object, ScorePercent, AdminMultiply, ModaratorMultiply);
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight()
        {
            var user1 = new Mock<IUser>();
            var user2 = new Mock<IUser>();
            var user3 = new Mock<IUser>();

            var moderator = new Mock<IUser>();
            var admin = new Mock<IUser>();

            user1.ExpectGet(u => u.Id).Returns(Guid.NewGuid());
            user2.ExpectGet(u => u.Id).Returns(Guid.NewGuid());
            user3.ExpectGet(u => u.Id).Returns(Guid.NewGuid());
            user1.ExpectGet(u => u.CurrentScore).Returns(100);
            user2.ExpectGet(u => u.CurrentScore).Returns(200);
            user3.ExpectGet(u => u.CurrentScore).Returns(300);

            moderator.ExpectGet(u => u.Id).Returns(Guid.NewGuid());
            admin.ExpectGet(u => u.Id).Returns(Guid.NewGuid());
            moderator.ExpectGet(u => u.Role).Returns(Roles.Moderator);
            admin.ExpectGet(u => u.Role).Returns(Roles.Administrator);

            var user1Vote = new Mock<IVote>();
            var user2Vote = new Mock<IVote>();
            var user3Vote = new Mock<IVote>();

            var moderatorVote = new Mock<IVote>();
            var adminVote = new Mock<IVote>();

            user1Vote.Expect(v => v.ByUser).Returns(user1.Object);
            user2Vote.Expect(v => v.ByUser).Returns(user2.Object);
            user3Vote.Expect(v => v.ByUser).Returns(user3.Object);

            moderatorVote.Expect(v => v.ByUser).Returns(moderator.Object);
            adminVote.Expect(v => v.ByUser).Returns(admin.Object);

            var story = new Mock<IStory>();

            _repository.Expect(r => r.FindAfter(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(new[] { user1Vote.Object, user2Vote.Object, user3Vote.Object, moderatorVote.Object, adminVote.Object }).Verifiable();

            Assert.Equal(
                            (
                                (Convert.ToDouble(user1.Object.CurrentScore) * ScorePercent) +
                                (Convert.ToDouble(user2.Object.CurrentScore) * ScorePercent) +
                                (Convert.ToDouble(user3.Object.CurrentScore) * ScorePercent) +
                                ((Convert.ToDouble(user3.Object.CurrentScore) * ScorePercent) * ModaratorMultiply) +
                                ((Convert.ToDouble(user3.Object.CurrentScore) * ScorePercent) * AdminMultiply)
                            ), 
                            _strategy.Calculate(SystemTime.Now(), story.Object)
                        );

            _repository.Verify();
        }
    }
}