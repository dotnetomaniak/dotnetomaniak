namespace Kigg.EF.DomainObjects
{
    using System;

    using Kigg.DomainObjects;
    
    public class DomainObjectFactory : IDomainObjectFactory
    {
        public IUser CreateUser(string userName, string email, string password)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotInvalidEmail(email, "email");

            var now = SystemTime.Now();

            string hashedPassword = null;

            if (!string.IsNullOrEmpty(password))
            {
                hashedPassword = password.Trim().Hash();
            }

            return new User
                       {
                           Id = Guid.NewGuid(),
                           UserName = userName.Trim(),
                           Email = email.ToLowerInvariant(),
                           Password = hashedPassword,
                           Role = Roles.User,
                           IsActive = string.IsNullOrEmpty(hashedPassword),
                           LastActivityAt = now,
                           CreatedAt = now,
                       };
        }

        public IKnownSource CreateKnownSource(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            return new KnownSource
                       {
                           Url = url.ToLowerInvariant(),
                           Grade = KnownSourceGrade.None
                       };
        }

        public ICategory CreateCategory(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return new Category
                       {
                           Id = Guid.NewGuid(),
                           UniqueName = name.Trim().ToLegalUrl(),
                           Name = name.Trim(),
                           CreatedAt = SystemTime.Now()
                       };
        }

        public ITag CreateTag(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return new Tag
                       {
                           Id = Guid.NewGuid(),
                           UniqueName = name.Trim().ToLegalUrl(),
                           Name = name.Trim(),
                           CreatedAt = SystemTime.Now()
                       };
        }

        public IStory CreateStory(ICategory forCategory, IUser byUser, string fromIpAddress, string title, string description, string url)
        {
            Check.Argument.IsNotNull(forCategory, "forCategory");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");
            Check.Argument.IsNotEmpty(title, "title");
            Check.Argument.IsNotEmpty(description, "description");
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            var now = SystemTime.Now();

            return new Story
                       {
                           Id = Guid.NewGuid(),
                           UniqueName = title.Trim().ToLegalUrl(),
                           Title = title.Trim(),
                           HtmlDescription = description.Trim(),
                           Url = url,
                           IpAddress = fromIpAddress,
                           CreatedAt = now,
                           LastActivityAt = now,
                           UrlHash = url.ToUpperInvariant().Hash(),
                           User = (User) byUser,
                           Category = (Category) forCategory,
                       };
        }

        public IStoryView CreateStoryView(IStory forStory, DateTime at, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            return new StoryView
                       {
                           Story = (Story) forStory,
                           IpAddress = fromIpAddress,
                           Timestamp = at,
                       };
        }

        public IVote CreateStoryVote(IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            PerformCheck(forStory, at, byUser, fromIpAddress);

            return new StoryVote
                       {
                           Story = (Story) forStory,
                           User = (User) byUser,
                           IpAddress = fromIpAddress,
                           Timestamp = at
                       };
        }

        public IMarkAsSpam CreateMarkAsSpam(IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            PerformCheck(forStory, at, byUser, fromIpAddress);

            return new StoryMarkAsSpam
                       {
                           Story = (Story) forStory,
                           User = (User) byUser,
                           IpAddress = fromIpAddress,
                           Timestamp = at
                       };
        }

        public IComment CreateComment(IStory forStory, string content, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotEmpty(content, "content");
            PerformCheck(forStory, at, byUser, fromIpAddress);

            return new StoryComment
                       {
                           Id = Guid.NewGuid(),
                           HtmlBody = content.Trim(),
                           TextBody = content.StripHtml().Trim(),
                           Story = (Story) forStory,
                           User = (User) byUser,
                           IpAddress = fromIpAddress,
                           CreatedAt = at
                       };
        }

        public ICommentSubscribtion CreateCommentSubscribtion(IStory forStory, IUser byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            return new CommentSubscribtion((Story)forStory, (User)byUser);
        }

        private static void PerformCheck(IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");
        }
    }
}
