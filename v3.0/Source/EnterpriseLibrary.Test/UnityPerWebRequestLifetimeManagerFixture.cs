using System;
using System.Collections;
using System.Web;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    using EnterpriseLibrary;

    public class UnityPerWebRequestLifetimeManagerFixture
    {
        private readonly UnityPerWebRequestLifetimeManager _lifetimeManager;
        private readonly Mock<IDisposable> _disposable;
        private readonly Mock<HttpContextBase> _httpContext;

        public UnityPerWebRequestLifetimeManagerFixture()
        {
            _httpContext = new Mock<HttpContextBase>();
            _httpContext.SetupGet(c => c.Items).Returns(new Hashtable());
            _lifetimeManager = new UnityPerWebRequestLifetimeManager(_httpContext.Object);

            _disposable = new Mock<IDisposable>();
        }

        [Fact]
        public void Default_Constructor_Should_Throw_Exception_When_Not_Running_In_WebServer()
        {
            Assert.Throws<ArgumentNullException>(() => new UnityPerWebRequestLifetimeManager());
        }

        [Fact]
        public void SetValue_Should_Set_Value()
        {
            _lifetimeManager.SetValue(_disposable.Object);

            Assert.Same(_disposable.Object, _lifetimeManager.GetValue());
        }

        [Fact]
        public void SetValue_With_Same_Value_Should_Not_Make_Any_Impact()
        {
            _lifetimeManager.SetValue(_disposable.Object);
            _lifetimeManager.SetValue(_disposable.Object);

            Assert.Same(_disposable.Object, _lifetimeManager.GetValue());
        }

        [Fact]
        public void SetValue_With_New_Value_Should_Dispose_The_Previous_Value_When_Previous_Value_Is_Disposable()
        {
            var newValue = new Mock<IDisposable>();

            _disposable.Setup(d => d.Dispose()).Verifiable();

            _lifetimeManager.SetValue(_disposable.Object);
            _lifetimeManager.SetValue(newValue.Object);

            _disposable.Verify();
        }

        [Fact]
        public void Setting_Null_Value_Should_Remove_The_Value()
        {
            _lifetimeManager.SetValue(null);

            Assert.Null(_lifetimeManager.GetValue());
        }

        [Fact]
        public void GetValue_Should_Return_The_Same_Value()
        {
            _lifetimeManager.SetValue(_disposable.Object);

            Assert.Same(_disposable.Object, _lifetimeManager.GetValue());
        }

        [Fact]
        public void RemoveValue_Should_Remove_The_Value()
        {
            _lifetimeManager.SetValue(_disposable.Object);
            _lifetimeManager.RemoveValue();

            Assert.Null(_lifetimeManager.GetValue());
        }
    }
}