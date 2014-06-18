using System;
using System.Collections.Generic;
using Kigg.Infrastructure;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class DependencyResolverTestDouble : IDependencyResolver
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Register<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public void Inject<T>(T existing)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(Type type)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }
    }
}