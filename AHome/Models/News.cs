using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class News
    {
        public System.Int32? Id { get; set; }
        public System.String Title { get; set; }
        public System.String Content { get; set; }
        //public System.Int32? Typeid {get;set;}
        public NewsType Type { get; set; }
        public System.DateTime? Time { get; set; }
        public System.String ArticleHtmlUrl { get; set; }
    }
}