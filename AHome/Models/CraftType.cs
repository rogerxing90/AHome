using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using AHome.Models.Base;

namespace AHome.Models
{
    public class CraftType : IEntity
    {
        public System.Int32 ID { get; set; }
        public System.String Name { get; set; }
        public System.Int32? level { get; set; }
        public System.Int32 Belongsid { get; set; }
        public System.String IsLeaf { get; set; }
        public System.String FId { get; set; }
    }
}