using System;

using Xunit;
using Xunit.Extensions;

namespace Kigg.Core.Test
{
    public class GuidExtensionFixture
    {
        [Theory]
        [InlineData("CD31A824-7686-4774-9143-3FC3ED79510F")]
        [InlineData("{909F4A0A-805A-4dea-BF20-C35E26A4089C}")]
        public void Shrink_Should_Always_Return_Twenty_Two_Character_String(string guid)
        {
            Assert.Equal(22, new Guid(guid).Shrink().Length);
        }

        [Fact]
        public void IsEmpty_Should_Return_True_For_Empty_Guid()
        {
            Assert.True(Guid.Empty.IsEmpty());
        }
    }
}