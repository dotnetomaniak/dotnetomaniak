namespace Kigg.Infrastructure
{
    using System.Text.RegularExpressions;

    public class HyperlinkCountSpamWeightCalculator : ISpamWeightCalculator
    {
        private readonly static Regex HyperLinkExpression = new Regex(@"(?:(?:ftp|http|https|mailto):\/{0,2}|(?:www|ftp|mail)\.)(?:[a-zA-Z0-9\-\@]{1,63}\.)+[a-zA-Z]{2,63}", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private readonly int _matchValue;

        public HyperlinkCountSpamWeightCalculator(int matchValue)
        {
            _matchValue = matchValue;
        }

        public int Calculate(string content)
        {
            return HyperLinkExpression.Matches(content).Count * _matchValue;
        }
    }
}