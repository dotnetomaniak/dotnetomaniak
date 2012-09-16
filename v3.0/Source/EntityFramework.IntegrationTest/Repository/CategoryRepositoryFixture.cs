using System;
using System.Linq;
using System.Data;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;
    
    public class CategoryRepositoryFixture : BaseIntegrationFixture
    {
        private readonly CategoryRepository _categoryRepository;
        
        public CategoryRepositoryFixture()
        {
            _categoryRepository = new CategoryRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new CategoryRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var category = (Category) _domainFactory.CreateCategory("Dummy Category");
                _categoryRepository.Add(category);
                
                Assert.Equal(EntityState.Added, category.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, category.EntityState);
            
            }
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            using(BeginTransaction())
            {
                var category = CreateNewCategory("AddCategoryTest");
                _database.InsertOnSubmit(category);
                _database.SubmitChanges();
                Assert.Throws<ArgumentException>(() => _categoryRepository.Add(_domainFactory.CreateCategory(category.Name)));
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var category = CreateNewCategory("DeleteCategoryTest");
                _database.InsertOnSubmit(category);
                _database.SubmitChanges();
                
                _categoryRepository.Remove(category);
                Assert.Equal(EntityState.Deleted, category.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, category.EntityState);
            }
        }

        [Fact]
        public void FindById_Should_Return_Correct_Category()
        {
            using (BeginTransaction())
            {
                var newCategory = CreateNewCategory("FindByIdTest");
                _database.InsertOnSubmit(newCategory);
                _database.SubmitChanges();

                var category = _categoryRepository.FindById(newCategory.Id);
                Assert.Equal(newCategory.Id, category.Id);
            }
        }

        [Fact]
        public void FindByUniqueName_Should_Return_Correct_Category()
        {
            using (BeginTransaction())
            {
                var newCategory = CreateNewCategory("FindByUniqueNameTest");
                _database.InsertOnSubmit(newCategory);
                _database.SubmitChanges();

                var category = _categoryRepository.FindByUniqueName(newCategory.UniqueName);
                Assert.Equal(newCategory.Id, category.Id);
                Assert.Equal(category.UniqueName, category.UniqueName);
            }
        }

        [Fact]
        public void FindAll_Should_Return_All_Category()
        {
            using (BeginTransaction())
            {
                _database.InsertOnSubmit(CreateNewCategory("FindAllTest1"));
                _database.InsertOnSubmit(CreateNewCategory("FindAllTest2"));
                _database.SubmitChanges();

                int count = _database.CategoryDataSource.Count();

                var result = _categoryRepository.FindAll();

                Assert.Equal(count, result.Count);
            }
        }
    }
}

    