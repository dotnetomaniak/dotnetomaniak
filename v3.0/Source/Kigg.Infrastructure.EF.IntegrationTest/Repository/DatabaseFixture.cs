using System;

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
                Assert.Equal(user.Tags.Count, user.TagCount);
            }
        }
        
        public void Dispose()
        {
            _database.Dispose();
        }
    }
}