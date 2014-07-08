using System;

using Xunit;

namespace Kigg.Web.Test
{
    public class DateTimeExtensionFixture
    {
        [Fact]
        public void ToRelative_Should_Return_Correct_String()
        {
            var since = new DateTime(2001, 1, 1).ToRelative();

            Assert.Contains("dni", since);
        }
    }
}