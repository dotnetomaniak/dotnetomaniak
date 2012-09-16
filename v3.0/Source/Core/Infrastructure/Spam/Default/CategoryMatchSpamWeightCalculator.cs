namespace Kigg.Infrastructure
{
    using DomainObjects;
    using Repository;

    public class CategoryMatchSpamWeightCalculator : ISpamWeightCalculator
    {
        private readonly int _matchValue;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryMatchSpamWeightCalculator(int matchValue, ICategoryRepository categoryRepository)
        {
            Check.Argument.IsNotNull(categoryRepository, "categoryRepository");

            _matchValue = matchValue;
            _categoryRepository = categoryRepository;
        }

        public int Calculate(string content)
        {
            int total = 0;

            content = content.StripHtml().ToUpperInvariant();

            foreach (ICategory category in _categoryRepository.FindAll())
            {
                if (content.Contains(category.Name.ToUpperInvariant()))
                {
                    total += _matchValue;
                }
            }

            return total;
        }
    }
}