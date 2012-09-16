namespace Kigg.Infrastructure
{
    public interface ISpamWeightCalculator
    {
        int Calculate(string content);
    }
}