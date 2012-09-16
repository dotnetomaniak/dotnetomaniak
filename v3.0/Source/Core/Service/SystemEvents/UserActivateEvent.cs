namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class UserActivateEventArgs
    {
        [DebuggerStepThrough]
        public UserActivateEventArgs(IUser user)
        {
            User = user;
        }

        public IUser User
        {
            get;
            private set;
        }
    }

    public class UserActivateEvent : BaseEvent<UserActivateEventArgs>
    {
    }
}