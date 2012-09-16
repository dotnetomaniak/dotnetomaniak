using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using Kigg.LinqToSql.DomainObjects;

    public class CategoryFixture : LinqToSqlBaseFixture
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