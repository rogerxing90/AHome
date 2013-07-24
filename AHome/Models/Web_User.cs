using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models.Base;

namespace AHome.Models
{
    public class Web_User : IEntity
    {
        public System.Int32 ID { get; set; }
        public System.String LOGNAME { get; set; }
        public System.String PASSWORD { get; set; }
        public System.String REALNAME { get; set; }
        public virtual Web_UserGroup GROUP { get; set; }
        public System.String STATE { get; set; }
        public System.DateTime? REG_DATE { get; set; }
        public System.DateTime? LAST_LOG_DATE { get; set; }
        public System.Int32? LOG_TIMES { get; set; }
        public System.String MEMO { get; set; }

        public override bool Equals(object obj)
        {
            Web_User target = obj as Web_User;
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