namespace Kigg.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DomainObjects;
    using Repository;

    public class VoteWeight : StoryWeightBaseCalculator
    {
        private readonly float _sameIpWeight;
        private readonly float _differentIpWeight;
        private readonly IVoteRepository _voteRepository;

        public VoteWeight(IVoteRepository voteRepository, float sameIPAddressWeight, float differentIPAddressWeight) : base("Głosy")
        {
            Check.Argument.IsNotNull(voteRepository, "voteRepository");
            Check.Argument.IsNotNegative(sameIPAddressWeight, "sameIPAddressWeight");
            Check.Argument.IsNotNegative(differentIPAddressWeight, "differentIPAddressWeight");

            _voteRepository = voteRepository;
            _sameIpWeight = sameIPAddressWeight;
            _differentIpWeight = differentIPAddressWeight;
        }

        public override double Calculate(DateTime publishingTimestamp, IStory story)
        {
            Check.Argument.IsNotNull(story, "story");

            ICollection<IVote> votes = _voteRepository.FindAfter(story.Id, story.LastProcessedAt ?? story.CreatedAt);

            double total = votes.Sum(v => (string.Compare(v.FromIPAddress, story.FromIPAddress, StringComparison.OrdinalIgnoreCase) == 0) ? _sameIpWeight : _differentIpWeight);

            return total;
        }
    }
}
