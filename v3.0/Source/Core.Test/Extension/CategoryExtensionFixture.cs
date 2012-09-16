using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;
    using Infrastructure.DomainRepositoryExtensions;
    using Kigg.Test.Infrastructure;

    public class CategoryExtensionFixture : BaseFixture
    {
        private readonly Mock<ICategory> _category;

        public CategoryExtensionFixture()
        {
            _category = new Mock<ICategory>();
        }
        [Fact]
        public void GetStoryCount_Should_Return_Correct_Value()
        {
            PrepareCountByCategory<IStoryRepository>(10);

            var i = _category.Object.GetStoryCount();

            Assert.True(i == 10);
        }

        private void PrepareCountByCategory<T>(int count) where T : class, IStoryRepository
        {
            var repository = new Mock<T>();

            resolver.Setup(r => r.Resolve<T>()).Returns(repository.Object);
            repository.Setup(r => r.CountByCategory(_category.Object.Id)).Returns(count);
        }
    }
}
