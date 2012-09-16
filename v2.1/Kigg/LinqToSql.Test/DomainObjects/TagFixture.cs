using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;

    public class TagFixture : LinqToSqlBaseFixture
    {
        private readonly Tag _tag;

        public TagFixture()
        {
            _tag = new Tag();
        }

        [Fact]
        public void StoryCount_Should_Use_Story_Repository()
        {
            storyRepository.Expect(r => r.CountByTag(It.IsAny<Guid>())).Returns(1);

            Assert.Equal(1, _tag.StoryCount);

            storyRepository.Verify();
        }
    }
}