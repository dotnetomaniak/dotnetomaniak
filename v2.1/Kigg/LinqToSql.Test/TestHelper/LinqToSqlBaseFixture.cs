using System.Collections.Generic;
using System.Linq;

using Moq;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;
    using Repository;
    using Repository.LinqToSql;
    using Kigg.Test.Infrastructure;

    public abstract class LinqToSqlBaseFixture : BaseFixture
    {
        protected readonly List<User> Users;
        protected readonly List<Category> Categories;
        protected readonly List<Tag> Tags;
        protected readonly List<Story> Stories;
        protected readonly List<KnownSource> KnownSources;
        protected readonly List<UserScore> UserScores;
        protected readonly List<UserTag> UserTags;
        protected readonly List<StoryTag> StoryTags;
        protected readonly List<StoryView> Views;
        protected readonly List<StoryVote> Votes;
        protected readonly List<StoryMarkAsSpam> MarkAsSpams;
        protected readonly List<StoryComment> Comments;
        protected readonly List<CommentSubscribtion> CommentSubscribtions;

        protected readonly Mock<IDatabaseFactory> databaseFactory;
        protected readonly Mock<IDatabase> database;

        protected readonly Mock<IDomainObjectFactory> domainObjectFactory;
        protected readonly Mock<IUserRepository> userRepository;
        protected readonly Mock<ICategoryRepository> categoryRepository;
        protected readonly Mock<ITagRepository> tagRepository;
        protected readonly Mock<IStoryRepository> storyRepository;
        protected readonly Mock<IKnownSourceRepository> knownSourceRepository;
        protected readonly Mock<ICommentRepository> commentRepository;
        protected readonly Mock<ICommentSubscribtionRepository> commentSubscribtionRepository;
        protected readonly Mock<IMarkAsSpamRepository> markAsSpamRepository;
        protected readonly Mock<IStoryViewRepository> storyViewRepository;
        protected readonly Mock<IVoteRepository> voteRepository;

        protected LinqToSqlBaseFixture()
        {
            databaseFactory = new Mock<IDatabaseFactory>();
            database = new Mock<IDatabase>();

            databaseFactory.Expect(f => f.Get()).Returns(database.Object);

            domainObjectFactory = new Mock<IDomainObjectFactory>();

            userRepository = new Mock<IUserRepository>();
            categoryRepository = new Mock<ICategoryRepository>();
            tagRepository = new Mock<ITagRepository>();
            storyRepository = new Mock<IStoryRepository>();
            knownSourceRepository = new Mock<IKnownSourceRepository>();
            commentRepository = new Mock<ICommentRepository>();
            commentSubscribtionRepository = new Mock<ICommentSubscribtionRepository>();
            markAsSpamRepository = new Mock<IMarkAsSpamRepository>();
            storyViewRepository = new Mock<IStoryViewRepository>();
            voteRepository = new Mock<IVoteRepository>();

            resolver.Expect(r => r.Resolve<IDatabaseFactory>()).Returns(databaseFactory.Object);
            resolver.Expect(r => r.Resolve<IDomainObjectFactory>()).Returns(domainObjectFactory.Object);
            resolver.Expect(r => r.Resolve<IUserRepository>()).Returns(userRepository.Object);
            resolver.Expect(r => r.Resolve<ICategoryRepository>()).Returns(categoryRepository.Object);
            resolver.Expect(r => r.Resolve<ITagRepository>()).Returns(tagRepository.Object);
            resolver.Expect(r => r.Resolve<IStoryRepository>()).Returns(storyRepository.Object);
            resolver.Expect(r => r.Resolve<IKnownSourceRepository>()).Returns(knownSourceRepository.Object);
            resolver.Expect(r => r.Resolve<ICommentRepository>()).Returns(commentRepository.Object);
            resolver.Expect(r => r.Resolve<ICommentSubscribtionRepository>()).Returns(commentSubscribtionRepository.Object);
            resolver.Expect(r => r.Resolve<IMarkAsSpamRepository>()).Returns(markAsSpamRepository.Object);
            resolver.Expect(r => r.Resolve<IStoryViewRepository>()).Returns(storyViewRepository.Object);
            resolver.Expect(r => r.Resolve<IVoteRepository>()).Returns(voteRepository.Object);

            Users = new List<User>();
            Categories = new List<Category>();
            Tags = new List<Tag>();
            Stories = new List<Story>();
            KnownSources = new List<KnownSource>();
            UserScores = new List<UserScore>();
            UserTags = new List<UserTag>();
            StoryTags = new List<StoryTag>();
            Views = new List<StoryView>();
            Votes = new List<StoryVote>();
            MarkAsSpams = new List<StoryMarkAsSpam>();
            Comments = new List<StoryComment>();
            CommentSubscribtions = new List<CommentSubscribtion>();

            database.ExpectGet(db => db.UserDataSource).Returns(Users.AsQueryable());
            database.ExpectGet(db => db.CategoryDataSource).Returns(Categories.AsQueryable());
            database.ExpectGet(db => db.TagDataSource).Returns(Tags.AsQueryable());
            database.ExpectGet(db => db.StoryDataSource).Returns(Stories.AsQueryable());
            database.ExpectGet(db => db.KnownSourceDataSource).Returns(KnownSources.AsQueryable());
            database.ExpectGet(db => db.UserScoreDataSource).Returns(UserScores.AsQueryable());
            database.ExpectGet(db => db.UserTagDataSource).Returns(UserTags.AsQueryable());
            database.ExpectGet(db => db.StoryTagDataSource).Returns(StoryTags.AsQueryable());
            database.ExpectGet(db => db.StoryViewDataSource).Returns(Views.AsQueryable());
            database.ExpectGet(db => db.VoteDataSource).Returns(Votes.AsQueryable());
            database.ExpectGet(db => db.MarkAsSpamDataSource).Returns(MarkAsSpams.AsQueryable());
            database.ExpectGet(db => db.CommentDataSource).Returns(Comments.AsQueryable());
            database.ExpectGet(db => db.CommentSubscribtionDataSource).Returns(CommentSubscribtions.AsQueryable());
        }

        public override void Dispose()
        {
            database.Verify();
        }
    }
}