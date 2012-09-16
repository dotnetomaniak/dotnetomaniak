namespace Kigg.Service
{
    using System;

    using DomainObjects;

    public class FreshnessWeight : StoryWeightBaseCalculator
    {
        private readonly float _freshnessThresholdInDays;
        private readonly float _intervalInHours;

        public FreshnessWeight(float freshnessThresholdInDays, float intervalInHours) : base("Œwie¿oœæ")
        {
            Check.Argument.IsNotNegativeOrZero(freshnessThresholdInDays, "freshnessThresholdInDays");
            Check.Argument.IsNotNegativeOrZero(intervalInHours, "intervalInHours");

            _freshnessThresholdInDays = freshnessThresholdInDays;
            _intervalInHours = intervalInHours;
        }

        public override double Calculate(DateTime publishingTimestamp, IStory story)
        {
            Check.Argument.IsNotInFuture(publishingTimestamp, "publishingTimestamp");
            Check.Argument.IsNotNull(story, "story");

            TimeSpan difference = (publishingTimestamp - story.CreatedAt);

            return (difference.TotalDays <= _freshnessThresholdInDays) ? difference.TotalHours / _intervalInHours : 0;
        }
    }
}