namespace Kigg.Web
{
    using System.Web.Mvc;

    public static class UrlHelperExtension
    {
        public static string Asset(this UrlHelper helper, string assetName)
        {
            return helper.Content("~/asset.axd") + "?name={0}&v={1}".FormatWith(assetName, AssetHandler.GetVersion(assetName));
        }

        public static string Image(this UrlHelper helper, string fileName)
        {
            return helper.Content("~/Assets/Images/{0}".FormatWith(fileName));//http://static.dotnetomaniak.pl
        }

        public static string File(this UrlHelper helper, string fileName)
        {
            return helper.Content("~/Assets/Files/{0}".FormatWith(fileName));
        }
    }
}