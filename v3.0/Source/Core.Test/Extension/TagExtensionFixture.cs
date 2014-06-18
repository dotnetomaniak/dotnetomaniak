using Kigg.DomainObjects;
using Kigg.Infrastructure.DomainRepositoryExtensions;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Repository;
    using Kigg.Test.Infrastructure;

    public class TagExtensionFixture : BaseFixture
    {
        private readonly Mock<ITag> _tag;

        public TagExtensionFixture()
        {
            _tag = new Mock<ITag>();
        }
        [Fact]
        public void GetStoryCount_Should_Return_Correct_Value()
        {
            PrepareCountByTag<IStoryRepository>(10);

            var i = _tag.Object.GetStoryCount();

            Assert.True(i == 10);
        }

        private void PrepareCountByTag<T>(int count) where T : class, IStoryRepository
        {
            var repository = new Mock<T>();

            resolver.Setup(r => r.Resolve<T>()).Returns(repository.Object);
            repository.Setup(r => r.CountByTag(_tag.Object.Id)).Returns(count);
        }
    }
}
