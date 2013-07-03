using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class Master_cert
    {
        public System.Int64 Id { get; set; }
        public System.String Name { get; set; }
        public System.String Picpath { get; set; }
        public virtual Master Master { get; set; }
    }
}