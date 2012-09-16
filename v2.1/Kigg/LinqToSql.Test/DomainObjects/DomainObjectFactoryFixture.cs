using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;

    public class DomainObjectFactoryFixture
    {
        private readonly DomainObjectFactory _factory;

        public DomainObjectFactoryFixture()
        {
            _factory = new DomainObjectFactory();
        }

        [Fact]
        public void CreateUser_Should_Return_New_User()
        {
            var user = CreateUser();

            Assert.NotNull(user);
        }

        [Fact]
        public void CreateKnownSource_Should_Return_New_KnownSource()
        {
            var knownSource = _factory.CreateKnownSource("http://asp.net");

            Assert.NotNull(knownSource);
        }

        [Fact]
        public void CreateCategory_Should_Return_New_Category()
        {
            var category = CreateCategory();

            Assert.NotNull(category);
        }

        [Fact]
        public void CreateTag_Should_Return_New_Tag()
        {
            var tag = _factory.CreateTag("Dummy Tag");

            Assert.NotNull(tag);
        }

        [Fact]
        public void CreateStory_Should_Return_New_Story()
        {
            var story = _factory.CreateStory(CreateCategory(), CreateUser(), "192.168.0.1", "Dummy Story", "Dummy Description", "http://astory.com");

            Assert.NotNull(story);
        }

        private ICategory CreateCategory()
        {
            return _factory.CreateCategory("Dummy Category");
        }

        private IUser CreateUser()
        {
            return _factory.CreateUser("dummyUser", "dummy@users.com", "dummyPassword");
        }
    }
}