using Xunit;

namespace Kigg.Web.Test
{
    public class ValidationFixture
    {
        private readonly Validation _validation;

        public ValidationFixture()
        {
            _validation = new Validation(() => "AString".Length == 0, "String should not be blank");
        }

        [Fact]
        public void Constructor_Should_Not_Throw_Exception()
        {
            Assert.NotNull(_validation);
        }
    }
}