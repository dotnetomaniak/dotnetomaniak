using System;
using System.Data;
using System.Data.Common;

using Moq;

using Kigg.EF.DomainObjects;
using Kigg.EF.Repository;



namespace Kigg.Infrastructure.EF.IntegrationTest
{
    public abstract class BaseIntegrationFixture
    {
        protected readonly Database _database;
        protected readonly IConnectionString _connectionString;
        protected readonly DatabaseFactory _dbFactory;
        protected readonly DomainObjectFactory _domainFactory;

        protected readonly Mock<IDependencyResolverFactory> _resolverFactory;
        protected readonly Mock<IDependencyResolver> _resolver;
        protected BaseIntegrationFixture()
        {
            _connectionString = CreateConnectionString();
            _database = new Database(_connectionString.Value);
            _dbFactory = new DatabaseFactory(_connectionString);
            _domainFactory = new DomainObjectFactory();

            _resolver = new Mock<IDependencyResolver>();
            _resolverFactory = new Mock<IDependencyResolverFactory>();
            _resolverFactory.Setup(f => f.CreateInstance()).Returns(_resolver.Object);
            _resolver.Setup(r => r.Resolve<ILog>()).Returns(new Mock<ILog>().Object);
            IoC.InitializeWith(_resolverFactory.Object);
        }
        
        protected string ConnectionString
        {
            get
            {
                return _connectionString.Value; 
            }
        }

        protected Story CreateNewStory(Category forCategory, User byUser, string fromIpAddress, string title, string description, string url)
        {
            return (Story)_domainFactory.CreateStory(forCategory, byUser,
                                               fromIpAddress, title,
                                               description, url);
        }
        protected Story CreateNewStory()
        {
            return CreateNewStory(CreateNewCategory(), CreateNewUser(), 
                                "127.0.0.1", "dummy story",
                                "dummy description", "http://dummy.com/story.aspx");
        }
        protected StoryVote CreateNewVote(Story forStory, User byUser)
        {
            return (StoryVote)_domainFactory.CreateStoryVote(forStory, SystemTime.Now(), byUser, "192.0.0.1");
        }
        protected StoryView CreateNewStoryView()
        {
            return CreateNewStoryView(CreateNewStory(), SystemTime.Now(), "192.168.0.1");
        }
        protected StoryView CreateNewStoryView(Story forStory)
        {
            return CreateNewStoryView(forStory, SystemTime.Now(), "192.168.0.1");
        }
        protected StoryView CreateNewStoryView(Story forStory, DateTime at)
        {
            return CreateNewStoryView(forStory, at, "192.168.0.1");
        }
        protected StoryView CreateNewStoryView(Story forStory, DateTime at, string fromIpAddress)
        {
            return (StoryView)_domainFactory.CreateStoryView(forStory, at, fromIpAddress);
        }
        protected Category CreateNewCategory()
        {
            return CreateNewCategory("DummyCategory");
        }
        protected Category CreateNewCategory(string categoryName)
        {
            return (Category)_domainFactory.CreateCategory(categoryName);
        }
        protected User CreateNewUser()
        {
            return CreateNewUser("dummyuser");
        }
        protected User CreateNewUser(string userName)
        {
            return (User)_domainFactory.CreateUser(userName, userName+"@mail.com", "Pa$$w0rd");
        }
        protected Tag CreateNewTag(string tagName)
        {
            return (Tag)_domainFactory.CreateTag(tagName);
        }
        protected Tag CreateNewTag()
        {
            return CreateNewTag("DummyTag");
        }
        protected StoryComment CreateNewComment(Story forStory, User byUser, string content)
        {
            return (StoryComment)_domainFactory.CreateComment(forStory, content, SystemTime.Now(), byUser, "192.168.0.1");
        }
        
        protected Story GenerateStoryGraph()
        {
            var story = CreateNewStory();
            CreateNewVote(story, story.User);
            CreateNewStoryView(story);
            CreateNewComment(story, story.User, "dummy content");
            story.AddTag(CreateNewTag());
            return story;
        }
        
        protected void GenerateStories(bool publish, bool markNew, bool approve)
        {
            GenerateStories(String.Empty, publish, markNew, approve);
        }

        protected void GenerateStories(string keyword, bool publish, bool markNew, bool approve)
        {
            var rnd = new Random();
            var category = CreateNewCategory();
            var user = CreateNewUser();
            var tag = CreateNewTag();
            for (var i = 0; i < 10; i++)
            {
                var title = "dummy title " + "keyword " + i;
                var url = String.Format("http://blog.net/story{0}.html", i);
                var story = CreateNewStory(category, user, "192.168.0.1", title, "dummy desc", url);
                story.AddTag(tag);
                if (approve)
                {
                    story.Approve(SystemTime.Now().AddDays(-6));
                }
                if (publish)
                {
                    story.Publish(SystemTime.Now().AddDays(-rnd.Next(1, 5)), rnd.Next(1, 10));
                }
                if (markNew)
                {
                    story.LastProcessedAt = null;
                }
                CreateNewStoryView(story);
                CreateNewComment(story, story.User, "dummy content " + keyword);
                _database.InsertOnSubmit(story);
            }
        }
        
        protected IConnectionString CreateConnectionString()
        {
// ReSharper disable RedundantAssignment
            var ssdlFileName = "DomainObjects";

            var connStringName = "KiGG";
// ReSharper restore RedundantAssignment
#if(MySql)
            ssdlFileName = "DomainObjects.MySql";
            connStringName = "KiGGMySql";
#endif

#if(SqlServer)
            ssdlFileName = "DomainObjects.SqlServer";
            connStringName = "KiGG";
#endif
            return new ConnectionString(new ConfigurationManagerWrapper(), connStringName, ".\\EDM", ssdlFileName);
        }

        
        protected TransactionScope BeginTransaction()
        {
            return BeginTransaction(_database);
        }
        protected TransactionScope BeginTransaction(Database db)
        {
            var txMngr = new TransactionScope(db.Connection);
            txMngr.StartTransaction();
            return txMngr;
        }

        protected class TransactionScope : IDisposable
        {
            private readonly DbConnection _connection;
            private DbTransaction _transaction;
            internal TransactionScope(DbConnection connection)
            {
                _connection = connection;
            }
            internal void StartTransaction()
            {
                _connection.Open();
                _transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);

            }
            public void Dispose()
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                }
                _connection.Close();
            }
        }
    }
}
