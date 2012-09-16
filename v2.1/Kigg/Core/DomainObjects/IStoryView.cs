namespace Kigg.DomainObjects
{
    using System;

    public interface IStoryView
    {
        IStory ForStory
        {
            get;
        }

        string FromIPAddress
        {
            get;
        }

        DateTime ViewedAt
        {
            get;
        }
    }
}