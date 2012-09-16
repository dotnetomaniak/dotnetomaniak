using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;
    using Kigg.LinqToSql.Repository;
    using Kigg.LinqToSql.DomainObjects;

    public class CategoryRepositoryFixture : LinqToSqlBaseFixture
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IDomainObjectFactory _factory;

        public CategoryRepositoryFixture()
        {
            _factory = new DomainObjectFactory();
            _categoryRepository = new CategoryRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new CategoryRepository(databaseFactory.Object));
        }

        [Fact]
        public void Add_Should_Use_Database()
        {
            database.Setup(d => d.Insert(It.IsAny<Category>())).Verifiable();

            _categoryRepository.Add(_factory.CreateCategory("Dummy Category"));
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            Categories.Add(_factory.CreateCategory("Demo") as Category);

            Assert.Throws<ArgumentException>(() => _categoryRepository.Add(_factory.CreateCategory("Demo")));
        }

        [Fact]
        public void Remove_Should_Use_Database()
        {
            Categories.Add(_factory.CreateCategory("Demo") as Category);

            database.Setup(d => d.Delete(It.IsAny<Category>())).Verifiable();

            _categoryRepository.Remove(Categories[0]);

            database.Verify();
        }

        [Fact]
        public void FindById_Should_Return_Correct_Category()
        {
            Categories.Add(_factory.CreateCategory("Demo") as Category);

            var id = Categories[0].Id;
            var category = _categoryRepository.FindById(id);

            Assert.Equal(id, category.Id);
        }

        [Fact]
        public void FindByUniqueName_Should_Return_Correct_Category()
        {
            Categories.Add(_factory.CreateCategory("Demo") as Category);

            var uniqueName = Categories[0].UniqueName;
            var category = _categoryRepository.FindByUniqueName(uniqueName);

            Assert.Equal(uniqueName, category.UniqueName);
        }

        [Fact]
        public void FindAll_Should_Return_All_Category()
        {
            Categories.Add(_factory.CreateCategory("Demo") as Category);

            var result = _categoryRepository.FindAll();

            Assert.Equal(1, result.Count);
        }
    }
}