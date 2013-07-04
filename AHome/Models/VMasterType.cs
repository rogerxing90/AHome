using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class VMasterType
    {
        public System.Int32 Id { get; set; }
        public System.String TypeName { get; set; }
        public System.Int32? Belongsid { get; set; }
        public System.Int32? level { get; set; }
        public System.String IsLeaf { get; set; }
        public System.String FId { get; set; }
        public virtual Master Master { get; set; }
        public System.Int32? m_TypeId { get; set; }
    }
}