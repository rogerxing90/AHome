using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models.Base;

namespace AHome.Models
{
    public class Master_Type : IEntity
    {
        public System.Int32 Id { get; set; }
        public virtual Master Master { get; set; }
        public virtual CraftType CragtType { get; set; }
    }
}