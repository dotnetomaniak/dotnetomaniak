using System;
using Kigg.DomainObjects;
using Moq;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Kigg.Test.Infrastructure;

    public abstract class DecoratedRepositoryFixture : BaseFixture
    {
        protected static IUser CreateStubUser()
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            user.SetupGet(u => u.UserName).Returns("Stub");
            user.SetupGet(u => u.Email).Returns("stub@tdd.com");

            return user.Object;
        }

        protected static ICategory CreateStubCategory()
        {
            var category = new Mock<ICategory>();

            category.SetupGet(c => c.Id).Returns(Guid.NewGuid());
            category.SetupGet(c => c.Name).Returns("Stub");

            return category.Object;
        }

        protected static ITag CreateStubTag()
        {
            var tag = new Mock<ITag>();

            tag.SetupGet(t => t.Id).Returns(Guid.NewGuid());
            tag.SetupGet(t => t.Name).Returns("Stub");

            return tag.Object;
        }

        protected static IStory CreateStubStory()
        {
            var story = new Mock<IStory>();

            story.SetupGet(s => s.Id).Returns(Guid.NewGuid());
            story.SetupGet(s => s.Title).Returns("Stub");
            story.SetupGet(s => s.BelongsTo).Returns(CreateStubCategory());

            return story.Object;
        }

        protected static IComment CreateStubComment()
        {
            var comment = new Mock<IComment>();

            comment.SetupGet(c => c.Id).Returns(Guid.NewGuid());
            comment.SetupGet(c => c.ForStory).Returns(CreateStubStory());

            return comment.Object;
        }

        protected static IVote CreateStubVote()
        {
            var story = new Mock<IStory>();

            story.SetupGet(s => s.Title).Returns("Stub Title");
            story.SetupGet(s => s.Url).Returns("Stub Url");

            var vote = new Mock<IVote>();

            vote.SetupGet(v => v.ForStory).Returns(story.Object);

            return vote.Object;
        }

        protected static IKnownSource CreateStubKnownSource()
        {
            var knownSource = new Mock<IKnownSource>();

            knownSource.SetupGet(ks => ks.Url).Returns("http://knownsoure.com");
            knownSource.SetupGet(ks => ks.Grade).Returns(KnownSourceGrade.A);

            return knownSource.Object;
        }
    }
}