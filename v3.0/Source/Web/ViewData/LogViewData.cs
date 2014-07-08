using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kigg.Web
{
    public class LogToFbSynchronizeViewData : BaseViewData
    {
        LogToFbSynchronizeViewData() { WantFbSynchronize = false; }
        public bool WantFbSynchronize { get; set; }
    }
}