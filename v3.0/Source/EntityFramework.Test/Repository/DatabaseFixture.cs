using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;
    
    public class DatabaseFixture : IDisposable
    {
        private readonly string _connectionString;
        
        private readonly Mock<Database> _database;

        public DatabaseFixture()
        {
            var configMngr = new Mock<IConfigurationManager>();
            configMngr.Setup(c => c.GetConnectionString("KiGG")).Returns("Data Source=.\\sqlexpress;Initial Catalog=KiGG;Integrated Security=True;MultipleActiveResultSets=False");
            configMngr.Setup(c => c.GetProviderName("KiGG")).Returns("System.Data.SqlClient");
            var connectionString = new ConnectionString(configMngr.Object, "KiGG", ".\\EDM");
            
            _connectionString = connectionString.Value;
            _database = new Mock<Database>(_connectionString);
        }

        [Fact]
        public void Instantiating_New_Database_Instance_Using_ConnectionString_Should_Not_Throw_Exception()
        {
            using(var db = new Database(_connectionString))
            {
                Assert.Equal(Database._defaultContainerName, db.DefaultContainerName);    
            }
        }

        [Fact]
        public void GetEntitySetName_Should_Return_Correct_EntitySet_Name()
        {
            using(var db = new Database(_connectionString))
            {
                var story = db.GetEntitySetName(typeof (Story));
                var storyComment = db.GetEntitySetName(typeof(StoryComment));
                var storyView = db.GetEntitySetName(typeof(StoryView));
                var storyVote = db.GetEntitySetName(typeof(StoryVote));
                var storyMarkAsSpam = db.GetEntitySetName(typeof(StoryMarkAsSpam));
                var user = db.GetEntitySetName(typeof(User));
                var userScore = db.GetEntitySetName(typeof(UserScore));
                var tag = db.GetEntitySetName(typeof(Tag));
                var category = db.GetEntitySetName(typeof(Category));
                var knownSource = db.GetEntitySetName(typeof(KnownSource));
                
                Assert.Equal("Story",story);
                Assert.Equal("StoryComment", storyComment);
                Assert.Equal("StoryView", storyView);
                Assert.Equal("StoryVote", storyVote);
                Assert.Equal("StoryMarkAsSpam", storyMarkAsSpam);
                Assert.Equal("UserDataSource", user);
                Assert.Equal("UserScore", userScore);
                Assert.Equal("Tag", tag);
                Assert.Equal("Category", category);
                Assert.Equal("KnownSource", knownSource);
            }
        }

        [Fact]
        public void CategoryDataSource_Should_Return_Category_Set()
        {
            _database.Setup(d => d.GetQueryable<Category>()).Returns(new List<Category>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.CategoryDataSource);
        }

        [Fact]
        public void TagDataSource_Should_Return_Tag_Set()
        {
            _database.Setup(d => d.GetQueryable<Tag>()).Returns(new List<Tag>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.TagDataSource);
        }

        [Fact]
        public void StoryDataSource_Should_Return_Story_Set()
        {
            _database.Setup(d => d.GetQueryable<Story>()).Returns(new List<Story>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.StoryDataSource);
        }

        [Fact]
        public void CommentDataSource_Should_Return_StoryComment_Set()
        {
            _database.Setup(d => d.GetQueryable<StoryComment>()).Returns(new List<StoryComment>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.CommentDataSource);
        }

        [Fact]
        public void VoteDataSource_Should_Return_StoryVote_Set()
        {
            _database.Setup(d => d.GetQueryable<StoryVote>()).Returns(new List<StoryVote>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.VoteDataSource);
        }

        [Fact]
        public void StoryViewDataSource_Should_Return_StoryView_Set()
        {
            _database.Setup(d => d.GetQueryable<StoryView>()).Returns(new List<StoryView>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.StoryViewDataSource);
        }

        [Fact]
        public void MarkAsSpamDataSource_Should_Return_StoryMarkAsSpam_Set()
        {
            _database.Setup(d => d.GetQueryable<StoryMarkAsSpam>()).Returns(new List<StoryMarkAsSpam>().AsQueryable()).Verifiable();
            
            Assert.NotNull(_database.Object.MarkAsSpamDataSource);
        }

        [Fact]
        public void UserDataSource_Should_Return_User_Set()
        {
            _database.Setup(d => d.GetQueryable<User>()).Returns(new List<User>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.UserDataSource);
        }

        [Fact]
        public void UserScoreDataSource_Should_Return_UserScore_Set()
        {
            _database.Setup(d => d.GetQueryable<UserScore>()).Returns(new List<UserScore>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.UserScoreDataSource);
        }

        [Fact]
        public void KnownSourceDataSource_Should_Return_KnownSource_Set()
        {
            _database.Setup(d => d.GetQueryable<KnownSource>()).Returns(new List<KnownSource>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.KnownSourceDataSource);
        }

        [Fact]
        public void InsertOnSubmit_Should_Call_GetEntitySetName_And_AddObject()
        {
            var story = new Story();

            _database.Object.InsertOnSubmit(story);
            _database.Verify(d => d.GetEntitySetName(typeof(Story)), Times.AtMostOnce());
            _database.Verify(d => d.AddObject("Story", story), Times.AtMostOnce());

            Assert.True(story.EntityState == EntityState.Added);
        }

        [Fact]
        public void DeleteOnSubmit_Should_Call_DeleteEntity()
        {
            var story = new Story();
            _database.Object.AddObject("Story",story);
            
            _database.Object.DeleteOnSubmit(story);

            _database.Verify(d => d.DeleteObject(story), Times.AtMostOnce());
        }
        
        [Fact]
        public void DeleteAllOnSubmit_Should_Call_DeleteOnSubmit()
        {
            var stories = new List<Story> {new Story(), new Story(), new Story(), new Story()};

            _database.Object.InsertAllOnSubmit(stories);

            _database.Object.DeleteAllOnSubmit(stories.AsQueryable());
            foreach (var story in stories)
            {
                Story entity = story;
                _database.Verify(e => e.DeleteOnSubmit(entity),Times.AtMostOnce());
            }
        }

        [Fact]
        public void GetQueryable_Should_Return_Empty_Set_When_Config_File_Is_Missing()
        {
            var database = new Database(_connectionString);

            Assert.NotNull(database.GetQueryable<Story>());
        }

        public void Dispose()
        {
            _database.Verify();
        }
    }
}