using System.Collections.Generic;
using Kigg.DomainObjects;
using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Repository;

    public class TagMatchSpamWeightCalculatorFixture
    {
        private const int MatchValue = -5;

        private readonly Mock<ITagRepository> _tagRepository;
        private readonly TagMatchSpamWeightCalculator _calculator;

        public TagMatchSpamWeightCalculatorFixture()
        {
            _tagRepository = new Mock<ITagRepository>();
            _calculator = new TagMatchSpamWeightCalculator(MatchValue, 10, _tagRepository.Object);

            SetupRepository();
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Value_When_Match_Found()
        {
            var value = _calculator.Calculate("ASPNETMVC and IoC/DI rocks together");

            Assert.Equal(MatchValue * 2, value);
        }

        [Fact]
        public void Calculate_Should_Return_Zero_When_No_Match_Is_Found()
        {
            var value = _calculator.Calculate("I dont know nothing but .net");

            Assert.Equal(0, value);
        }

        [Fact]
        public void Calculate_Should_Use_TagRepository()
        {
            _calculator.Calculate("This is dump text");

            _tagRepository.Verify();
        }

        private void SetupRepository()
        {
            var tags = new List<ITag>();

            var tag1 = new Mock<ITag>();
            tag1.SetupGet(t => t.Name).Returns("ASPNETMVC");

            var tag2 = new Mock<ITag>();
            tag2.SetupGet(t => t.Name).Returns("IoC/DI");

            tags.AddRange(new[] { tag1.Object, tag2.Object });

            _tagRepository.Setup(r => r.FindByUsage(It.IsAny<int>())).Returns(tags).Verifiable();
        }
    }
}