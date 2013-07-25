using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class SystemLog
    {
        public System.Int32 Id { get; set; }
        public System.String Title { get; set; }
        public System.DateTime? AddTime { get; set; }
        public System.String Url { get; set; }
        public System.String UserName { get; set; }
    }
}