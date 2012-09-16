using System;
using System.Linq;
using System.Data;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class TagRepositoryFixture : BaseIntegrationFixture
    {
        private readonly TagRepository _tagRepository;

        public TagRepositoryFixture()
        {   
            _tagRepository = new TagRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new TagRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var tag = (Tag)_domainFactory.CreateTag("Dummy Tag");
                _tagRepository.Add(tag);
                Assert.Equal(EntityState.Added, tag.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, tag.EntityState);
            }
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            using (BeginTransaction())
            {
                var tag = (Tag)_domainFactory.CreateTag("Dummy Category");
                _database.InsertOnSubmit(tag);
                _database.SubmitChanges();
                
                Assert.Throws<ArgumentException>(() => _tagRepository.Add(_domainFactory.CreateTag(tag.Name)));
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var tag = (Tag)_domainFactory.CreateTag("Dummy Tag");
                _database.InsertOnSubmit(tag);
                _database.SubmitChanges();

                _tagRepository.Remove(tag);
                Assert.Equal(EntityState.Deleted, tag.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, tag.EntityState);
            }
        }

        [Fact]
        public void FindById_Should_Return_Correct_Tag()
        {
            using (BeginTransaction())
            {
                var tag = (Tag) _domainFactory.CreateTag("Dummy Tag");
                _database.InsertOnSubmit(tag);
                _database.SubmitChanges();

                var id = tag.Id;
                var foundTag = _tagRepository.FindById(id);

                Assert.NotNull(foundTag);
                Assert.Equal(id, foundTag.Id);
            }
        }

        [Fact]
        public void FindByUniqueName_Should_Return_Correct_Tag()
        {
            using (BeginTransaction())
            {
                var tag = (Tag)_domainFactory.CreateTag("Dummy Tag");
                _database.InsertOnSubmit(tag);
                _database.SubmitChanges();

                var uniqueName = tag.UniqueName;
                var foundTag = _tagRepository.FindByUniqueName(uniqueName);

                Assert.NotNull(foundTag);
                Assert.Equal(uniqueName, foundTag.UniqueName);
            }
        }

        [Fact]
        public void FindByName_Should_Return_Correct_Tag()
        {
            using (BeginTransaction())
            {
                var tag = (Tag) _domainFactory.CreateTag("Dummy Tag");
                _database.InsertOnSubmit(tag);
                _database.SubmitChanges();
                var name = tag.Name;

                var foundTag = _tagRepository.FindByName(name);

                Assert.Equal(name, foundTag.Name);
            }
        }

        [Fact]
        public void FindMatching_Should_Return_Correct_Tags()
        {
            using (BeginTransaction())
            {
                _tagRepository.Add(_domainFactory.CreateTag("UniqueDemo 01"));
                _tagRepository.Add(_domainFactory.CreateTag("UniqueDemo 02"));
                _tagRepository.Add(_domainFactory.CreateTag("UniqueDemo 03"));
                _database.SubmitChanges();
                var tags = _tagRepository.FindMatching("UniqueDemo", 10);
                Assert.Equal(3, tags.Count);
            }
        }

        [Fact]
        public void FindByUsage_Should_Return_Top_Tags()
        {
            using(BeginTransaction())
            {
                GenerateStories(true, false, true);
                _database.SubmitChanges();

                var count = _database.TagDataSource
                .Where(t => t.StoriesInternal.Any())
                .OrderByDescending(t => t.StoriesInternal.Count(st => st.ApprovedAt != null))
                .ThenBy(t => t.Name)
                .Take(10).AsEnumerable().Count();

                var result = _tagRepository.FindByUsage(10);

                Assert.Equal(count, result.Count);
            }
            
        }

        [Fact]
        public void FindAll_Should_Return_All_Tag()
        {
            using (BeginTransaction())
            {
                GenerateStories(true, false, true);
                _database.SubmitChanges();

                var count = _database.TagDataSource.Count();

                var result = _tagRepository.FindAll();

                Assert.Equal(count, result.Count);
            }
            
        }
    }
}
