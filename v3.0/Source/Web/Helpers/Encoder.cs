using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kigg.Core.Infrastructure.Thumbnail;

namespace Kigg.Web.Helpers
{
    public class Encoder : IEncoder
    {
        private string _url;

        public Encoder(string url)
        {
            Check.Argument.IsNotEmpty(url,"url");
            _url = url;
        }

        public string EncodeUrl(string _url)
        {
            return HttpUtility.UrlEncode(_url);
        }
    }
}