using System;
using System.Collections.Generic;
using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class BaseEventFixture
    {
        private readonly BaseEventTestDouble _event;

        public BaseEventFixture()
        {
            _event = new BaseEventTestDouble();
        }

        [Fact]
        public void Subscribe_Should_Increase_Subscribtions()
        {
            int previousCount = _event.SubscriptionColection.Count;

            Subscribe();

            Assert.True(_event.SubscriptionColection.Count > previousCount);
        }

        [Fact]
        public void Contains_Should_Return_True_When_Token_Exists()
        {
            var token = Subscribe();

            Assert.True(_event.Contains(token));
        }

        [Fact]
        public void Unsubscribe_Should_Decrease_Subscribtions()
        {
            var token = Subscribe();
            int previousCount = _event.SubscriptionColection.Count;

            _event.Unsubscribe(token);

            Assert.True(_event.SubscriptionColection.Count < previousCount);
        }

        [Fact]
        public void Publish_Should_Raise_Event()
        {
            Mock<IEventSubscription> subscription = new Mock<IEventSubscription>();

            bool isRaised = false;

            subscription.Setup(s => s.GetExecutionStrategy()).Returns(delegate { isRaised = true; });
            subscription.Setup(s => s.SubscriptionToken).Returns(new SubscriptionToken());

            _event.Subscribe(subscription.Object);
            _event.Publish();

            Assert.True(isRaised);
        }

        [Fact]
        public void Publish_Should_Remove_From_Subscribtion_Is_Action_Is_Null()
        {
            Mock<IEventSubscription> subscription = new Mock<IEventSubscription>();

            subscription.Setup(s => s.GetExecutionStrategy()).Returns((Action<object[]>)null);

            _event.Subscribe(subscription.Object);
            _event.Publish();

            Assert.True(_event.SubscriptionColection.Count == 0);
        }

        private SubscriptionToken Subscribe()
        {
            var eventSubscription = new Mock<IEventSubscription>();

            return _event.Subscribe(eventSubscription.Object);
        }
    }

    public class BaseEventTestDouble : BaseEvent
    {
        public ICollection<IEventSubscription> SubscriptionColection
        {
            get
            {
                return Subscriptions;
            }
        }

        public new SubscriptionToken Subscribe(IEventSubscription eventSubscription)
        {
            return base.Subscribe(eventSubscription);
        }

        public new void Publish(params object[] arguments)
        {
            base.Publish(arguments);
        }
    }
}