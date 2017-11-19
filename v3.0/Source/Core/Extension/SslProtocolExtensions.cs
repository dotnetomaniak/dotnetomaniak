using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;

namespace Kigg.Core.Extension
{
    public static class SslProtocolExtensions
    {
        public const SecurityProtocolType Tls12 = (SecurityProtocolType)0x00000C00;
        public const SecurityProtocolType Tls11 = (SecurityProtocolType)0x00000300;
    }
}
