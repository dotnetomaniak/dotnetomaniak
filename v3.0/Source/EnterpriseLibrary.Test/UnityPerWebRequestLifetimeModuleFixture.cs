using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    using EnterpriseLibrary;

    public class UnityPerWebRequestLifetimeModuleFixture
    {
        private readonly Mock<HttpContextBase> _httpContext;
        private readonly UnityPerWebRequestLifetimeModule _module;

        public UnityPerWebRequestLifetimeModuleFixture()
        {
            _httpContext = new Mock<HttpContextBase>();
            _httpContext.SetupGet(c => c.Items).Returns(new Hashtable());

            _module = new UnityPerWebRequestLifetimeModule(_httpContext.Object);
        }

        [Fact]
        public void Default_Constructor_Should_Not_Throws_Exception()
        {
            Assert.DoesNotThrow(() => new UnityPerWebRequestLifetimeModule());
        }

        [Fact]
        public void GetInstances_Should_Add_Instances_When_Instances_Does_Not_Exist_In_Context()
        {
            Assert.NotNull(UnityPerWebRequestLifetimeModule.GetInstances(_httpContext.Object));
        }

        [Fact]
        public void GetInstances_Should_Return_Existing_Instances_When_Instances_Exist_In_Context()
        {
            var items = new Mock<IDictionary>();

            items.Setup(i => i.Contains(It.IsAny<object>())).Returns(true);
            items.SetupGet(i => i[It.IsAny<Object>()]).Returns(new Dictionary<UnityPerWebRequestLifetimeManager, object>());

            _httpContext.Setup(h => h.Items).Returns(items.Object);

            Assert.NotNull(UnityPerWebRequestLifetimeModule.GetInstances(_httpContext.Object));
        }

        [Fact]
        public void RemoveAllInstances_Should_Make_The_Instances_Empty()
        {
            _module.Instances.Add(new UnityPerWebRequestLifetimeManager(_httpContext.Object), new object());
            _module.Instances.Add(new UnityPerWebRequestLifetimeManager(_httpContext.Object), new object());
            _module.Instances.Add(new UnityPerWebRequestLifetimeManager(_httpContext.Object), new object());

            _module.RemoveAllInstances();

            Assert.Empty(_module.Instances);
        }

        [Fact]
        public void RemoveAllInstances_Should_Call_Dispose_Of_The_Value_When_the_Value_Is_Disposable()
        {
            var disposable = new Mock<IDisposable>();

            disposable.Setup(d => d.Dispose()).Verifiable();

            _module.Instances.Add(new UnityPerWebRequestLifetimeManager(_httpContext.Object), disposable.Object);
            _module.RemoveAllInstances();

            disposable.Verify();
        }
    }
}