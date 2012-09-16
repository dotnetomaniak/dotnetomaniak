namespace Kigg.Infrastructure
{
    using System;

    public interface IEventSubscription
    {
        SubscriptionToken SubscriptionToken
        {
            get;
            set;
        }

        Action<object[]> GetExecutionStrategy();
    }
}