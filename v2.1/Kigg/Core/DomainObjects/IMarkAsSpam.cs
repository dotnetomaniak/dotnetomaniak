namespace Kigg.DomainObjects
{
    using System;

    public interface IMarkAsSpam
    {
        IStory ForStory
        {
            get;
        }

        IUser ByUser
        {
            get;
        }

        string FromIPAddress
        {
            get;
        }

        DateTime MarkedAt
        {
            get;
        }
    }
}