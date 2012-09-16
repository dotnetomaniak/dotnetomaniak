namespace Kigg.Infrastructure
{
    using System.Diagnostics;
    using System.Collections.Specialized;
    using System.Configuration;

    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        public NameValueCollection AppSettings
        {
            [DebuggerStepThrough]
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        [DebuggerStepThrough]
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        [DebuggerStepThrough]
        public string GetProviderName(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ProviderName;
        }

        [DebuggerStepThrough]
        public T GetSection<T>(string sectionName)
        {
            return (T) ConfigurationManager.GetSection(sectionName);
        }
    }
}