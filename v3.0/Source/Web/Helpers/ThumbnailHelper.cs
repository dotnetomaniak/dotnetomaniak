using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using Kigg.Infrastructure;

namespace Kigg.Web
{
    public static class ThumbnailHelper
    {

        private const string ThumbnailHost = "http://dotnetomaniak.pl";
        private const string ThumbnailStoragePath = "/Data/Thumbnails/";
        private const string ThumbnailExtension = ".png";
        private const string ThumbnailSizeSmallPrefix = "small_";
        private const string ThumbnailSizeMediumPrefix = "medium_";
        private const string BlankThumbnailImageName = "blank_thumbnail.png";



        public static string GetThumbnailVirtualPathForStoryOrCreateNew(string storyUrl, string shrinkedStoryId, ThumbnailSize size, bool createMediumThumbnail = false, bool fullPath = false, bool doNotCheckForExistingMiniature = false)
        {
            if (!ThumbnailExists(shrinkedStoryId, ThumbnailSize.Small) || doNotCheckForExistingMiniature)
            {
                var uri = IoC.Resolve<IThumbnail>().For(storyUrl, ThumbnailSize.Small);
                var thumbnail = CreateThumbnailFromUri(uri);
                SaveThumbnail(thumbnail, shrinkedStoryId, ThumbnailSize.Small);
            }

            if (createMediumThumbnail && (!ThumbnailExists(shrinkedStoryId, ThumbnailSize.Medium) || doNotCheckForExistingMiniature))
            {
                var uri = IoC.Resolve<IThumbnail>().For(storyUrl, ThumbnailSize.Medium);
                var thumbnail = CreateThumbnailFromUri(uri);
                SaveThumbnail(thumbnail, shrinkedStoryId, ThumbnailSize.Medium);
            }

            return GetThumbnailVirtualPathForStory(shrinkedStoryId, size, fullPath);
        }

        public static string GetThumbnailVirtualPathForStory(string shrinkedStoryId, ThumbnailSize size, bool fullPath = false)
        {
            string path = ((fullPath) ? ThumbnailHost : string.Empty) + ThumbnailStoragePath;

            if (size == ThumbnailSize.Small)
                path += ThumbnailSizeSmallPrefix;
            if (size == ThumbnailSize.Medium)
                path += ThumbnailSizeMediumPrefix;

            path += shrinkedStoryId + ThumbnailExtension;

            return path;
        }


        private static void SaveThumbnail(Image thumbnail, string shrinkedStoryId, ThumbnailSize size)
        {
            if (thumbnail != null)
            {
                string fullPath = GenerateThumbnailFullPath(shrinkedStoryId, size);
                thumbnail.Save(fullPath, ImageFormat.Png);
            }
        }

        private static Image CreateThumbnailFromUri(string uri)
        {
            Image img = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        img = Image.FromStream(response.GetResponseStream());
                    }
                }
            }
            catch (Exception ex) { /*404 throws exception :/ */  }
            if (img == null)
            {
                var blankThumbnailPath = Path.Combine(HttpContext.Current.Server.MapPath(ThumbnailStoragePath), BlankThumbnailImageName);
                img = new Bitmap(blankThumbnailPath);
            }
            return img;
        }

        private static bool ThumbnailExists(string shrinkedStoryId, ThumbnailSize size)
        {
            string fullPath = GenerateThumbnailFullPath(shrinkedStoryId, size);
            return File.Exists(fullPath);
        }

        private static string GenerateThumbnailFullPath(string shrinkedStoryId, ThumbnailSize size)
        {
            string fileName = "";
            if (size == ThumbnailSize.Small)
                fileName = ThumbnailSizeSmallPrefix;
            if (size == ThumbnailSize.Medium)
                fileName = ThumbnailSizeMediumPrefix;

            fileName += shrinkedStoryId + ThumbnailExtension;

            return Path.Combine(HttpContext.Current.Server.MapPath(ThumbnailStoragePath), fileName);
        }
    }
}