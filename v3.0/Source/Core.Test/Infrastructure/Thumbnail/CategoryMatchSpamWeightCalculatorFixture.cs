using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Repository;

    public class CategoryMatchSpamWeightCalculatorFixture
    {
        private const int MatchValue = -5;

        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly CategoryMatchSpamWeightCalculator _calculator;

        public CategoryMatchSpamWeightCalculatorFixture()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _calculator = new CategoryMatchSpamWeightCalculator(MatchValue, _categoryRepository.Object);

            SetupRepository();
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Value_When_Match_Found()
        {
            var value = _calculator.Calculate("Both C# and ASP.NET is Cool");

            Assert.Equal(MatchValue * 2, value);
        }

        [Fact]
        public void Calculate_Should_Return_Zero_When_No_Match_Is_Found()
        {
            var value = _calculator.Calculate("I dont know nothing but .net");

            Assert.Equal(0, value);
        }

        [Fact]
        public void Calculate_Should_Use_CategoryRepository()
        {
            _calculator.Calculate("This is dump text");

            _categoryRepository.Verify();
        }

        private void SetupRepository()
        {
            var categories = new List<ICategory>();

            var category1 = new Mock<ICategory>();
            category1.SetupGet(c => c.Name).Returns("C#");

            var category2 = new Mock<ICategory>();
            category2.SetupGet(c => c.Name).Returns("ASP.NET");

            categories.AddRange(new[] { category1.Object, category2.Object });

            _categoryRepository.Setup(r => r.FindAll()).Returns(categories).Verifiable();
        }
    }
}