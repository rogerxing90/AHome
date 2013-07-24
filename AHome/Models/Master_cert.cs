using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models.Base;

namespace AHome.Models
{
    public class Master_cert : IEntity
    {
        public System.Int64 Id { get; set; }
        public System.String Name { get; set; }
        public System.String Picpath { get; set; }
        public virtual Master Master { get; set; }
    }
}