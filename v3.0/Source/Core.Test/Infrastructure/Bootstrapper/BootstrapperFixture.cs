using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    using Kigg.Test.Infrastructure;

    public class BootstrapperFixture : BaseFixture
    {
        [Fact]
        public void Run_Should_Execute_Tasks()
        {
            var task1 = new Mock<IBootstrapperTask>();
            var task2 = new Mock<IBootstrapperTask>();
            var task3 = new Mock<IBootstrapperTask>();

            task1.Setup(t => t.Execute()).Verifiable();
            task2.Setup(t => t.Execute()).Verifiable();
            task3.Setup(t => t.Execute()).Verifiable();

            resolver.Setup(r => r.ResolveAll<IBootstrapperTask>()).Returns(new[] { task1.Object, task2.Object, task3.Object });

            Bootstrapper.Run();

            task1.Verify();
            task2.Verify();
            task3.Verify();
        }
    }
}