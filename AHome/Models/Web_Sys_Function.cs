using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class Web_Sys_Function
    {
        [Key]
        public System.Int32 FUNTION_ID { get; set; }
        public System.String NAME { get; set; }
        public System.String URL { get; set; }
        public System.Int32 FATHER_ID { get; set; }
        public System.String STATE { get; set; }
        public System.String DESCRIPTION { get; set; }
    }
}