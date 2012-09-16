using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    public class OrSpecificationFixture
    {
        private readonly Mock<ISpecification<IDummyObject>> _leftHandSide;
        private readonly Mock<ISpecification<IDummyObject>> _rightHandSide;

        private readonly OrSpecification<IDummyObject> _specificaton;

        public OrSpecificationFixture()
        {
            _leftHandSide = new Mock<ISpecification<IDummyObject>>();
            _rightHandSide = new Mock<ISpecification<IDummyObject>>();

            _specificaton = new OrSpecification<IDummyObject>(_leftHandSide.Object, _rightHandSide.Object);
        }

        [Fact]
        public void IsStatisfiedBy_Should_Return_True_When_Either_Left_Or_Right_Is_True()
        {
            _leftHandSide.Expect(s => s.IsSatisfiedBy(It.IsAny<IDummyObject>())).Returns(true);
            _rightHandSide.Expect(s => s.IsSatisfiedBy(It.IsAny<IDummyObject>())).Returns(false);

            var candidate = new Mock<IDummyObject>();
            var result = _specificaton.IsSatisfiedBy(candidate.Object);

            Assert.True(result);
        }
    }
}