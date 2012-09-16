using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    public class BaseSpecificationFixture
    {
        private readonly Mock<BaseSpecification<IDummyObject>> _specification;

        public BaseSpecificationFixture()
        {
            _specification = new Mock<BaseSpecification<IDummyObject>>();
        }

        [Fact]
        public void And_Should_Return_New_AndSpecification()
        {
            var other = new Mock<BaseSpecification<IDummyObject>>();

            var and = _specification.Object.And(other.Object);

            Assert.NotNull(and);
        }

        [Fact]
        public void Or_Should_Return_New_OrSpecification()
        {
            var other = new Mock<BaseSpecification<IDummyObject>>();

            var or = _specification.Object.Or(other.Object);

            Assert.NotNull(or);
        }
    }
}