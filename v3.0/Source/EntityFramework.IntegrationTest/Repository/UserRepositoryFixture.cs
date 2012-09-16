using System;
using System.Linq;
using System.Data;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;
    using DomainObjects;

    public class UserRepositoryFixture : BaseIntegrationFixture
    {
        private readonly UserRepository _userRepository;

        public UserRepositoryFixture()
        {
            _userRepository = new UserRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new UserRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var user = CreateNewUser();
                _userRepository.Add(user);
                Assert.Equal(EntityState.Added, user.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, user.EntityState);
            }
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            using (BeginTransaction())
            {
                var user = CreateNewUser("demouser");
                _database.InsertOnSubmit(user);
                _database.SubmitChanges();

                var newUser = CreateNewUser("demouser");
                Assert.Throws<ArgumentException>(() => _userRepository.Add(newUser));
            }

        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Email_Already_Exists()
        {
            using (BeginTransaction())
            {
                var user = CreateNewUser("demouser1");
                _database.InsertOnSubmit(user);
                _database.SubmitChanges();

                var email = user.Email;
                var newUser = CreateNewUser("demouser2");
                newUser.Email = email;

                Assert.Throws<ArgumentException>(() => _userRepository.Add(newUser));
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var user = (User)_domainFactory.CreateUser("dummyuser", "dummyuser@mail.com", String.Empty);

                _userRepository.Add(user);

                var category = _domainFactory.CreateCategory("dummycategory");
                var story = _domainFactory.CreateStory(category, user, "192.168.0.1", "dummy title", "dummy Desc",
                                                       "http://kiGG.net/story.aspx");
                var tag = _domainFactory.CreateTag("DummyTag");
                story.AddTag(tag);
                user.AddTag(tag);

#pragma warning disable 168
                var comment = _domainFactory.CreateComment(story, "comment", SystemTime.Now(), user, "192.168.0.2");
                var vote = _domainFactory.CreateStoryVote(story, SystemTime.Now(), user, "192.168.0.1");
                var spamMark = _domainFactory.CreateMarkAsSpam(story, SystemTime.Now(), user, "192.168.0.3");
#pragma warning restore 168

                _database.SubmitChanges();

                _userRepository.Remove(user);
                _database.SubmitChanges();
            }
        }

        [Fact]
        public void FindById_Should_Return_Correct_User()
        {
            using (BeginTransaction())
            {
                var newUser = CreateNewUser("demouser1");
                _database.InsertOnSubmit(newUser);
                _database.SubmitChanges();

                var id = newUser.Id;

                var user = _userRepository.FindById(id);

                Assert.Equal(id, user.Id);
            }

        }

        [Fact]
        public void FindByUserName_Should_Return_Correct_User()
        {
            using (BeginTransaction())
            {
                var newUser = CreateNewUser("demouser1");
                _database.InsertOnSubmit(newUser);
                _database.SubmitChanges();

                var userName = newUser.UserName;

                var user = _userRepository.FindByUserName(userName);

                Assert.Equal(userName, user.UserName);
            }
        }

        [Fact]
        public void FindByEmail_Should_Return_Correct_User()
        {
            using (BeginTransaction())
            {
                var newUser = CreateNewUser("demouser1");
                _database.InsertOnSubmit(newUser);
                _database.SubmitChanges();

                var email = newUser.Email;

                var user = _userRepository.FindByEmail(email);

                Assert.Equal(email, user.Email);
            }

        }

        [Fact]
        public void FindScoreById_Should_Return_Correct_Score()
        {
            using (BeginTransaction())
            {
                GenerateUsersWithScores();
                _database.SubmitChanges();

                var id = _database.UserScoreDataSource.Select(us => us.User).First().Id;

                var userScore = _database.UserScoreDataSource.Where(us => us.User.Id == id).Sum(us => us.Score);

                var foundScore = _userRepository.FindScoreById(id, new DateTime(2009, 1, 1), SystemTime.Now());

                Assert.Equal(userScore, foundScore);
            }
        }

        [Fact]
        public void FindTop_Should_Return_Top_Users()
        {
            using (BeginTransaction())
            {
                GenerateUsersWithScores();
                _database.SubmitChanges();
                var pagedResult = _userRepository.FindTop(SystemTime.Now().AddMinutes(-1), SystemTime.Now(), 0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.Equal(10, pagedResult.Total);
            }
        }

        [Fact]
        public void FindAll_Should_Return_Top_Users()
        {
            using (BeginTransaction())
            {
                GenerateUsersWithScores();
                _database.SubmitChanges();
                var total = _database.UserDataSource.Count(u => u.IsActive && !u.IsLockedOut && u.AssignedRole == (int)Roles.User);
                var pagedResult = _userRepository.FindAll(0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.Equal(total, pagedResult.Total);
            }
        }

        [Fact]
        public void FindIPAddresses_Should_Return_The_IPAddresses_That_The_User_Used_To_Submit_Comment_Promote_And_MarkAsSpams()
        {
            using (BeginTransaction())
            {
                var user = (User)_domainFactory.CreateUser("dummyuser", "dummyuser@mail.com", String.Empty);

                _userRepository.Add(user);

                var category = _domainFactory.CreateCategory("dummycategory");
                var story = _domainFactory.CreateStory(category, user, "192.168.0.1", "dummy title", "dummy Desc",
                                                       "http://kiGG.net/story.aspx");

#pragma warning disable 168
                var comment = _domainFactory.CreateComment(story, "comment", SystemTime.Now(), user, "192.168.0.2");
                var vote = _domainFactory.CreateStoryVote(story, SystemTime.Now(), user, "192.168.0.1");
                var spamMark = _domainFactory.CreateMarkAsSpam(story, SystemTime.Now(), user, "192.168.0.3");
#pragma warning restore 168

                _database.SubmitChanges();
                var ipAddresses = _userRepository.FindIPAddresses(user.Id);

                Assert.Equal(3, ipAddresses.Count);
            }
        }

        private void GenerateUsersWithScores()
        {
            for (var i = 0; i < 10; i++)
            {
                var username = "username" + i;
                var email = "username" + i + "@mail.com";
                var user = (User)_domainFactory.CreateUser(username, email, "Pa$$w0rd");
                user.IsActive = true;
                user.IsLockedOut = false;
                _userRepository.Add(user);
                for (var j = 1; j <= 10; j++)
                {
                    var score = new UserScore
                    {
                        User = user,
                        Action = 2,
                        Score = 10,
                        Timestamp = SystemTime.Now()
                    };

                    _database.AddObject("UserScore", score);
                }
            }
        }
    }
}
