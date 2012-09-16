namespace Kigg.Infrastructure
{
    using System;

    public interface ISpamProtection
    {
        ISpamProtection NextHandler
        {
            get;
            set;
        }

        bool IsSpam(SpamCheckContent spamCheckContent);

        void IsSpam(SpamCheckContent spamCheckContent, Action<string, bool> callback);
    }
}