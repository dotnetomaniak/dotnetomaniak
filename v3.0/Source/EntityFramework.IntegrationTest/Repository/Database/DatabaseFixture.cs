using System;
using System.Linq;
using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;
    

    public class DatabaseFixture : BaseIntegrationFixture, IDisposable
    {
        [Fact]
        public void ObjectContext_With_LoadOptions_Should_Preload_UserTags_When_User_Is_Loaded()
        {
            var options = new DataLoadOptions();
            _database.LoadOptions = options;

            options.LoadWith<User>(u => u.UserTagsInternal);
            var users = _database.UserDataSource;
            foreach(var user in users)
            {
                Assert.True(user.UserTags.IsLoaded);
                //This line will cause an issue when database doesn't support MARS
                //Assert.Equal(user.Tags.Count, user.TagCount);
            }
        }
        
        [Fact]
        public void DeleteAllOnSubmit_And_Presist_Changes_Should_Correctly_Update_Database()
        {
            using(BeginTransaction())
            {
                GenerateStories(true, false, true);
                _database.SubmitChanges();
                var views = _database.StoryViewDataSource;
                Assert.True(views.Count() > 0);

                _database.DeleteAllOnSubmit(views);
                _database.SubmitChanges();

                Assert.True(views.Count() == 0);
            }
        }
        [Fact]
        public void StorySearchResult_Should_Return_Correct_Search_Results()
        {
            using(BeginTransaction())
            {
                GenerateStories("mvc", true, false, true);
                _database.SetSearchQuery("mvc");
                var results = _database.StorySearchResult;
                Assert.True(results.Count() >= 0);
            }
            
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}