using System.Collections.Generic;
using System.Linq;

using Moq;

namespace Kigg.Infrastructure.EF.Test
{
    using DomainObjects;
    using Repository;
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;
    using Kigg.Test.Infrastructure;

    public abstract class EfBaseFixture : BaseFixture
    {
        protected readonly List<User> Users;
        protected readonly List<Category> Categories;
        protected readonly List<Tag> Tags;
        protected readonly List<Story> Stories;
        protected readonly List<KnownSource> KnownSources;
        protected readonly List<UserScore> UserScores;
        
        protected readonly List<StoryView> Views;
        protected readonly List<StoryVote> Votes;
        protected readonly List<StoryMarkAsSpam> MarkAsSpams;
        protected readonly List<StoryComment> Comments;
        

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

        protected EfBaseFixture()
        {
            databaseFactory = new Mock<IDatabaseFactory>();
            database = new Mock<IDatabase>();

            databaseFactory.Setup(f => f.Create()).Returns(database.Object);

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

            resolver.Setup(r => r.Resolve<IDatabaseFactory>()).Returns(databaseFactory.Object);
            resolver.Setup(r => r.Resolve<IDomainObjectFactory>()).Returns(domainObjectFactory.Object);
            resolver.Setup(r => r.Resolve<IUserRepository>()).Returns(userRepository.Object);
            resolver.Setup(r => r.Resolve<ICategoryRepository>()).Returns(categoryRepository.Object);
            resolver.Setup(r => r.Resolve<ITagRepository>()).Returns(tagRepository.Object);
            resolver.Setup(r => r.Resolve<IStoryRepository>()).Returns(storyRepository.Object);
            resolver.Setup(r => r.Resolve<IKnownSourceRepository>()).Returns(knownSourceRepository.Object);
            resolver.Setup(r => r.Resolve<ICommentRepository>()).Returns(commentRepository.Object);
            resolver.Setup(r => r.Resolve<ICommentSubscribtionRepository>()).Returns(commentSubscribtionRepository.Object);
            resolver.Setup(r => r.Resolve<IMarkAsSpamRepository>()).Returns(markAsSpamRepository.Object);
            resolver.Setup(r => r.Resolve<IStoryViewRepository>()).Returns(storyViewRepository.Object);
            resolver.Setup(r => r.Resolve<IVoteRepository>()).Returns(voteRepository.Object);
            
            Users = new List<User>();
            Categories = new List<Category>();
            Tags = new List<Tag>();
            Stories = new List<Story>();
            KnownSources = new List<KnownSource>();
            UserScores = new List<UserScore>();
            
            Views = new List<StoryView>();
            Votes = new List<StoryVote>();
            MarkAsSpams = new List<StoryMarkAsSpam>();
            Comments = new List<StoryComment>();
            
            database.SetupGet(db => db.UserDataSource).Returns(Users.AsQueryable());
            database.SetupGet(db => db.CategoryDataSource).Returns(Categories.AsQueryable());
            database.SetupGet(db => db.TagDataSource).Returns(Tags.AsQueryable());
            database.SetupGet(db => db.StoryDataSource).Returns(Stories.AsQueryable());
            database.SetupGet(db => db.KnownSourceDataSource).Returns(KnownSources.AsQueryable());
            database.SetupGet(db => db.UserScoreDataSource).Returns(UserScores.AsQueryable());
            database.SetupGet(db => db.StoryViewDataSource).Returns(Views.AsQueryable());
            database.SetupGet(db => db.VoteDataSource).Returns(Votes.AsQueryable());
            database.SetupGet(db => db.MarkAsSpamDataSource).Returns(MarkAsSpams.AsQueryable());
            database.SetupGet(db => db.CommentDataSource).Returns(Comments.AsQueryable());
        }

        public override void Dispose()
        {
            database.Verify();
        }
    }
}