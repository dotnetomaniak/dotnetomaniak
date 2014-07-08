using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kigg.Web.ViewData
{
    public class LogToFbSynchronizeViewData : BaseViewData
    {
        public string Id { get; set; }
        public bool SynchronizeWithFb { get; set; }
    }
}