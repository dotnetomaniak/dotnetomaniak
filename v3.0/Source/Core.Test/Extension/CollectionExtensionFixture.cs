using Xunit;

namespace Kigg.Core.Test
{
    public class CollectionExtensionFixture
    {
        [Fact]
        public static void IsNullOrEmpty_Should_Return_True_When_Collection_Is_Null()
        {
            Assert.True(((string[]) null).IsNullOrEmpty());
        }

        [Fact]
        public static void IsNullOrEmpty_Should_Return_True_When_Collection_Is_Empty()
        {
            Assert.True((new string[]{}).IsNullOrEmpty());
        }

        [Fact]
        public static void IsNullOrEmpty_Should_Return_False_When_Collection_Containes_Items()
        {
            Assert.False((new[] { "An item" }).IsNullOrEmpty());
        }
    }
}