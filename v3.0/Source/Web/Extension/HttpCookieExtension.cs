namespace Kigg.Web
{
    using System;
    using System.Web;

    public static class HttpCookieExtension
    {
        public static void Expire(this HttpCookie cookie)
        {
            cookie.Expires = DateTime.Now.AddMinutes(-1);
        }
    }
}