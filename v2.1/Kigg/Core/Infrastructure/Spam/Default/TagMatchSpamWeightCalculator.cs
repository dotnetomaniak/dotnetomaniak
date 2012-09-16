namespace Kigg.Infrastructure
{
    using DomainObjects;
    using Repository;

    public class TagMatchSpamWeightCalculator : ISpamWeightCalculator
    {
        private readonly int _matchValue;

        private readonly int _topTags;
        private readonly ITagRepository _tagRepository;

        public TagMatchSpamWeightCalculator(int matchValue, int topTags, ITagRepository tagRepository)
        {
            Check.Argument.IsNotNegativeOrZero(topTags, "topTags");
            Check.Argument.IsNotNull(tagRepository, "tagRepository");

            _matchValue = matchValue;
            _topTags = topTags;
            _tagRepository = tagRepository;
        }

        public int Calculate(string content)
        {
            int total = 0;

            content = content.StripHtml().ToUpperInvariant();

            foreach (ITag tag in _tagRepository.FindByUsage(_topTags))
            {
                if (content.Contains(tag.Name.ToUpperInvariant()))
                {
                    total += _matchValue;
                }
            }

            return total;
        }
    }
}