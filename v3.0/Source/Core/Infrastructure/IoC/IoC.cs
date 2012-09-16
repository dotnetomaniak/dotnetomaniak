namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class IoC
    {
        private static IDependencyResolver _resolver;

        [DebuggerStepThrough]
        public static void InitializeWith(IDependencyResolverFactory factory)
        {
            Check.Argument.IsNotNull(factory, "factory");

            _resolver = factory.CreateInstance();
        }

        [DebuggerStepThrough]
        public static void Register<T>(T instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            _resolver.Register(instance);
        }

        [DebuggerStepThrough]
        public static void Inject<T>(T existing)
        {
            Check.Argument.IsNotNull(existing, "existing");

            _resolver.Inject(existing);
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(Type type)
        {
            Check.Argument.IsNotNull(type, "type");

            return _resolver.Resolve<T>(type);
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(Type type, string name)
        {
            Check.Argument.IsNotNull(type, "type");
            Check.Argument.IsNotEmpty(name, "name");

            return _resolver.Resolve<T>(type, name);
        }

        [DebuggerStepThrough]
        public static T Resolve<T>()
        {
            return _resolver.Resolve<T>();
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return _resolver.Resolve<T>(name);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> ResolveAll<T>()
        {
            return _resolver.ResolveAll<T>();
        }

        [DebuggerStepThrough]
        public static void Reset()
        {
            if (_resolver != null)
            {
                _resolver.Dispose();
            }
        }
    }
}