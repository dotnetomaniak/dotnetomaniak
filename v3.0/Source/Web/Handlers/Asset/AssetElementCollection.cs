namespace Kigg.Web
{
    using System.Configuration;
    using System.Diagnostics;

    public class AssetElementCollection : ConfigurationElementCollection
    {
        public new AssetElement this[string name]
        {
            [DebuggerStepThrough]
            get
            {
                return BaseGet(name) as AssetElement;
            }
        }

        public void Add(AssetElement element)
        {
            BaseAdd(element);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssetElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssetElement) element).Name;
        }
    }
}