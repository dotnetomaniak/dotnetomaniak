using System;

using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class DependencyResolverFactoryFixture
    {
        private readonly IDependencyResolver _resolver;

        public DependencyResolverFactoryFixture()
        {
            var type = typeof(DependencyResolverTestDouble);
            var typeName = string.Format("{0}, {1}", type.FullName, type.Assembly.FullName);

            var factory = new DependencyResolverFactory(typeName);

            _resolver = factory.CreateInstance();
        }

        [Fact]
        public void CreateInstance_Should_Return_New_Resolver()
        {
            Assert.NotNull(_resolver);
        }

        [Fact]
        public void CreateInstance_Should_Return_Correct_Resolver_Type()
        {
            Assert.IsType<DependencyResolverTestDouble>(_resolver);
        }

        [Fact]
        public void Constructor_Should_Throw_Exception_When_Config_File_Is_Missing()
        {
            Assert.Throws<ArgumentException>(() => new DependencyResolverFactory());
        }
    }
}