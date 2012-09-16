using System;

using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class ConfigurationManagerWrapperFixture
    {
        private readonly IConfigurationManager _configurationManager;

        public ConfigurationManagerWrapperFixture()
        {
            _configurationManager = new ConfigurationManagerWrapper();
        }

        [Fact]
        public void AppSettings_Should_Be_Null_When_Quering_An_Non_Existent_Key()
        {
            Assert.Null(_configurationManager.AppSettings["foo"]);
        }

        [Fact]
        public void ConnectionString_Should_Throw_Exception_When_Accessing_An_Non_Existent_Key()
        {
            Assert.Throws<NullReferenceException>(() => _configurationManager.ConnectionStrings("foo"));
        }

        [Fact]
        public void GetSection_Should_Not_Throw_Exception_When_Accessing_An_Non_Existent_Key()
        {
            Assert.DoesNotThrow(() => _configurationManager.GetSection<object>("foo"));
        }
    }
}