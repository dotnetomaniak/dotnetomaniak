using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using Kigg.LinqToSql.DomainObjects;
    using Kigg.LinqToSql.Repository;

    public class DatabaseFixture : IDisposable
    {
        private readonly Mock<Database> _database;

        public DatabaseFixture()
        {
            _database = new Mock<Database>("foo");
        }

        public void Dispose()
        {
            _database.Verify();
        }

        [Fact]
        public void CategoryDataSource_Should_Return_Category_Table()
        {
            _database.Setup(d => d.GetQueryable<Category>()).Returns(new List<Category>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.CategoryDataSource);
        }

        [Fact]
        public void TagDataSource_Should_Return_Tag_Table()
        {
            _database.Setup(d => d.GetQueryable<Tag>()).Returns(new List<Tag>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.TagDataSource);
        }

        [Fact]
        public void StoryDataSource_Should_Return_Story_Table()
        {
            _database.Setup(d => d.GetQueryable<Story>()).Returns(new List<Story>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.StoryDataSource);
        }

        [Fact]
        public void CommentDataSource_Should_Return_StoryComment_Table()
        {
            _database.Setup(d => d.GetQueryable<StoryComment>()).Returns(new List<StoryComment>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.CommentDataSource);
        }

        [Fact]
        public void VoteDataSource_Should_Return_StoryVote_Table()
        {
            _database.Setup(d => d.GetQueryable<StoryVote>()).Returns(new List<StoryVote>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.VoteDataSource);
        }

        [Fact]
        public void MarkAsSpamDataSource_Should_Return_StoryMarkAsSpam_Table()
        {
            _database.Setup(d => d.GetQueryable<StoryMarkAsSpam>()).Returns(new List<StoryMarkAsSpam>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.MarkAsSpamDataSource);
        }

        [Fact]
        public void StoryTagDataSource_Should_Return_StoryTag_Table()
        {
            _database.Setup(d => d.GetQueryable<StoryTag>()).Returns(new List<StoryTag>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.StoryTagDataSource);
        }

        [Fact]
        public void StoryViewDataSource_Should_Return_StoryView_Table()
        {
            _database.Setup(d => d.GetQueryable<StoryView>()).Returns(new List<StoryView>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.StoryViewDataSource);
        }

        [Fact]
        public void UserTagDataSource_Should_Return_UserTag_Table()
        {
            _database.Setup(d => d.GetQueryable<UserTag>()).Returns(new List<UserTag>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.UserTagDataSource);
        }

        [Fact]
        public void UserDataSource_Should_Return_User_Table()
        {
            _database.Setup(d => d.GetQueryable<User>()).Returns(new List<User>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.UserDataSource);
        }

        [Fact]
        public void UserScoreDataSource_Should_Return_UserScore_Table()
        {
            _database.Setup(d => d.GetQueryable<UserScore>()).Returns(new List<UserScore>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.UserScoreDataSource);
        }

        [Fact]
        public void CommentSubscribtionDataSource_Should_Return_CommentSubscribtion_Table()
        {
            _database.Setup(d => d.GetQueryable<CommentSubscribtion>()).Returns(new List<CommentSubscribtion>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.CommentSubscribtionDataSource);
        }

        [Fact]
        public void KnownSourceDataSource_Should_Return_KnownSource_Table()
        {
            _database.Setup(d => d.GetQueryable<KnownSource>()).Returns(new List<KnownSource>().AsQueryable()).Verifiable();

            Assert.NotNull(_database.Object.KnownSourceDataSource);
        }

        [Fact]
        public void Insert_Should_Call_GetEditable_And_InsertOnSubmit()
        {
            var editable = new Mock<ITable>();
            var story = new Story();

            _database.Setup(d => d.GetEditable<Story>()).Returns(editable.Object).Verifiable();
            editable.Setup(e => e.InsertOnSubmit(story)).Verifiable();

            _database.Object.Insert(story);
            editable.Verify();
        }

        [Fact]
        public void InsertAll_Should_Call_GetEditable_And_InsertAllOnSubmit()
        {
            var editable = new Mock<ITable>();
            var stories = new List<Story>();

            _database.Setup(d => d.GetEditable<Story>()).Returns(editable.Object).Verifiable();
            editable.Setup(e => e.InsertAllOnSubmit(stories)).Verifiable();

            _database.Object.InsertAll(stories);
            editable.Verify();
        }

        [Fact]
        public void Delete_Should_Call_GetEditable_And_DeleteOnSubmit()
        {
            var editable = new Mock<ITable>();
            var story = new Story();

            _database.Setup(d => d.GetEditable<Story>()).Returns(editable.Object).Verifiable();
            editable.Setup(e => e.DeleteOnSubmit(story)).Verifiable();

            _database.Object.Delete(story);
            editable.Verify();
        }

        [Fact]
        public void DeleteAll_Should_Calls_GetEditable_And_DeleteAllOnSubmit()
        {
            var editable = new Mock<ITable>();
            var stories = new List<Story>();

            _database.Setup(d => d.GetEditable<Story>()).Returns(editable.Object).Verifiable();
            editable.Setup(e => e.DeleteAllOnSubmit(stories)).Verifiable();

            _database.Object.DeleteAll(stories);
            editable.Verify();
        }

        [Fact]
        public void GetQueryable_Should_Return_Empty_Table_When_Config_File_Is_Missing()
        {
            var database = new Database("foo");

            Assert.NotNull(database.GetQueryable<Story>());
        }

        [Fact]
        public void GetEditable_Should_Return_Empty_Table_When_Config_File_Is_Missing()
        {
            var database = new Database("foo");

            Assert.NotNull(database.GetEditable<Story>());
        }
    }
}