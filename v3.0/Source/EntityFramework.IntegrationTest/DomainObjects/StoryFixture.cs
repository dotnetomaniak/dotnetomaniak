using System;
using System.Data;
using System.Linq;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;

    public class StoryFixture : BaseIntegrationFixture
    {
        [Fact]
        public void Tags_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            var story = CreateNewStory();
            Assert.Empty(story.Tags);
        }
        
        [Fact]
        public void TagCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            var story = CreateNewStory();
            Assert.Equal(0, story.TagCount);
        }
        
        [Fact]
        public void TagCount_Should_Return_Correct_Count_For_Existing_Story()
        {
            using(BeginTransaction())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Story>(s => s.StoryTagsInternal);
                _database.LoadOptions = options;

                GenerateStoriesAndSave();
                _database.SubmitChanges();
                var story = _database.StoryDataSource.First();
                Assert.Equal(story.Tags.Count, story.TagCount);    
            }
        }

        [Fact]
        public void AddTag_For_New_Instance_Should_Increase_Tags_Collection()
        {
            var story = CreateNewStory();

            story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.Equal(1, story.Tags.Count);
        }

        [Fact]
        public void RemoveTag_For_New_Instance_Should_Decrease_Tags_Collection()
        {
            var story = CreateNewStory();

            story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.Equal(1, story.Tags.Count);

            story.RemoveTag(new Tag { Name = "Dummy" });

            Assert.Equal(0, story.Tags.Count);
        }

        [Fact]
        public void AddTag_For_Existing_Story_Should_Increase_Tags_Collection()
        {
            using (BeginTransaction())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Story>(s => s.StoryTagsInternal);
                _database.LoadOptions = options;

                GenerateStoriesAndSave();
                _database.SubmitChanges();
                var story = _database.StoryDataSource.First();
                var tagsCount = story.Tags.Count;
                story.AddTag(new Tag {Id = Guid.NewGuid(), Name = "Dummy"});

                Assert.Equal(tagsCount + 1, story.Tags.Count);
            }
        }

        [Fact]
        public void RemoveTag_For_Existing_Story_Should_Decrease_Tags_Collection()
        {
            using(BeginTransaction())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Story>(s => s.StoryTagsInternal);
                _database.LoadOptions = options;
                
                GenerateStoriesAndSave();
                _database.SubmitChanges();
                var story = _database.StoryDataSource.First();
                var tagsCount = story.Tags.Count;
                story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

                Assert.Equal(tagsCount + 1, story.Tags.Count);

                story.RemoveTag(new Tag { Name = "Dummy" });

                Assert.Equal(tagsCount, story.Tags.Count);
            }
            
        }

        [Fact]
        public void ContainsTag_For_New_Instance_Should_Return_True_When_Tag_Exists_In_Tags_Collection()
        {
            var story = CreateNewStory();

            story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.True(story.ContainsTag(new Tag { Name = "Dummy" }));
        }

        [Fact]
        public void ContainsTag_For_Existing_Story_Should_Return_True_When_Tag_Exists_In_Preloaded_Tags_Collection()
        {
            using(BeginTransaction())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Story>(s => s.StoryTagsInternal);
                _database.LoadOptions = options;
                GenerateStoriesAndSave();
                _database.SubmitChanges();
                var story = _database.StoryDataSource.First();
                var tag = story.StoryTagsInternal.First();

                Assert.True(story.ContainsTag(new Tag { Name = tag.Name }));
            }            
        }

        [Fact]
        public void ContainsTag_For_Existing_Story_Should_Return_True_When_Tag_Exists_In_Lazy_Loaded_Tags_Collection()
        {
            using (BeginTransaction())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Story>(s => s.StoryTagsInternal);
                _database.LoadOptions = options;
                GenerateStoriesAndSave();
                _database.SubmitChanges();
                var story = _database.StoryDataSource.First();
                var tag = story.StoryTagsInternal.CreateSourceQuery().First();

                Assert.True(story.ContainsTag(new Tag {Name = tag.Name}));
            }
        }

        [Fact]
        public void Create_New_Story_And_Insert_It_Should_Presist_Story_In_Database()
        {
            using (BeginTransaction())
            {
                GenerateStoriesAndSave();
                _database.SubmitChanges();
                var user = _database.UserDataSource.First();
                var category = _database.CategoryDataSource.First();
                var story = CreateNewStory(category, user, "127.0.0.1", "dummy", "dummy",
                                           "http://www.dummy.com/dummy.aspx");
                var tag = (Tag) _domainFactory.CreateTag("dummy-tag");

                story.AddTag(tag);
                _database.InsertOnSubmit(story);

                Assert.True(story.EntityState == EntityState.Added);

                _database.SubmitChanges();

                Assert.True(story.EntityState == EntityState.Unchanged);
                Assert.True(tag.EntityState == EntityState.Unchanged);

                Assert.NotNull(_database.StoryDataSource.FirstOrDefault(s => s.Id == story.Id));
            }
        }

        [Fact]
        public void AddTag_And_Submit_Changes_Should_Presist_Story_Tag_Association_In_Database()
        {
            using (BeginTransaction())
            {
                var story = CreateNewStory();
                story.AddTag(CreateNewTag());
                _database.InsertOnSubmit(story);
                _database.SubmitChanges();

                var tag = (Tag)story.Tags.First(t => t.Name == "DummyTag");
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
                dataLoadOptions.LoadWith<Tag>(t => t.StoriesInternal);
                _database.LoadOptions = dataLoadOptions;
                
                GenerateStoriesAndSave();
                _database.SubmitChanges();
                var story = _database.StoryDataSource.First();
                var tag = story.StoryTagsInternal.CreateSourceQuery().First();
                
                story.RemoveTag(tag);

                _database.SubmitChanges();

                var deletedTag = _database.TagDataSource.FirstOrDefault(t => t.Name == tag.Name);

                Assert.Null(deletedTag.StoriesInternal.FirstOrDefault(s => s.Id==story.Id));
            }
        }

        [Fact]
        public void RemoveAllTags_And_Submit_Changes_Should_Remove_All_User_Tag_Associations_In_Database()
        {
            using (BeginTransaction())
            {
                var dataLoadOptions = new DataLoadOptions();
                dataLoadOptions.LoadWith<Tag>(t => t.StoriesInternal);
                _database.LoadOptions = dataLoadOptions;
                GenerateStoriesAndSave();
                _database.SubmitChanges();

                var story = _database.StoryDataSource.First();

                var tags = _database.TagDataSource.Where(t => t.StoriesInternal.Any(s => s.Id == story.Id));

                Assert.True(tags.Count() > 0);

                story.RemoveAllTags();

                _database.SubmitChanges();

                Assert.Equal(0, tags.Count());
            }
        }

        [Fact]
        public void ChangeCategory_And_Presist_Story_Should_Preserve_Relation_With_New_Category_And_Delete_Relation_With_Old_One()
        {
            using (BeginTransaction())
            {
                GenerateStoriesAndSave();
                _database.SubmitChanges();
                var story = _database.StoryDataSource.First();
                var category = _database.CategoryDataSource.First(c => c.Stories.Any(s => s.Id == story.Id));

                var newCategory = CreateNewCategory();
                story.ChangeCategory(newCategory);

                _database.SubmitChanges();

                Assert.Null(category.Stories.CreateSourceQuery().FirstOrDefault(s=>s.Id == story.Id));
                Assert.NotNull(newCategory.Stories.CreateSourceQuery().FirstOrDefault(s => s.Id == story.Id));

            }
        }

        [Fact]
        public void SubscribeComment_And_Presist_Story_Should_Preserve_User_Subscription_For_The_Story()
        {
            using (BeginTransaction())
            {
                GenerateStoriesAndSave();
                var user = _database.UserDataSource.First();

                var story = _database.StoryDataSource.First();

                var srcQuery = story.CommentSubscribersInternal.CreateSourceQuery();
                
                var count = story.CommentSubscribersInternal.Count;
                var realCount = srcQuery.Count();

                story.SubscribeComment(user);
                Assert.Equal(count+1,story.CommentSubscribersInternal.Count);


                _database.SubmitChanges();

                Assert.Equal(realCount+1,srcQuery.Count());
            }
        }

        [Fact]
        public void ContainsCommentSubscriber_Should_Return_True_When_User_Is_Subscribed_For_The_Story()
        {
            using (BeginTransaction())
            {
                GenerateStoriesAndSave();
                var user = _database.UserDataSource.First();

                var story = _database.StoryDataSource.First();
                story.SubscribeComment(user);
                _database.SubmitChanges();
                story = _database.StoryDataSource.First();
                Assert.True(story.ContainsCommentSubscriber(user));
            }
        }

        [Fact]
        public void UnsubscribeComment_And_Presist_Story_Should_Preserve_User_Unsubscription_From_The_Story()
        {
            using (BeginTransaction())
            {
                GenerateStoriesAndSave();
                var user = _database.UserDataSource.First();

                var story = _database.StoryDataSource.First();
                var srcQuery = story.CommentSubscribersInternal.CreateSourceQuery();
                var realCount = srcQuery.Count();

                story.SubscribeComment(user);
                
                _database.SubmitChanges();

                story = _database.StoryDataSource.First();
                story.UnsubscribeComment(user);
                
                _database.SubmitChanges();
                
                Assert.Equal(realCount, srcQuery.Count());
            }            
        }

        private void GenerateStoriesAndSave()
        {
            GenerateStories(true, false, true);
            _database.SubmitChanges();
        }
    }
}
