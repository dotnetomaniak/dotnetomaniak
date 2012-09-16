namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Service;

    public interface IEmailSender
    {
        void SendRegistrationInfo(string email, string userName, string password, string activateUrl);

        void SendNewPassword(string email, string userName, string password);

        void SendComment(string url, IComment comment, IEnumerable<IUser> users);

        void NotifySpamStory(string url, IStory story, string source);

        void NotifyStoryMarkedAsSpam(string url, IStory story, IUser byUser);

        void NotifySpamComment(string url, IComment comment, string source);

        void NotifyStoryApprove(string url, IStory story, IUser byUser);

        void NotifyConfirmSpamStory(string url, IStory story, IUser byUser);

        void NotifyConfirmSpamComment(string url, IComment comment, IUser byUser);

        void NotifyCommentAsOffended(string url, IComment comment, IUser byUser);

        void NotifyStoryDelete(IStory story, IUser byUser);

        void NotifyPublishedStories(DateTime timestamp, IEnumerable<PublishedStory> stories);

        void NotifyFeedback(string email, string name, string content);
    }
}