using System;
using System.Data;
using System.Linq;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using DomainObjects;
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;
    
    public class UserFixture : BaseIntegrationFixture
    {

        [Fact]
        public void Tags_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            var user = CreateNewUser();
            Assert.Empty(user.Tags);
        }

        [Fact]
        public void TagCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            var user = CreateNewUser();
            Assert.Equal(0, user.TagCount);
        }

        [Fact]
        public void TagCount_Should_Return_Correct_Count_For_Existing_User()
        {
            using (BeginTransaction())
            {
                var options = new DataLoadOptions();
                options.LoadWith<User>(u => u.UserTagsInternal);
                _database.LoadOptions = options;
                CreateNewUserAndSave();

                var user = _database.UserDataSource.First();

                Assert.Equal(user.Tags.Count, user.TagCount);
            }
        }

        [Fact]
        public void IncreaseScoreBy_For_New_Instance_Should_Add_New_Item_In_UserScore_Collection()
        {
            var user = CreateNewUser();

            user.IncreaseScoreBy(10, UserAction.AccountActivated);

            Assert.Equal(1, user.UserScoreInternal.Count);
        }

        [Fact]
        public void DecreaseScoreBy_For_New_Instance_Should_Add_New_Item_In_UserScore_Collection()
        {
            var user = CreateNewUser();

            user.DecreaseScoreBy(20, UserAction.SpamStorySubmitted);

            Assert.Equal(1, user.UserScoreInternal.Count);
        }

        [Fact]
        public void IncreaseScoreBy_For_Existing_User_Should_Add_New_Item_In_UserScore_Collection()
        {
            using(BeginTransaction())
            {
                CreateNewUserAndSave();
                var user = _database.UserDataSource.First();

                user.IncreaseScoreBy(10, UserAction.AccountActivated);

                Assert.Equal(1, user.UserScoreInternal.Count);    
            }
        }

        [Fact]
        public void DecreaseScoreBy_For_Existing_User_Should_Add_New_Item_In_UserScore_Collection()
        {
            using (BeginTransaction())
            {
                CreateNewUserAndSave();
                var user = _database.UserDataSource.First();

                user.DecreaseScoreBy(20, UserAction.SpamStorySubmitted);

                Assert.Equal(1, user.UserScoreInternal.Count);
            }
        }

        [Fact]
        public void AddTag_For_New_Instance_Should_Increase_Tags_Collection()
        {
            var user = CreateNewUser();

            user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.Equal(1, user.Tags.Count);
        }

        [Fact]
        public void RemoveTag_For_New_Instance_Should_Decrease_Tags_Collection()
        {
            var user = CreateNewUser();

            user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.Equal(1, user.Tags.Count);

            user.RemoveTag(new Tag { Name = "Dummy" });

            Assert.Equal(0, user.Tags.Count);
        }

        [Fact]
        public void AddTag_For_Existing_User_Should_Increase_Tags_Collection()
        {
            using (BeginTransaction())
            {
                var options = new DataLoadOptions();
                options.LoadWith<User>(u => u.UserTagsInternal);
                _database.LoadOptions = options;

                CreateNewUserAndSave();

                var user = _database.UserDataSource.First();
                int tagsCount = user.Tags.Count;
                user.AddTag(new Tag {Id = Guid.NewGuid(), Name = "Dummy"});

                Assert.Equal(tagsCount + 1, user.Tags.Count);
            }
        }

        [Fact]
        public void RemoveTag_For_Existing_User_Should_Decrease_Tags_Collection()
        {
            using (BeginTransaction())
            {
                var options = new DataLoadOptions();
                options.LoadWith<User>(u => u.UserTagsInternal);
                _database.LoadOptions = options;

                CreateNewUserAndSave();
                
                var user = _database.UserDataSource.First();
                var tagsCount = user.Tags.Count;
                user.AddTag(new Tag {Id = Guid.NewGuid(), Name = "Dummy"});

                Assert.Equal(tagsCount + 1, user.Tags.Count);

                user.RemoveTag(new Tag {Name = "Dummy"});

                Assert.Equal(tagsCount, user.Tags.Count);
            }
        }

        [Fact]
        public void ContainsTag_For_New_Instance_Should_Return_True_When_Tag_Exists_In_Tags_Collection()
        {
            var user = CreateNewUser();

            user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.True(user.ContainsTag(new Tag { Name = "Dummy" }));
        }

        [Fact]
        public void ContainsTag_For_Existing_User_Should_Return_True_When_Tag_Exists_In_Preloaded_Tags_Collection()
        {
            using (BeginTransaction())
            {

                var options = new DataLoadOptions();
                options.LoadWith<User>(u => u.UserTagsInternal);
                _database.LoadOptions = options;
                CreateNewUserAndSave();

                var user = _database.UserDataSource.First();
                var tag = user.UserTagsInternal.First();

                Assert.True(user.ContainsTag(new Tag {Name = tag.Name}));
            }
        }

        [Fact]
        public void ContainsTag_For_Existing_User_Should_Return_True_When_Tag_Exists_In_Lazy_Loaded_Tags_Collection()
        {
            using (BeginTransaction())
            {
                CreateNewUserAndSave();
                var user = _database.UserDataSource.First();
                var tag = user.UserTagsInternal.CreateSourceQuery().First();

                Assert.True(user.ContainsTag(new Tag {Name = tag.Name}));
            }
        }

        [Fact]
        public void Create_New_User_And_Insert_It_Should_Presist_User_In_Database()
        {
            using(BeginTransaction())
            {
                var user = CreateNewUser();
                _database.InsertOnSubmit(user);

                _database.SubmitChanges();

                Assert.True(user.EntityState == EntityState.Unchanged);

                Assert.NotNull(_database.UserDataSource.FirstOrDefault(u => u.UserName == user.UserName));
            }
        }

        [Fact]
        public void AddTag_And_Submit_Changes_Should_Presist_User_Tag_Association_In_Database()
        {
            using (BeginTransaction())
            {
                CreateNewUserAndSave();
                var user = _database.UserDataSource.First();
                user.AddTag(CreateNewTag("anotherdummytag"));

                _database.SubmitChanges();

                var tag = (Tag)user.Tags.First(t => t.Name == "anotherdummytag");
                Assert.NotNull(tag);
                Assert.True(tag.EntityState == EntityState.Unchanged);
            }

        }

        [Fact]
        public void RemoveTag_And_Submit_Changes_Should_Remove_User_Tag_Association_In_Database()
        {
            using (BeginTransaction())
            {
                var dataLoadOptions = new DataLoadOptions();
                dataLoadOptions.LoadWith<Tag>(t => t.UsersInternal);
                _database.LoadOptions = dataLoadOptions;
                
                CreateNewUserAndSave();

                var user = _database.UserDataSource.First();
                var tag = user.UserTagsInternal.CreateSourceQuery().First();
                user.RemoveTag(tag);

                _database.SubmitChanges();
                
                var deletedTag = _database.TagDataSource.FirstOrDefault(t => t.Name == tag.Name);
                
                Assert.Null(deletedTag.UsersInternal.FirstOrDefault(u => u.UserName == user.UserName));
            }
        }

        [Fact]
        public void RemoveAllTags_And_Submit_Changes_Should_Remove_All_User_Tag_Associations_In_Database()
        {
            
            using (BeginTransaction())
            {
                var dataLoadOptions = new DataLoadOptions();
                dataLoadOptions.LoadWith<Tag>(t => t.UsersInternal);
                _database.LoadOptions = dataLoadOptions;
                
                CreateNewUserAndSave();
                
                var user = _database.UserDataSource.First();
            
                var tags = _database.TagDataSource.Where(t => t.UsersInternal.Any(u => u.UserName == user.UserName));
                
                Assert.True(tags.Count()>0);
                
                user.RemoveAllTags();

                _database.SubmitChanges();

                Assert.Equal(0, tags.Count());
            }
        }
        
        private void CreateNewUserAndSave()
        {
            var user = CreateNewUser();
            user.AddTag(CreateNewTag());
            _database.InsertOnSubmit(user);
            _database.SubmitChanges();
        }
        //private static Tag CraeteTag()
        //{
        //    return Tag.CreateTag(Guid.NewGuid(), "DummyTagUniqueName", "DummyTag", SystemTime.Now());
        //}
        //private static User CreateUser()
        //{
        //    return User.CreateUser(Guid.NewGuid(), "dummy", "dummy@mail.com",
        //                                false, false, SystemTime.Now(), SystemTime.Now());
        //}
    }
}
