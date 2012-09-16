using System;
using System.Linq;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;
    using Kigg.LinqToSql.Repository;
    using Kigg.LinqToSql.DomainObjects;

    public class UserRepositoryFixture : LinqToSqlBaseFixture
    {
        private readonly UserRepository _userRepository;
        private readonly IDomainObjectFactory _factory;

        public UserRepositoryFixture()
        {
            _factory = new DomainObjectFactory();
            _userRepository = new UserRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new UserRepository(databaseFactory.Object));
        }

        [Fact]
        public void Add_Should_Use_Database()
        {
            database.Setup(d => d.Insert(It.IsAny<User>())).Verifiable();

            _userRepository.Add(_factory.CreateUser("dummy", "dummy@users.com", "xxxx"));
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            Users.Add(_factory.CreateUser("demo", "demo@users.com", "xxxx") as User);

            Assert.Throws<ArgumentException>(() => _userRepository.Add(_factory.CreateUser("demo", "demo@users.com", "xxxx")));
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Email_Already_Exists()
        {
            Users.Add(_factory.CreateUser("demo1", "demo@users.com", "xxxx") as User);

            Assert.Throws<ArgumentException>(() => _userRepository.Add(_factory.CreateUser("demo2", "demo@users.com", "xxxx")));
        }

        [Fact]
        public void Remove_Should_Use_Database()
        {
            Users.Add(_factory.CreateUser("demo", "demo@users.com", "xxxx") as User);

            database.Setup(d => d.Delete(It.IsAny<User>())).Verifiable();

            _userRepository.Remove(Users[0]);
        }

        [Fact]
        public void FindById_Should_Return_Correct_User()
        {
            Users.Add(_factory.CreateUser("demo", "demo@users.com", "xxxx") as User);

            var id = Users[0].Id;
            var user = _userRepository.FindById(id);

            Assert.Equal(id, user.Id);
        }

        [Fact]
        public void FindByUserName_Should_Return_Correct_User()
        {
            Users.Add(_factory.CreateUser("demo", "demo@users.com", "xxxx") as User);

            var userName = Users[0].UserName;
            var user = _userRepository.FindByUserName(userName);

            Assert.Equal(userName, user.UserName);
        }

        [Fact]
        public void FindByEmail_Should_Return_Correct_User()
        {
            Users.Add(_factory.CreateUser("demo", "demo@users.com", "xxxx") as User);

            var email = Users[0].Email;
            var user = _userRepository.FindByEmail(email);

            Assert.Equal(email, user.Email);
        }

        [Fact]
        public void FindScoreById_Should_Return_Correct_Score()
        {
            var id = Guid.NewGuid();

            UserScores.AddRange(    new []
                                    {
                                         new UserScore
                                         {
                                             Id = 1,
                                             Score = 10,
                                             Timestamp = SystemTime.Now().AddDays(-3),
                                             ActionType = UserAction.AccountActivated,
                                             UserId = id
                                         },
                                         new UserScore
                                         {
                                             Id = 2,
                                             Score = 20,
                                             Timestamp = SystemTime.Now().AddDays(-2),
                                             ActionType = UserAction.StorySubmitted,
                                             UserId = id
                                         }
                                    }
                               );


            var score = _userRepository.FindScoreById(id, SystemTime.Now().AddMonths(-1), SystemTime.Now());

            Assert.Equal(30, score);
        }

        [Fact]
        public void FindTop_Should_Return_Top_Users()
        {
            UserScores.AddRange(
                                    new[]
                                    {
                                       new UserScore
                                       {
                                           Id = 1,
                                           Score = 10,
                                           Timestamp = SystemTime.Now().AddDays(-1),
                                           ActionType = UserAction.AccountActivated,
                                           User = new User { Id = Guid.NewGuid(), Role = Roles.User }
                                       },
                                       new UserScore
                                       {
                                           Id = 1,
                                           Score = 10,
                                           Timestamp = SystemTime.Now().AddHours(-12),
                                           ActionType = UserAction.AccountActivated,
                                           User = new User{ Id = Guid.NewGuid(), Role = Roles.User }
                                       },
                                    }
                               );

            Users.AddRange(UserScores.Select(us => us.User));

            var pagedResult = _userRepository.FindTop(SystemTime.Now().AddMonths(-1), SystemTime.Now(), 0, 10);

            Assert.Equal(2, pagedResult.Result.Count);
            Assert.Equal(2, pagedResult.Total);
        }

        [Fact]
        public void FindAll_Should_Return_All_Public_Users()
        {
            var pagedResult = _userRepository.FindAll(0, 10);

            Assert.Empty(pagedResult.Result);
            Assert.Equal(0, pagedResult.Total);
        }

        [Fact]
        public void FindIPAddresses_Should_Return_The_IPAddresses_That_The_User_Used_To_Submit_Comment_Promote_And_MarkAsSpams()
        {
            var id = Guid.NewGuid();

            Stories.Add(new Story { Id = Guid.NewGuid(), UserId = id, IPAddress = "192.168.0.1" });
            Comments.Add(new StoryComment { Id = Guid.NewGuid(), UserId = id, IPAddress = "192.168.0.2", StoryId = Stories[0].Id });
            Votes.Add(new StoryVote { UserId = id, IPAddress = "192.168.0.1", StoryId = Stories[0].Id });
            MarkAsSpams.Add(new StoryMarkAsSpam { UserId = id, IPAddress = "192.168.0.3" });

            var ipAddresses = _userRepository.FindIPAddresses(id);

            Assert.Equal(3, ipAddresses.Count);
        }
    }
}