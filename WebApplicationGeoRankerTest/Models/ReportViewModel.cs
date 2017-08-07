using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationGeoRankerTest.Models
{
    public class ReportViewModel
    {
        
        public string url { get; set; }
        public string email { get; set; }
        public string session { get; set; }
        public string searchEngines { get; set; }
        public string type { get; set; }
        public string countries { get; set; }
        public string keywords { get; set; }


    }
}