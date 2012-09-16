namespace Kigg.Infrastructure
{
    using System;
    using System.Diagnostics;

    public abstract class BaseBackgroundTask : IBackgroundTask
    {
        private readonly IEventAggregator _eventAggregator;

        protected BaseBackgroundTask(IEventAggregator eventAggregator)
        {
            Check.Argument.IsNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        protected internal IEventAggregator EventAggregator
        {
            [DebuggerStepThrough]
            get
            {
                return _eventAggregator;
            }
        }

        public void Start()
        {
            OnStart();
            IsRunning = true;
        }

        public void Stop()
        {
            OnStop();
            IsRunning = false;
        }

        protected abstract void OnStart();

        protected abstract void OnStop();

        protected internal SubscriptionToken Subscribe<TEvent, TEventArgs>(Action<TEventArgs> action) where TEvent : BaseEvent<TEventArgs> where TEventArgs : class
        {
            return _eventAggregator.GetEvent<TEvent>().Subscribe(action, true);
        }

        protected internal void Unsubscribe<TEvent>(SubscriptionToken token) where TEvent : BaseEvent
        {
            _eventAggregator.GetEvent<TEvent>().Unsubscribe(token);
        }
    }
}