using System;

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

            user.ExpectGet(u => u.Id).Returns(Guid.NewGuid());
            user.ExpectGet(u => u.UserName).Returns("Stub");
            user.ExpectGet(u => u.Email).Returns("stub@tdd.com");

            return user.Object;
        }

        protected static ICategory CreateStubCategory()
        {
            var category = new Mock<ICategory>();

            category.ExpectGet(c => c.Id).Returns(Guid.NewGuid());
            category.ExpectGet(c => c.Name).Returns("Stub");

            return category.Object;
        }

        protected static ITag CreateStubTag()
        {
            var tag = new Mock<ITag>();

            tag.ExpectGet(t => t.Id).Returns(Guid.NewGuid());
            tag.ExpectGet(t => t.Name).Returns("Stub");

            return tag.Object;
        }

        protected static IStory CreateStubStory()
        {
            var story = new Mock<IStory>();

            story.ExpectGet(s => s.Id).Returns(Guid.NewGuid());
            story.ExpectGet(s => s.Title).Returns("Stub");
            story.ExpectGet(s => s.BelongsTo).Returns(CreateStubCategory());

            return story.Object;
        }

        protected static IComment CreateStubComment()
        {
            var comment = new Mock<IComment>();

            comment.ExpectGet(c => c.Id).Returns(Guid.NewGuid());
            comment.ExpectGet(c => c.ForStory).Returns(CreateStubStory());

            return comment.Object;
        }

        protected static IVote CreateStubVote()
        {
            var story = new Mock<IStory>();

            story.ExpectGet(s => s.Title).Returns("Stub Title");
            story.ExpectGet(s => s.Url).Returns("Stub Url");

            var vote = new Mock<IVote>();

            vote.ExpectGet(v => v.ForStory).Returns(story.Object);

            return vote.Object;
        }

        protected static IKnownSource CreateStubKnownSource()
        {
            var knownSource = new Mock<IKnownSource>();

            knownSource.ExpectGet(ks => ks.Url).Returns("http://knownsoure.com");
            knownSource.ExpectGet(ks => ks.Grade).Returns(KnownSourceGrade.A);

            return knownSource.Object;
        }
    }
}