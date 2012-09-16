using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class IoCFixture : BaseFixture
    {
        public override void Dispose()
        {
            resolver.Verify();
        }

        [Fact]
        public void InitializeWith_Should_Call_Factory_CreateInstance()
        {
            var factory = new Mock<IDependencyResolverFactory>();

            factory.Setup(f => f.CreateInstance()).Returns(resolver.Object).Verifiable();

            IoC.InitializeWith(factory.Object);
            factory.Verify();
        }

        [Fact]
        public void Register_Should_Use_Resolver()
        {
            resolver.Setup(r => r.Register<IDummyObject>(It.IsAny<DummyObject>()));

            IoC.Register(new DummyObject());
        }

        [Fact]
        public void Inject_Should_Use_Resolver()
        {
            resolver.Setup(r => r.Inject(It.IsAny<DummyObject>()));

            IoC.Inject(new DummyObject());
        }

        [Fact]
        public void Parameterless_Resolve_Should_Use_Resolver()
        {
            resolver.Setup(r => r.Resolve<IDummyObject>(typeof(IDummyObject))).Returns((IDummyObject) null).Verifiable();

            IoC.Resolve<IDummyObject>(typeof(IDummyObject));
        }

        [Fact]
        public void Parameterized_Resolve_Should_Use_Resolver()
        {
            resolver.Setup(r => r.Resolve<IDummyObject>(typeof(IDummyObject), "foo")).Returns((IDummyObject)null).Verifiable();

            IoC.Resolve<IDummyObject>(typeof(IDummyObject), "foo");
        }

        [Fact]
        public void Parameterless_Generic_Resolve_Should_Use_Resolve()
        {
            resolver.Setup(r => r.Resolve<IDummyObject>()).Returns((IDummyObject)null).Verifiable();

            IoC.Resolve<IDummyObject>();
        }

        [Fact]
        public void Parameterized_Generic_Resolve_Should_Use_Resolver()
        {
            resolver.Setup(r => r.Resolve<IDummyObject>("foo")).Returns((IDummyObject) null).Verifiable();

            IoC.Resolve<IDummyObject>("foo");
        }

        [Fact]
        public void ResolveAll_Should_Use_Resolver()
        {
            resolver.Setup(r => r.ResolveAll<IDummyObject>()).Returns((IDummyObject[]) null).Verifiable();

            IoC.ResolveAll<IDummyObject>();
        }

        [Fact]
        public void Reset_Should_Use_Resolver()
        {
            resolver.Setup(r => r.Dispose()).Verifiable();

            IoC.Reset();
        }
    }
}