using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class Web_User
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
    }
}