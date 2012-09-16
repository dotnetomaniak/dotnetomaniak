using Xunit;

namespace Kigg.Core.Test
{
    public class LinqSpecificationFixture
    {
        private readonly LinqSpecification<string> _specfication;

        public LinqSpecificationFixture()
        {
            _specfication = new LinqSpecification<string>(s => s.Length == 1);
        }

        [Fact]
        public void IsStatisfiedBy_Should_Returns_Correct_Value()
        {
            Assert.True(_specfication.IsSatisfiedBy("a"));
        }
    }
}