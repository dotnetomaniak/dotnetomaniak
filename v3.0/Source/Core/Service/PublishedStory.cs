namespace Kigg.Service
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using DomainObjects;

    public class PublishedStory
    {
        public PublishedStory(IStory story)
        {
            Check.Argument.IsNotNull(story, "story");

            Story = story;
            Weights = new Dictionary<string, double>();
        }

        public IStory Story
        {
            get;
            private set;
        }

        public int Rank
        {
            get;
            internal set;
        }

        public IDictionary<string, double> Weights
        {
            get;
            private set;
        }

        public double TotalScore
        {
            [DebuggerStepThrough]
            get
            {
                return Weights.Sum(p => p.Value);
            }
        }
    }
}