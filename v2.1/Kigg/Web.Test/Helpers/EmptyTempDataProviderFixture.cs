using Xunit;

namespace Kigg.Web.Test
{
    public class EmptyTempDataProviderFixture
    {
        private readonly EmptyTempDataProvider _provider;

        public EmptyTempDataProviderFixture()
        {
            _provider = new EmptyTempDataProvider();
        }

        [Fact]
        public void LoadTempData_Should_Return_New_Dictionary()
        {
            Assert.NotNull(_provider.LoadTempData(null));
        }

        [Fact]
        public void SaveTempData_Should_Do_Nothing()
        {
            Assert.DoesNotThrow(() => _provider.SaveTempData(null, null));
        }
    }
}