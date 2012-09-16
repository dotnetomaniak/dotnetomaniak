namespace Kigg.Service
{
    using System;
    using System.Diagnostics;

    using DomainObjects;

    public abstract class StoryWeightBaseCalculator : IStoryWeightCalculator
    {
        private readonly string _name;

        protected StoryWeightBaseCalculator(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            _name = name;
        }

        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return _name;
            }
        }

        public abstract double Calculate(DateTime publishingTimestamp, IStory story);
    }
}