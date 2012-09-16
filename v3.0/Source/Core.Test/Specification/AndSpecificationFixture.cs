using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    public class AndSpecificationFixture
    {
        private readonly Mock<ISpecification<IDummyObject>> _leftHandSide;
        private readonly Mock<ISpecification<IDummyObject>> _rightHandSide;

        private readonly AndSpecification<IDummyObject> _specificaton;

        public AndSpecificationFixture()
        {
            _leftHandSide = new Mock<ISpecification<IDummyObject>>();
            _rightHandSide = new Mock<ISpecification<IDummyObject>>();

            _specificaton = new AndSpecification<IDummyObject>(_leftHandSide.Object, _rightHandSide.Object);
        }

        [Fact]
        public void IsStatisfiedBy_Should_Returns_True_When_Both_Left_And_Right_Is_True()
        {
            _leftHandSide.Expect(s => s.IsSatisfiedBy(It.IsAny<IDummyObject>())).Returns(true);
            _rightHandSide.Expect(s => s.IsSatisfiedBy(It.IsAny<IDummyObject>())).Returns(true);

            var candidate = new Mock<IDummyObject>();
            var result = _specificaton.IsSatisfiedBy(candidate.Object);

            Assert.True(result);
        }
    }
}