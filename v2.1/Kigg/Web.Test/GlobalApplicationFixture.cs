using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class GlobalApplicationFixture : BaseFixture
    {
        [Fact]
        public void OnStart_Should_Not_Throw_Exception()
        {
            GlobalApplication.OnStart();
        }

        [Fact]
        public void OnEnd_Should_Log_Warning()
        {
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            GlobalApplication.OnEnd();

            log.Verify();
        }
    }
}