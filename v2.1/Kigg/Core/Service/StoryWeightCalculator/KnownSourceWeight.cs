namespace Kigg.Service
{
    using System;

    using DomainObjects;
    using Repository;

    public class KnownSourceWeight : StoryWeightBaseCalculator
    {
        private readonly IKnownSourceRepository _repository;

        public KnownSourceWeight(IKnownSourceRepository repository) : base("Znane Ÿród³o")
        {
            Check.Argument.IsNotNull(repository, "repository");

            _repository = repository;
        }

        public override double Calculate(DateTime publishingTimestamp, IStory story)
        {
            Check.Argument.IsNotNull(story, "story");

            IKnownSource knownSource = _repository.FindMatching(story.Url);
            KnownSourceGrade grade = (knownSource == null) ? KnownSourceGrade.None : knownSource.Grade;

            return (int) grade;
        }
    }
}