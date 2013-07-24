using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models.Base;

namespace AHome.Models
{
    public class VProductCraftType : IEntity
    {
        public System.Int64? Id { get; set; }
        //CraftType的
        //public System.String TypeName { get; set; }
        //public System.Int32? TypeId { get; set; }

        public virtual CraftType CraftType { get; set; }

        public System.String FId { get; set; }
        public System.Int64 rank { get; set; }
        public System.Int64 hit { get; set; }
        public System.String Simplename { get; set; }
        public System.String Name { get; set; }
        public System.String Explain { get; set; }
        public System.String Picturepath { get; set; }
        //public System.Int32? Masterid { get; set; }
        public virtual Master Master { get; set; }

        public System.Int32? Belongstype { get; set; }
        public System.Int32? Companyid { get; set; }
        public System.Double? Lsprice { get; set; }
        public System.Int64? Num { get; set; }
        public System.Int64? Soldnum { get; set; }
        public System.Double? Pfprice { get; set; }
        public System.String Isshow { get; set; }
        public System.String Isrecomment { get; set; }
        public System.String Nongenetic { get; set; }
        public System.String Isexcellent { get; set; }
        public System.String Issell { get; set; }
    }
}