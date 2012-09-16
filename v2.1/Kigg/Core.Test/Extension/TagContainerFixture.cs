using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;

    public class TagContainerFixture
    {
        private readonly Mock<ITagContainer> _tagContainer;

        public TagContainerFixture()
        {
            _tagContainer = new Mock<ITagContainer>();
        }

        [Fact]
        public void HasTags_Should_Be_True_When_TagCount_Is_Greater_Than_Zero()
        {
            _tagContainer.ExpectGet(c => c.TagCount).Returns(1);

            Assert.True(_tagContainer.Object.HasTags());
        }
    }
}