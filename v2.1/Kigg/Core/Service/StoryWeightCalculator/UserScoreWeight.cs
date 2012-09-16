namespace Kigg.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DomainObjects;
    using Repository;

    public class UserScoreWeight : StoryWeightBaseCalculator
    {
        private readonly float _scorePercent;
        private readonly float _adminMultiply;
        private readonly float _moderatorMultiply;

        private readonly IVoteRepository _voteRepository;

        public UserScoreWeight(IVoteRepository voteRepository, float scorePercent, float adminMultiply, float moderatorMultiply): base("Ocena użytkownika")
        {
            Check.Argument.IsNotNull(voteRepository, "voteRepository");
            Check.Argument.IsNotNegative(scorePercent, "scorePercent");
            Check.Argument.IsNotNegative(adminMultiply, "adminMultiply");
            Check.Argument.IsNotNegative(moderatorMultiply, "moderatorMultiply");

            _voteRepository = voteRepository;
            _scorePercent = scorePercent;
            _adminMultiply = adminMultiply;
            _moderatorMultiply = moderatorMultiply;
        }

        public override double Calculate(DateTime publishingTimestamp, IStory story)
        {
            Check.Argument.IsNotNull(story, "story");

            double maxScore = double.MinValue;
            double userScore = 0;

            IEnumerable<IUser> users = _voteRepository.FindAfter(story.Id, story.LastProcessedAt ?? story.CreatedAt).Select(v => v.ByUser);

            foreach (IUser user in users.Where(u => u.IsPublicUser()))
            {
                double score = (Convert.ToDouble(user.CurrentScore) * _scorePercent);

                // Ignore bad users
                if (score > 0)
                {
                    userScore += score;

                    if (score > maxScore)
                    {
                        maxScore = score;
                    }
                }
            }

            if (maxScore > 0)
            {
                double adminScore = 0;
                double modaratorScore = 0;

                if (_adminMultiply > 0)
                {
                    users.Where(u => u.IsAdministrator()).ForEach(u => adminScore += (maxScore * _adminMultiply));
                }

                if (_adminMultiply > 0)
                {
                    users.Where(u => u.IsModerator()).ForEach(u => modaratorScore += (maxScore * _moderatorMultiply));
                }

                userScore += (adminScore + modaratorScore);
            }

            return userScore;
        }
    }
}
