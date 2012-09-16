using System;
using System.Diagnostics;
using System.Globalization;

namespace Kigg.EF.Repository
{
    using Infrastructure;
    using System.Data.EntityClient;
    
    public class ConnectionString :IConnectionString
    {

        private const string _edmMetadataFormat = "{0}\\{1}.csdl|{0}\\{2}.ssdl|{0}\\{3}.msl";
        private const string _csdlFileName = "DomainObjects";
        private const string _mslFileName = "DomainObjects";
        private const string _ssdlFileName = "DomainObjects";
        private const string _edmFilesPath = "|DataDirectory|";
        
        private readonly EntityConnectionStringBuilder _entityBuilder;

        public ConnectionString(IConfigurationManager configuration, string name)
            : this(configuration, name, null, null, null, null)
        {

        }
        
        public ConnectionString(IConfigurationManager configuration, string name, string edmFilesPath)
            : this(configuration, name, edmFilesPath, null, null, null)
        {

        }
        
        public ConnectionString(IConfigurationManager configuration, string name, string edmFilesPath, string ssdlFileName)
            : this(configuration, name, edmFilesPath, ssdlFileName, null, null)
        {

        }
        
        public ConnectionString(IConfigurationManager configuration, string name, string edmFilesPath, string ssdlFileName, string csdlFileName, string mslFileName)
        {
            Check.Argument.IsNotNull(configuration, "configuration");
            Check.Argument.IsNotEmpty(name, "name");
            
            if (String.IsNullOrEmpty(edmFilesPath))
            {
                edmFilesPath = _edmFilesPath;
            }
            if (String.IsNullOrEmpty(ssdlFileName))
            {
                ssdlFileName = _ssdlFileName;
            }
            
            if(String.IsNullOrEmpty(csdlFileName))
            {
                csdlFileName = _csdlFileName;
            }
            
            if(String.IsNullOrEmpty(mslFileName))
            {
                mslFileName = _mslFileName;
            }

            string providerConnectionString = configuration.GetConnectionString(name);
            string providerName = configuration.GetProviderName(name);
            string metadata = String.Format(CultureInfo.InvariantCulture,
                                            _edmMetadataFormat, 
                                            edmFilesPath, 
                                            csdlFileName, 
                                            ssdlFileName, 
                                            mslFileName);

            _entityBuilder = new EntityConnectionStringBuilder
                                 {
                                     ProviderConnectionString = providerConnectionString,
                                     Provider = providerName,
                                     Metadata = metadata
                                 };
        }

        public string Value
        {
            [DebuggerStepThrough]
            get
            {
                return _entityBuilder.ConnectionString;
            }
        }

        public string ProviderName
        {
            get
            {
                return _entityBuilder.Provider;
            }
        }
    }
}
