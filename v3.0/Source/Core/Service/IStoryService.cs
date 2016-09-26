namespace Kigg.Service
{
    using System;
    using System.Collections.Specialized;

    using DomainObjects;
    using System.Collections.Generic;

    public interface IStoryService
    {
        StoryCreateResult Create(IUser byUser, string url, string title, string category, string description, string tags, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables, Func<IStory, string> buildDetailUrl);

        void Update(IStory theStory, string uniqueName, DateTime createdAt, string title, string category, string description, string tags);

        void Delete(IStory theStory, IUser byUser);

        void View(IStory theStory, IUser byUser, string fromIPAddress);

        void Promote(IStory theStory, IUser byUser, string fromIPAddress);

        void Demote(IStory theStory, IUser byUser);

        void MarkAsSpam(IStory theStory, string storyUrl, IUser byUser, string fromIPAddress);

        void UnmarkAsSpam(IStory theStory, IUser byUser);

        CommentCreateResult Comment(IStory forStory, string storyUrl, IUser byUser, string content, bool subscribe, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables);

        void Publish();

        void Approve(IStory theStory, string storyUrl, IUser byUser);

        void Spam(IStory theStory, string storyUrl, IUser byUser);

        void Spam(IComment theComment, string storyUrl, IUser byUser);

        void MarkAsOffended(IComment theComment, string storyUrl, IUser byUser);

        IList<IStory> FindWeekly(DateTime minimumDate, DateTime maximumDate);
    }
}