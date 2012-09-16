using System;

using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class SubscriptionTokenFixture
    {
        private readonly SubscriptionToken _token;

        public SubscriptionTokenFixture()
        {
            _token = new SubscriptionToken();
        }

        [Fact]
        public void Equals_Should_Return_False_When_Tokens_Are_Not_Same()
        {
            SubscriptionToken token = new SubscriptionToken();

            Assert.False(_token.Equals(token));
        }

        [Fact]
        public void Equals_Should_Return_False_When_Comparing_Different_Instances()
        {
            SubscriptionToken token = new SubscriptionToken();

            object tokenObject = new SubscriptionToken();

            Assert.False(token.Equals(tokenObject));
        }

        [Fact]
        public void GetHashCode_Should_Not_Be_Zero()
        {
            Assert.NotEqual(0, _token.GetHashCode());
        }

        [Fact]
        public void ToString_Should_Return_Internal_Guid()
        {
            Assert.NotEqual(new Guid(_token.ToString()), Guid.Empty);
        }
    }
}