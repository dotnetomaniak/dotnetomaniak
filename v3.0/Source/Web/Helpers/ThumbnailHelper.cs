using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using Kigg.Infrastructure;

namespace Kigg.Web
{
    public static class ThumbnailHelper
    {

        private const string ThumbnailStoragePath = "/Data/Thumbnails/";
        private const string ThumbnailExtension = ".png";
        private const string ThumbnailSizeSmallPrefix = "small_";
        private const string ThumbnailSizeMediumPrefix = "medium_";

        public static void GenerateAndSaveThumbnailsForStory(string storyUrl, string shrinkedStoryId)
        {

           
            if(!ThumbnailExists(shrinkedStoryId, ThumbnailSize.Small))
            {
                var uri = IoC.Resolve<IThumbnail>().For(storyUrl, ThumbnailSize.Small);
                var thumbnail = CreateThumbnailFromUri(uri);
                SaveThumbnail(thumbnail, shrinkedStoryId, ThumbnailSize.Small);
            }

            if (!ThumbnailExists(shrinkedStoryId, ThumbnailSize.Medium))
            {
                var uri = IoC.Resolve<IThumbnail>().For(storyUrl, ThumbnailSize.Medium);
                var thumbnail = CreateThumbnailFromUri(uri);
                SaveThumbnail(thumbnail, shrinkedStoryId, ThumbnailSize.Medium);
            }

        }

        public static string GetThumbnailVirtualPathForStory(string shrinkedStoryId, ThumbnailSize size)
        {
            string path = ThumbnailStoragePath;

            if(size == ThumbnailSize.Small)
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
                thumbnail.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private static Image CreateThumbnailFromUri(string uri)
        {
            try
            {
                var request = WebRequest.Create(uri);
                request.Method = "GET";

                using (var response = request.GetResponse())
                {
                    var img = Image.FromStream(response.GetResponseStream());
                    return img;
                }
            }
            catch(Exception ex)
            {
                return null;
            }

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