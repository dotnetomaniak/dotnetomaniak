namespace Kigg.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DomainObjects;
    using Repository;

    public class CommentWeight : StoryWeightBaseCalculator
    {
        private readonly float _ownerWeight;
        private readonly float _sameIpAddressWeight;
        private readonly float _differentIPAddressWeight;

        private readonly ICommentRepository _commentRepository;

        public CommentWeight(ICommentRepository commentRepository, float ownerWeight, float sameIPAddressWeight, float differentIPAddressWeight) : base("Comment")
        {
            Check.Argument.IsNotNull(commentRepository, "commentRepository");
            Check.Argument.IsNotNegative(ownerWeight, "ownerWeight");
            Check.Argument.IsNotNegative(sameIPAddressWeight, "sameIPAddressWeight");
            Check.Argument.IsNotNegative(differentIPAddressWeight, "differentIPAddressWeight");

            _commentRepository = commentRepository;
            _ownerWeight = ownerWeight;
            _sameIpAddressWeight = sameIPAddressWeight;
            _differentIPAddressWeight = differentIPAddressWeight;
        }

        public override double Calculate(DateTime publishingTimestamp, IStory story)
        {
            Check.Argument.IsNotNull(story, "story");

            // Give ownerScore if comment is postedby the same user
            // who actually posted the story
            // or
            // Give sameIpScore if comment is submitted form the same ip
            // or differntIpScore if from different ip
            ICollection<IComment> comments = _commentRepository.FindAfter(story.Id, story.LastProcessedAt ?? story.CreatedAt);

            double total = comments.Sum(c => story.IsPostedBy(c.ByUser) ? _ownerWeight : string.CompareOrdinal(c.FromIPAddress, story.FromIPAddress) == 0 ? _sameIpAddressWeight : _differentIPAddressWeight);

            return total;
        }
    }
}