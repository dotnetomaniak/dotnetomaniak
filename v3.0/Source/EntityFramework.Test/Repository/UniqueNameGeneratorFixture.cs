using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using DomainObjects;
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;

    public class UniqueNameGeneratorFixture
    {
        private readonly IDomainObjectFactory _factory;

        public UniqueNameGeneratorFixture()
        {
            _factory = new DomainObjectFactory();
        }

        [Fact]
        public void Generate_Should_Return_Correct_UniqueName_When_DataSource_Is_Empty()
        {
            var database = new Mock<IDatabase>();
            database.SetupGet(d => d.CategoryDataSource).Returns(new List<Category>().AsQueryable());

            string uniqueName = UniqueNameGenerator.GenerateFrom(database.Object.CategoryDataSource, "C");

            Assert.Equal("C", uniqueName);
        }

        [Fact]
        public void Generate_Should_Return_Correct_UniqueName_When_DataSource_Contains_One_Item()
        {
            var categoryList = new List<Category>();

            var database = new Mock<IDatabase>();
            database.SetupGet(d => d.CategoryDataSource).Returns(categoryList.AsQueryable());

            var category = (Category)_factory.CreateCategory("C");
            category.UniqueName = UniqueNameGenerator.GenerateFrom(database.Object.CategoryDataSource, category.Name);
            categoryList.Add(category);
            Thread.Sleep(500);

            string uniqueName = UniqueNameGenerator.GenerateFrom(database.Object.CategoryDataSource, "C++");

            Assert.Equal("C-2", uniqueName);
        }

        [Fact]
        public void Generate_Should_Return_Correct_UniqueName_When_DataSource_Contains_Two_Item()
        {
            var categoryList = new List<Category>();

            var database = new Mock<IDatabase>();
            database.SetupGet(d => d.CategoryDataSource).Returns(categoryList.AsQueryable());

            var category1 = (Category)_factory.CreateCategory("C");
            category1.UniqueName = UniqueNameGenerator.GenerateFrom(database.Object.CategoryDataSource, category1.Name);
            categoryList.Add(category1);
            Thread.Sleep(500);

            var category2 = (Category)_factory.CreateCategory("C++");
            category2.UniqueName = UniqueNameGenerator.GenerateFrom(database.Object.CategoryDataSource, category2.Name);
            categoryList.Add(category2);
            Thread.Sleep(500);

            string uniqueName = UniqueNameGenerator.GenerateFrom(database.Object.CategoryDataSource, "C#");

            Assert.Equal("C-3", uniqueName);
        }

        [Fact]
        public void Generate_Should_Return_Correct_UniqueName_When_Second_Name_Starts_With_First_Entity_Name()
        {
            var categoryList = new List<Category>();

            var database = new Mock<IDatabase>();
            database.SetupGet(d => d.CategoryDataSource).Returns(categoryList.AsQueryable());

            var category1 = (Category)_factory.CreateCategory("IoC-DI");
            category1.UniqueName = UniqueNameGenerator.GenerateFrom(database.Object.CategoryDataSource, category1.Name);
            categoryList.Add(category1);
            Thread.Sleep(500);

            string uniqueName = UniqueNameGenerator.GenerateFrom(database.Object.CategoryDataSource, "IoC");

            Assert.Equal("IoC", uniqueName);
        }
    }
}