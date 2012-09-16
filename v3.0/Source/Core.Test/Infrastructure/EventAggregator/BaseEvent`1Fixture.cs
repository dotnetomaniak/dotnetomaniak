using System;
using System.Collections.Generic;

using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class BaseEventGenericFixture
    {
        private readonly BaseEventTestDouble<string> _event;

        public BaseEventGenericFixture()
        {
            _event = new BaseEventTestDouble<string>();
        }

        [Fact]
        public void Subscribe_With_ActionShould_Return_New_Token()
        {
            var token = _event.Subscribe(delegate { });

            Assert.NotNull(token);
        }

        [Fact]
        public void Subscribe_With_Action_And_KeepAlive_Should_Return_New_Token()
        {
            var token = _event.Subscribe(delegate { }, true);

            Assert.NotNull(token);
        }

        [Fact]
        public void Subscribe_With_Action_KeepAlive_And_Filter_Should_Return_New_Token()
        {
            var token = _event.Subscribe(delegate { }, true, delegate { return true; });

            Assert.NotNull(token);
        }

        [Fact]
        public void Publish_Should_Raise_Event()
        {
            bool isFired = false;

            _event.Subscribe(delegate { isFired = true; });

            _event.Publish("fireIt");

            Assert.True(isFired);
        }

        [Fact]
        public void Unsubscribe_Should_Decrease_Subscriptions()
        {
            Action<string> fireIt = aParam => Console.Write(aParam);

            _event.Subscribe(fireIt);

            var previousCount = _event.SubscriptionCollection.Count;

            _event.Unsubscribe(fireIt);

            Assert.True(_event.SubscriptionCollection.Count < previousCount);
        }

        [Fact]
        public void Contains_Should_Return_True_When_Subscriber_Exists()
        {
            Action<string> fireIt = aParam => Console.Write(aParam);

            _event.Subscribe(fireIt);

            Assert.True(_event.Contains(fireIt));
        }
    }

    public class BaseEventTestDouble<TPayload> : BaseEvent<TPayload>
    {
        public ICollection<IEventSubscription> SubscriptionCollection
        {
            get
            {
                return Subscriptions;
            }
        }
    }
}