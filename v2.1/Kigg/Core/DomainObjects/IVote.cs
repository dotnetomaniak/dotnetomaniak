namespace Kigg.DomainObjects
{
    using System;

    public interface IVote
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

        DateTime PromotedAt
        {
            get;
        }
    }
}