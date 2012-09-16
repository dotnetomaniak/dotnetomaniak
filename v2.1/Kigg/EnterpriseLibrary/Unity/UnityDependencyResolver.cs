namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Diagnostics;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    public class UnityDependencyResolver : DisposableResource, IDependencyResolver
    {
        private readonly IUnityContainer _container;

        [DebuggerStepThrough]
        public UnityDependencyResolver() : this(new UnityContainer())
        {
            UnityConfigurationSection configuration = (UnityConfigurationSection) ConfigurationManager.GetSection("unity");
            configuration.Containers.Default.Configure(_container);
        }

        [DebuggerStepThrough]
        public UnityDependencyResolver(IUnityContainer container)
        {
            Check.Argument.IsNotNull(container, "container");

            _container = container;
        }

        [DebuggerStepThrough]
        public void Register<T>(T instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            _container.RegisterInstance(instance);
        }

        [DebuggerStepThrough]
        public void Inject<T>(T existing)
        {
            Check.Argument.IsNotNull(existing, "existing");

            _container.BuildUp(existing);
        }

        [DebuggerStepThrough]
        public T Resolve<T>(Type type)
        {
            Check.Argument.IsNotNull(type, "type");

            return (T) _container.Resolve(type);
        }

        [DebuggerStepThrough]
        public T Resolve<T>(Type type, string name)
        {
            Check.Argument.IsNotNull(type, "type");
            Check.Argument.IsNotEmpty(name, "name");

            return (T) _container.Resolve(type, name);
        }

        [DebuggerStepThrough]
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        [DebuggerStepThrough]
        public T Resolve<T>(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return _container.Resolve<T>(name);
        }

        [DebuggerStepThrough]
        public IEnumerable<T> ResolveAll<T>()
        {
            IEnumerable<T> namedInstances = _container.ResolveAll<T>();
            T unnamedInstance = default(T);

            try
            {
                unnamedInstance = _container.Resolve<T>();
            }
            catch (ResolutionFailedException)
            {
                //When default instance is missing
            }

            if (Equals(unnamedInstance, default(T)))
            {
                return namedInstances;
            }

            return new ReadOnlyCollection<T>(new List<T>(namedInstances) { unnamedInstance });
        }

        [DebuggerStepThrough]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}