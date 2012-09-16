namespace Kigg.Web
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;

    using Infrastructure;

    public class AssetHandler : BaseHandler
    {
        public AssetHandler()
        {
            IoC.Inject(this);
        }

        public IConfigurationManager Configuration
        {
            get;
            set;
        }

        public IFile FileReader
        {
            get;
            set;
        }

        public static string GetVersion(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            ICache cache = IoC.Resolve<ICache>();

            string cacheKey = "assetVersion:{0}".FormatWith(name);
            string version;

            cache.TryGet(cacheKey, out version);

            if (string.IsNullOrEmpty(version))
            {
                IConfigurationManager configuration = IoC.Resolve<IConfigurationManager>();

                AssetElement setting = GetSetting(configuration, name);

                version = setting.Version;

                if ((setting.CacheDurationInDays > 0) && (!cache.Contains(cacheKey)))
                {
                    cache.Set(cacheKey, version, SystemTime.Now().AddDays(setting.CacheDurationInDays));
                }
            }

            return version;
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            string assetName = context.Request.QueryString["name"];

            if (!string.IsNullOrEmpty(assetName))
            {
                AssetElement setting = GetSetting(Configuration, assetName);

                if (setting != null)
                {
                    HandlerCacheItem asset = GetAsset(context, setting);

                    if (asset != null)
                    {
                        if (setting.GenerateETag)
                        {
                            if (HandleIfNotModified(context, asset.ETag))
                            {
                                return;
                            }
                        }

                        HttpResponseBase response = context.Response;

                        // Set the content type
                        response.ContentType = setting.ContentType;

                        // Compress
                        if (setting.Compress)
                        {
                            context.CompressResponse();
                        }

                        // Write
                        using (StreamWriter sw = new StreamWriter(response.OutputStream))
                        {
                            sw.Write(asset.Content);
                        }

                        // Cache
                        if (setting.CacheDurationInDays > 0)
                        {
                            // Helpful when hosting in Single Web server
                            if (setting.GenerateETag)
                            {
                                response.Cache.SetETag(asset.ETag);
                            }

                            context.CacheResponseFor(TimeSpan.FromDays(setting.CacheDurationInDays));
                        }
                    }
                }
            }
        }

        private static AssetElement GetSetting(IConfigurationManager configuration, string assetName)
        {
            AssetSettingsSection settings = configuration.GetSection<AssetSettingsSection>(AssetSettingsSection.SectionName);
            AssetElement setting = settings.Assets[assetName];
            AssetElement clone = null;

            if (setting != null)
            {
                clone = new AssetElement
                                  {
                                      Name = setting.Name,
                                      ContentType = setting.ContentType,
                                      Compress = setting.Compress,
                                      GenerateETag = setting.GenerateETag,
                                      Version = setting.Version,
                                      CacheDurationInDays = setting.CacheDurationInDays,
                                      Directory = setting.Directory,
                                      Files = setting.Files
                                  };

                // Assign Global value if Value is default
                if ((clone.Compress == AssetSettingsSection.DefaultCompress) && (settings.Compress != AssetSettingsSection.DefaultCompress))
                {
                    clone.Compress = settings.Compress;
                }

                // Assign Global value if Value is default
                if ((clone.GenerateETag == AssetSettingsSection.DefaultGenerateETag) && (settings.GenerateETag != AssetSettingsSection.DefaultGenerateETag))
                {
                    clone.GenerateETag = settings.GenerateETag;
                }

                // Assign Global value if Value is default
                if ((clone.CacheDurationInDays == AssetSettingsSection.DefaultCacheDurationInDays) && (settings.CacheDurationInDays != AssetSettingsSection.DefaultCacheDurationInDays))
                {
                    clone.CacheDurationInDays = settings.CacheDurationInDays;
                }

                // Assign the global version if setting does not have any version
                if (string.IsNullOrEmpty(clone.Version))
                {
                    clone.Version = settings.Version;
                }
            }

            return clone;
        }

        private HandlerCacheItem GetAsset(HttpContextBase context, AssetElement setting)
        {
            string key = "cache:{0}".FormatWith(setting.Name);
            HandlerCacheItem asset;

            Cache.TryGet(key, out asset);

            if (asset == null)
            {
                string[] files = setting.Files.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (files.Length > 0)
                {
                    StringBuilder contentBuilder = new StringBuilder();

                    for (int i = 0; i < files.Length; i++)
                    {
                        string file = context.Server.MapPath(Path.Combine(setting.Directory, files[i]));
                        string fileContent = FileReader.ReadAllText(file);

                        if (!string.IsNullOrEmpty(fileContent))
                        {
                            contentBuilder.AppendLine(fileContent);
                            contentBuilder.AppendLine();
                        }
                    }

                    string content = contentBuilder.ToString();

                    if (!string.IsNullOrEmpty(content))
                    {
                        asset = new HandlerCacheItem { Content = content };

                        if ((setting.CacheDurationInDays > 0) && (!Cache.Contains(key)))
                        {
                            Cache.Set(key, asset, SystemTime.Now().AddDays(setting.CacheDurationInDays));
                        }
                    }
                }
            }

            return asset;
        }
    }
}