using System;

using Xunit;

namespace Kigg.Core.Test
{
    public class DateTimeExtensionFixture
    {
        [Fact]
        public void IsValid_Should_Return_False_When_Specified_Date_Is_Smaller_Than_MinDate()
        {
            Assert.False(DateTime.MinValue.IsValid());
        }

        [Fact]
        public void IsValid_Should_Return_False_When_Specified_Date_Is_Greater_Than_MaxDate()
        {
            Assert.False(DateTime.MaxValue.IsValid());
        }

        [Fact]
        public void IsValid_Should_Return_True_When_Specified_Date_Is_Between_MinDate_And_MaxDate()
        {
            Assert.True(SystemTime.Now().IsValid());
        }
    }
}