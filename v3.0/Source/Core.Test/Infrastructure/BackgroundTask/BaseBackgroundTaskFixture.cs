using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Service;

    public class BaseBackgroundTaskFixture
    {
        private readonly Mock<IEventAggregator> _eventAggregator;

        private readonly Mock<BaseBackgroundTask> _task;

        public BaseBackgroundTaskFixture()
        {
            _eventAggregator = new Mock<IEventAggregator>();

            _task = new Mock<BaseBackgroundTask>(_eventAggregator.Object);
        }

        [Fact]
        public void EventAggregator_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Same(_eventAggregator.Object, _task.Object.EventAggregator);
        }

        [Fact]
        public void IsRunning_Should_Be_False_When_New_Instance_Is_Created()
        {
            Assert.False(_task.Object.IsRunning);
        }

        [Fact]
        public void Start_Should_Change_Is_Running_Status()
        {
            _task.Object.Start();

            Assert.True(_task.Object.IsRunning);
        }

        [Fact]
        public void Stop_Should_Change_Is_Running_Status()
        {
            _task.Object.Start();
            _task.Object.Stop();

            Assert.False(_task.Object.IsRunning);
        }

        [Fact]
        public void Subscribe_Should_Use_EventAggregator()
        {
            var userActivateEvent = new Mock<UserActivateEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<UserActivateEvent>()).Returns(userActivateEvent.Object).Verifiable();
            userActivateEvent.Setup(e => e.Subscribe(It.IsAny<Action<UserActivateEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            _task.Object.Subscribe<UserActivateEvent, UserActivateEventArgs>(delegate{ });

            _eventAggregator.Verify();
            userActivateEvent.Verify();
        }

        [Fact]
        public void Unsubscribe_Should_Use_EventAggregator()
        {
            var userActivateEvent = new Mock<UserActivateEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<UserActivateEvent>()).Returns(userActivateEvent.Object).Verifiable();
            userActivateEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            _task.Object.Unsubscribe<UserActivateEvent>(new SubscriptionToken());

            _eventAggregator.Verify();
            userActivateEvent.Verify();
        }
    }
}