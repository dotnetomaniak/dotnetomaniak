using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.DomainObjects;

    class CategoryFixture : EfBaseFixture
    {
        private readonly Category _category;

        public CategoryFixture()
        {
            _category = new Category();
        }

        [Fact]
        public void StoryCount_Should_Use_Story_Repository()
        {
            storyRepository.Setup(r => r.CountByCategory(It.IsAny<Guid>())).Returns(1);

            Assert.Equal(1, _category.StoryCount);

            storyRepository.Verify();
        }
    }
}
