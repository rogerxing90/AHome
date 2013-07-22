using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class Web_UserGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Int32 Group_ID { get; set; }
        public System.String USERGROUP { get; set; }
        public System.String DESCRIPTION { get; set; }
        //与实例不同，此处直接用外键，没有另外建表 WEB_USERGROUP_FUNCTIONS
        public virtual List<Web_Sys_Function> Web_Sys_Functions { get; set; }

        //此处表示一个usergoup包含多个user
        public virtual List<Web_User> Web_Users { get; set; }

        public override bool Equals(object obj)
        {
            Web_UserGroup target = obj as Web_UserGroup;
            if (target != null)
            {
                return this.Group_ID == target.Group_ID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.Group_ID.GetHashCode();
        }

    }

}