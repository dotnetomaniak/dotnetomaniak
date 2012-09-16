namespace Kigg.Infrastructure
{
    using System.Collections.Specialized;

    public interface IConfigurationManager
    {
        NameValueCollection AppSettings
        {
            get;
        }

        string ConnectionStrings(string name);

        T GetSection<T>(string sectionName);
    }
}