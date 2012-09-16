using System;
using System.Linq;

using Xunit;

namespace Kigg.Core.Test
{
    public class EnumerableExtensionFixture
    {
        [Fact]
        public void ForEach_Should_Call_The_Specified_Method_Each_Item()
        {
            var array = new[] { "Item 1", "Item2" };
            var called = new[] { false, false };

            array.ForEach(i => called[Array.IndexOf(array, i)] = true);

            Assert.True(called.All(c => c));
        }
    }
}