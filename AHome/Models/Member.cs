using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models.Base;

namespace AHome.Models
{
    public class Member : IEntity
    {
        public System.Int32 Id { get; set; }
        public System.String UserName { get; set; }
        public System.String Password { get; set; }
        public System.String Sex { get; set; }
        public System.String nation { get; set; }
        public System.String mobilephone { get; set; }
        public System.String Telephone { get; set; }
        public System.String Email { get; set; }
        public System.String qq { get; set; }
        public System.String Zipcode { get; set; }
        public System.String Address { get; set; }
        public System.String States { get; set; }
        public System.String VCode { get; set; }
        public System.DateTime? VTime { get; set; }
        public Member()
        {
            UserName = "";
            Password = "";
            Sex = "";
            nation = "";
            mobilephone = "";
            Telephone = "";
            Email = "";
            qq = "";
            Zipcode = "";
            States = "";
            Address = "";

        }
    }
}