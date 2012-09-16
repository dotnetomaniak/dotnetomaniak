namespace Kigg.Web
{
    using System.Configuration;
    using System.Diagnostics;

    public class AssetElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            [DebuggerStepThrough]
            get { return (string)this["name"]; }
            [DebuggerStepThrough]
            set { this["name"] = value; }
        }

        [ConfigurationProperty("contentType", IsRequired = true)]
        public string ContentType
        {
            [DebuggerStepThrough]
            get { return (string)this["contentType"]; }
            [DebuggerStepThrough]
            set { this["contentType"] = value; }
        }

        [ConfigurationProperty("compress", DefaultValue = AssetSettingsSection.DefaultCompress)]
        public bool Compress
        {
            [DebuggerStepThrough]
            get { return (bool) this["compress"]; }
            [DebuggerStepThrough]
            set { this["compress"] = value; }
        }

        [ConfigurationProperty("generateETag", DefaultValue = AssetSettingsSection.DefaultGenerateETag)]
        public bool GenerateETag
        {
            [DebuggerStepThrough]
            get { return (bool)this["generateETag"]; }
            [DebuggerStepThrough]
            set { this["generateETag"] = value; }
        }

        [ConfigurationProperty("version")]
        public string Version
        {
            [DebuggerStepThrough]
            get { return (string) this["version"]; }
            [DebuggerStepThrough]
            set { this["version"] = value; }
        }

        [ConfigurationProperty("cacheDurationInDays", DefaultValue = AssetSettingsSection.DefaultCacheDurationInDays)]
        public float CacheDurationInDays
        {
            [DebuggerStepThrough]
            get { return (float) this["cacheDurationInDays"]; }
            [DebuggerStepThrough]
            set { this["cacheDurationInDays"] = value; }
        }

        [ConfigurationProperty("directory", IsRequired = true)]
        public string Directory
        {
            [DebuggerStepThrough]
            get { return (string)this["directory"]; }
            [DebuggerStepThrough]
            set { this["directory"] = value; }
        }

        [ConfigurationProperty("files", IsRequired = true)]
        public string Files
        {
            [DebuggerStepThrough]
            get { return (string)this["files"]; }
            [DebuggerStepThrough]
            set { this["files"] = value; }
        }
    }
}