namespace Kigg.Web
{
    using System.Configuration;
    using System.Diagnostics;

    public class AssetSettingsSection : ConfigurationSection
    {
        public const string SectionName = "assetSettings";

        internal const bool DefaultCompress = true;
        internal const bool DefaultGenerateETag = true;
        internal const float DefaultCacheDurationInDays = 365f;

        [ConfigurationProperty("compress", DefaultValue = DefaultCompress)]
        public bool Compress
        {
            [DebuggerStepThrough]
            get { return (bool)this["compress"]; }
            [DebuggerStepThrough]
            set { this["compress"] = value; }
        }

        [ConfigurationProperty("generateETag", DefaultValue = DefaultGenerateETag)]
        public bool GenerateETag
        {
            [DebuggerStepThrough]
            get { return (bool)this["generateETag"]; }
            [DebuggerStepThrough]
            set { this["generateETag"] = value; }
        }

        [ConfigurationProperty("cacheDurationInDays", DefaultValue = DefaultCacheDurationInDays)]
        public float CacheDurationInDays
        {
            [DebuggerStepThrough]
            get { return (float) this["cacheDurationInDays"]; }
            [DebuggerStepThrough]
            set { this["cacheDurationInDays"] = value; }
        }

        [ConfigurationProperty("version")]
        public string Version
        {
            [DebuggerStepThrough]
            get { return (string)this["version"]; }
            [DebuggerStepThrough]
            set { this["version"] = value; }
        }

        [ConfigurationProperty("assets", IsDefaultCollection = true, IsRequired = true)]
        public AssetElementCollection Assets
        {
            [DebuggerStepThrough]
            get
            {
                return (AssetElementCollection) base["assets"] ?? new AssetElementCollection();
            }
        }
    }
}