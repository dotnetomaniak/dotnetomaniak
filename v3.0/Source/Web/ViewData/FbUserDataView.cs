using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kigg.Web.ViewData
{
    public class FbUserDataView
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string First_name { get; set; }
        public string Gender { get; set; }
        public string Last_name { get; set; }
        public string Link { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public int Timezone { get; set; }
        public string Updated_time { get; set; }
        public bool Verified { get; set; }
    }
}