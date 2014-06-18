using Kigg.Infrastructure;
using Xunit;

namespace Kigg.Core.Test
{
    using Service;

    public class EventAggregatorFixture
    {
        private readonly EventAggregator _eventAggregator;

        public EventAggregatorFixture()
        {
            _eventAggregator = new EventAggregator();
        }

        [Fact]
        public void Get_Should_Return_Same_Instance_Of_Same_Event_Type()
        {
            var instance1 = _eventAggregator.GetEvent<StorySubmitEvent>();
            var instance2 = _eventAggregator.GetEvent<StorySubmitEvent>();

            Assert.Same(instance2, instance1);
        }
    }
}