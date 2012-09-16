namespace Kigg.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DomainObjects;
    using Repository;

    public class ViewWeight : StoryWeightBaseCalculator
    {
        private readonly float _weightMultiply;
        private readonly IStoryViewRepository _viewRepository;

        public ViewWeight(IStoryViewRepository storyViewRepository, float weightMultiply) : base("View")
        {
            Check.Argument.IsNotNull(storyViewRepository, "storyViewRepository");
            Check.Argument.IsNotNegative(weightMultiply, "weightMultiply");

            _viewRepository = storyViewRepository;
            _weightMultiply = weightMultiply;
        }

        public override double Calculate(DateTime publishingTimestamp, IStory story)
        {
            Check.Argument.IsNotNull(story, "story");

            ICollection<IStoryView> views = _viewRepository.FindAfter(story.Id, story.LastProcessedAt ?? story.CreatedAt);

            // Multiply weight for each unique ip address
            return views.Select(v => v.FromIPAddress).Distinct().Count() * _weightMultiply;
        }
    }
}