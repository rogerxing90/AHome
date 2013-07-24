using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models.Base;

namespace AHome.Models
{
    public class Master : IEntity
    {
        public System.Int32 Id { get; set; }
        public System.String Username { get; set; }
        public System.String Password { get; set; }
        public System.String Name { get; set; }
        public System.String Introduction { get; set; }
        public System.String Isrecommend { get; set; }
        public System.String Isshow { get; set; }
        public System.String Picturepath { get; set; }
        public System.String Sex { get; set; }
        public System.String Nation { get; set; }
        public System.String mobilephone { get; set; }
        public System.String Telephone { get; set; }
        public System.String Email { get; set; }
        public System.String QQ { get; set; }
        public System.String Zipcode { get; set; }
        public System.String Address { get; set; }
        public System.String appreciation { get; set; }
        public System.String website { get; set; }
        public System.String Reward { get; set; }
        public System.DateTime BirthDay { get; set; }
        public System.String state { get; set; }
        public System.String state1 { get; set; }
        public System.Int64? hit { get; set; }
        public System.Int64? rank { get; set; }
        public System.String VCode { get; set; }
        public System.DateTime VTime { get; set; }
    }
}