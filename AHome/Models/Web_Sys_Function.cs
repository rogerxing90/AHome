using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class Web_Sys_Function
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public System.Int32 ID { get; set; }
        public System.String NAME { get; set; }
        public System.String URL { get; set; }
        public System.Int32 FATHER_ID { get; set; }
        public System.String STATE { get; set; }
        public System.String DESCRIPTION { get; set; }
        public virtual Web_UserGroup GROUP { get; set; }

        public override bool Equals(object obj)
        {
            Web_Sys_Function target = obj as Web_Sys_Function;
            if (target != null)
            {
                return this.ID == target.ID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

    }
}