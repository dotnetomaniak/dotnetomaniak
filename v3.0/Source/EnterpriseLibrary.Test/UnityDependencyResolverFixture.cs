using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class UnityDependencyResolverFixture : IDisposable
    {
        private readonly Mock<UnityContainerTestDouble> _container;
        private readonly UnityDependencyResolver _resolver;

        public UnityDependencyResolverFixture()
        {
            _container = new Mock<UnityContainerTestDouble>();
            _resolver = new UnityDependencyResolver(_container.Object);
        }

        public void Dispose()
        {
            _container.Verify();
        }

        [Fact]
        public void Constructor_Should_Throw_Exception_When_Config_File_Is_Missing()
        {
            Assert.Throws<NullReferenceException>(() => new UnityDependencyResolver());
        }

        [Fact]
        public void Dispose_Should_Use_Container()
        {
            _container.Setup(c => c.Dispose()).Verifiable();

            _resolver.Dispose();
        }

        [Fact]
        public void Register_Should_Use_Container()
        {
            _container.Setup(c => c.RegisterInstance<IDummyObject>(It.IsAny<DummyObject>())).Returns(_container.Object);

            _resolver.Register<IDummyObject>(new DummyObject());
        }

        [Fact]
        public void Inject_Should_Use_Container()
        {
            _container.Setup(c => c.BuildUp(It.IsAny<DummyObject>())).Returns(new DummyObject());

            _resolver.Inject(new DummyObject());
        }

        [Fact]
        public void Parameterized_Generic_Resolve_Should_Use_Container()
        {
            _container.Setup(c => c.Resolve<IDummyObject>("foo")).Returns((IDummyObject) null).Verifiable();

            _resolver.Resolve<IDummyObject>("foo");
        }

        [Fact]
        public void Parameterless_Generic_Resolve_Should_Use_Container()
        {
            _container.Setup(c => c.Resolve<IDummyObject>()).Returns((IDummyObject) null).Verifiable();

            _resolver.Resolve<IDummyObject>();
        }

        [Fact]
        public void Parameterized_Resolve_Should_Use_Container()
        {
            _container.Setup(c => c.Resolve(typeof (IDummyObject), "foo")).Returns((IDummyObject) null).Verifiable();

            _resolver.Resolve<IDummyObject>(typeof (IDummyObject), "foo");
        }

        [Fact]
        public void Parameterless_Resolve_Should_Use_Container()
        {
            _container.Setup(c => c.Resolve(typeof (IDummyObject))).Returns((IDummyObject) null).Verifiable();

            _resolver.Resolve<IDummyObject>(typeof (IDummyObject));
        }

        [Fact]
        public void ResolveAll_Should_Use_Container()
        {
            _container.Setup(c => c.ResolveAll<IDummyObject>()).Returns((IDummyObject[]) null).Verifiable();
            _container.Setup(c => c.Resolve<IDummyObject>()).Returns((IDummyObject) null).Verifiable();

            _resolver.ResolveAll<IDummyObject>();
        }

        [Fact]
        public void ResolveAll_Should_Copy_The_Default_Instance()
        {
            var obj1 = new Mock<IDummyObject>();
            var obj2 = new Mock<IDummyObject>();

            _container.Setup(c => c.ResolveAll<IDummyObject>()).Returns((new List<IDummyObject>{ obj1.Object })).Verifiable();
            _container.Setup(c => c.Resolve<IDummyObject>()).Returns(obj2.Object).Verifiable();

            _resolver.ResolveAll<IDummyObject>();
        }

        [Fact]
        public void ResolveAll_Should_Ignore_Resolution_Failed_Exception()
        {
            _container.Setup(c => c.ResolveAll<IDummyObject>()).Returns((IDummyObject[])null);
            _container.Setup(c => c.Resolve<IDummyObject>()).Throws<ResolutionFailedExceptionTestDouble>();

            _resolver.ResolveAll<IDummyObject>();
        }
    }

    public abstract class UnityContainerTestDouble : IUnityContainer
    {
        public abstract void Dispose();

        public abstract IUnityContainer RegisterInstance<TInterface>(TInterface instance);

        public abstract T BuildUp<T>(T existing);

        public abstract object Resolve(Type t);

        public abstract object Resolve(Type t, string name);

        public abstract T Resolve<T>();

        public abstract T Resolve<T>(string name);

        public abstract IEnumerable<T> ResolveAll<T>();

        #region Unused IUnityContainer Members

        // ReSharper disable UnusedTypeParameter

        public IUnityContainer Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IUnityContainer RegisterType<T>(params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType<TFrom, TTo>(string name, params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType<TFrom, TTo>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType<T>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType<T>(string name, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType<T>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type t, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type from, Type to, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type from, Type to, string name, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type from, Type to, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type t, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type t, string name, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type t, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance<TInterface>(TInterface instance, LifetimeManager lifetimeManager)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance<TInterface>(string name, TInterface instance)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance<TInterface>(string name, TInterface instance, LifetimeManager lifetimeManager)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance(Type t, object instance)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance(Type t, object instance, LifetimeManager lifetimeManager)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            throw new NotImplementedException();
        }

        public T BuildUp<T>(T existing, string name)
        {
            throw new NotImplementedException();
        }

        public object BuildUp(Type t, object existing)
        {
            throw new NotImplementedException();
        }

        public object BuildUp(Type t, object existing, string name)
        {
            throw new NotImplementedException();
        }

        public void Teardown(object o)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer AddNewExtension<TExtension>() where TExtension : UnityContainerExtension, new()
        {
            throw new NotImplementedException();
        }

        public TConfigurator Configure<TConfigurator>() where TConfigurator : IUnityContainerExtensionConfigurator
        {
            throw new NotImplementedException();
        }

        public object Configure(Type configurationInterface)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RemoveAllExtensions()
        {
            throw new NotImplementedException();
        }

        public IUnityContainer CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        // ReSharper restore UnusedTypeParameter

        #endregion
    }

    public interface IDummyObject
    {
        string Foo
        {
            get;
            set;
        }
    }

    public class DummyObject : IDummyObject
    {
        #region IDummyObject Members

        public virtual string Foo
        {
            get;
            set;
        }

        #endregion
    }

    public class ResolutionFailedExceptionTestDouble : ResolutionFailedException
    {
        public ResolutionFailedExceptionTestDouble() : base(typeof(DummyObject), null, new InvalidOperationException())
        {
        }
    }
}