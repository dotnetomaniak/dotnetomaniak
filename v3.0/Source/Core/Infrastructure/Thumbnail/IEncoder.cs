using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kigg.Core.Infrastructure.Thumbnail
{
    public interface IEncoder
    {
        string EncodeUrl(string url);
    }
}
