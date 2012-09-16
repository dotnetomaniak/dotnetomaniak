#if(MySql)
using System;
using System.Linq;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    public class DatabaseFactoryFixture : BaseIntegrationFixture, IDisposable
    {
        
        private readonly DatabaseFactory _mySqlfactory;
     
        public DatabaseFactoryFixture()
        {
        
            var mySqlConnString = new ConnectionString(new ConfigurationManagerWrapper(), "KiGGMySql", ".\\EDM", "DomainObjects.MySql");
        
            _mySqlfactory = new DatabaseFactory(mySqlConnString);
        }
        
        [Fact]
        public void Get_Should_Return_Correctly_Initialized_Database()
        {
            using (var db = _mySqlfactory.Create())
            {
                using (BeginTransaction((Database) db))
                {
                    db.InsertOnSubmit(GenerateStoryGraph());
                    db.SubmitChanges();
                    var story = db.StoryDataSource.First();

                    var vote = db.VoteDataSource.First();

                    var comment = db.CommentDataSource.First();

                    Assert.True(story.UserReference.IsLoaded);
                    Assert.True(story.CategoryReference.IsLoaded);
                    Assert.True(story.StoryTagsInternal.IsLoaded);

                    Assert.True(vote.UserReference.IsLoaded);

                    Assert.True(comment.UserReference.IsLoaded);
                }
            }
        }

        public void Dispose()
        {
            _mySqlfactory.Dispose();
        }
    }
}
#endif