using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;
    using Service;

    public class ViewWeightFixture
    {
        private const float WeightMultiply = 0.01f;

        private readonly Mock<IStoryViewRepository> _repository;
        private readonly ViewWeight _strategy;

        public ViewWeightFixture()
        {
            _repository = new Mock<IStoryViewRepository>();
            _strategy = new ViewWeight(_repository.Object, WeightMultiply);
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_Same_Ip()
        {
            var story = new Mock<IStory>();

            Setup("192.168.0.1", "192.168.0.1", "192.168.0.1");

            Assert.Equal(WeightMultiply, _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_Different_Ip()
        {
            var story = new Mock<IStory>();

            Setup("192.168.0.1", "192.168.0.2", "192.168.0.3");

            Assert.Equal(WeightMultiply * 3, _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        [Fact]
        public void Calculate_Should_Use_StroyViewRepository()
        {
            var story = new Mock<IStory>();

            Setup("192.168.0.1", "192.168.0.2", "192.168.0.3");

            _strategy.Calculate(SystemTime.Now(), story.Object);

            _repository.Verify();
        }

        private void Setup(params string[] ipAddress)
        {
            var views = new List<IStoryView>();

            foreach(string ip in ipAddress)
            {
                var view = new Mock<IStoryView>();

                view.Expect(v => v.FromIPAddress).Returns(ip);

                views.Add(view.Object);
            }

            _repository.Expect(r => r.FindAfter(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(views).Verifiable();
        }
    }
}