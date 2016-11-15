using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using Kigg.Infrastructure;
using System.Diagnostics;
using Browshot;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

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



        public static string GetThumbnailVirtualPathForStoryOrCreateNew(string storyUrl, string shrinkedStoryId, ThumbnailSize size, bool createMediumThumbnail = false, bool fullPath = false, bool doNotCheckForExistingMiniature = false, bool async = false)
        {

            if (createMediumThumbnail && (!ThumbnailExists(shrinkedStoryId, ThumbnailSize.Medium) || doNotCheckForExistingMiniature))
            {
                if (!async)
                    return "";
                var uri = IoC.Resolve<IThumbnail>().For(storyUrl, ThumbnailSize.Medium);
                var thumbnail = CreateThumbnailFromUri(uri);
                SaveThumbnail(thumbnail, shrinkedStoryId, ThumbnailSize.Medium);
            }

            if (!ThumbnailExists(shrinkedStoryId, ThumbnailSize.Small) || doNotCheckForExistingMiniature)
            {
                if (!async)
                    return "";
                var uri = IoC.Resolve<IThumbnail>().For(storyUrl, ThumbnailSize.Small);
                var thumbnail = CreateThumbnailFromUri(uri);
                SaveThumbnail(thumbnail, shrinkedStoryId, ThumbnailSize.Small);
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
                var queryString = uri.Substring(uri.IndexOf('?')).Split('#')[0];
                var uriParams = HttpUtility.ParseQueryString(queryString);
                var key = uriParams["key"];
                var url = uriParams["url"];
                var hashtable = new Hashtable() {
                     { "instance_id", uriParams["instance_id"] },
                     { "height", uriParams["height"] },
                     {"shots", 1 },
                     {"delay", 5 },
                     {"shot_interval", 5 }
                };
                if (uriParams["width"] != null)
                {
                    hashtable.Add("width", uriParams["width"]);
                }
                var browshot = new BrowshotClient(key);
                Dictionary<string, object> results = new Dictionary<string, object>();
                int tried = 0;
                while (true)
                {
                    Debug.WriteLine("Screenshot for " + url + " - attempt: " + tried);
                    results = browshot.ScreenshotCreate(url, hashtable);
                    tried++;
                    if (results.ContainsKey("id"))
                        Debug.WriteLine("ID: " + results["id"]);
                    if (results.ContainsKey("status"))
                        Debug.WriteLine("Status: " + results["status"]);
                    if (results.ContainsKey("error") && results["error"].ToString().Length > 0)
                    {
                        Debug.WriteLine("Status: " + results["error"]);
                        if (tried > 3)
                        {
                            Debug.WriteLine("Too many retry, give up");
                            break;
                        }
                        if (results.ContainsKey("id") == false)
                            break;

                        // try again
                        continue;
                    }

                    // finished or in_process
                    if (results["status"].ToString().StartsWith("finished") == false)
                    {
                        int wait = (int)hashtable["delay"] + (int)hashtable["shots"] * (int)hashtable["shot_interval"] + 10;
                        Console.WriteLine(String.Format("Waiting {0} seconds...", wait));
                        Thread.Sleep(wait * 1000);
                    }
                    break;
                }

                while (results["status"].ToString().StartsWith("finished") == false && results["status"].ToString().StartsWith("error") == false)
                {
                    results = browshot.ScreenshotInfo(int.Parse(results["id"].ToString()));

                    int wait = (int)hashtable["delay"] + (int)hashtable["shots"] * (int)hashtable["shot_interval"] + 10;
                    Debug.WriteLine(String.Format("Waiting {0} seconds...", wait));
                    Thread.Sleep(wait * 1000);

                    if (results["status"].ToString().StartsWith("error"))
                    {
                        Debug.WriteLine("Screenshot failed");
                        if (results.ContainsKey("error") && results["error"].ToString().Length > 0)
                            Debug.WriteLine("Status: " + results["error"]);
                        break;
                    }
                }

                Debug.WriteLine("Screenshot ID: " + results["id"].ToString());

                // finished
                hashtable.Add("shot", 1);
                for (int i = 1; i <= (int)hashtable["shots"]; i++)
                {
                    hashtable["shot"] = i;
                    img = browshot.Thumbnail(int.Parse(results["id"].ToString()), hashtable);
                    if (img == null)
                    {
                        Debug.WriteLine("Could not retrieve image for shot " + hashtable["shot"]);
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex) { }
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