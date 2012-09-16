namespace Kigg.Web
{
    using System;
    using System.IO.Compression;
    using System.Web;

    public static class HttpContextExtension
    {
        public static void CacheResponseFor(this HttpContextBase context, TimeSpan duration)
        {
            HttpCachePolicyBase cache = context.Response.Cache;

            cache.SetCacheability(HttpCacheability.Public);
            cache.SetLastModified(context.Timestamp);
            cache.SetExpires(context.Timestamp.Add(duration));
            cache.SetMaxAge(duration);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        }

        public static void CompressResponse(this HttpContextBase context)
        {
            HttpRequestBase request = context.Request;

            string acceptEncoding = (request.Headers["Accept-Encoding"] ?? string.Empty).ToUpperInvariant();

            HttpResponseBase response = context.Response;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}