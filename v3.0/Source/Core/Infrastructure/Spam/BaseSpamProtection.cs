using System;

namespace Kigg.Infrastructure
{
    public abstract class BaseSpamProtection : ISpamProtection
    {
        public ISpamProtection NextHandler
        {
            get;
            set;
        }

        public abstract bool IsSpam(SpamCheckContent spamCheckContent);

        public abstract void IsSpam(SpamCheckContent spamCheckContent, Action<string, bool> callback);
    }
}