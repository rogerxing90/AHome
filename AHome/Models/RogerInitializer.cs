using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Common;

namespace AHome.Models
{
    public class RogerInitializer : DropCreateDatabaseIfModelChanges<RogerContext>
    {
        protected override void Seed(RogerContext context)
        {
            var newsTypes = new List<NewsType> 
            {
                new NewsType(){ Name="First"},
                new NewsType(){ Name = "Second"},
                new NewsType(){ Name = "Third"},
                new NewsType(){ Name = "Forth"},
                new NewsType(){ Name = "Fifth"},
                new NewsType(){ Name = "Sixth"}
            };
            newsTypes.ForEach(s => context.NewsTypes.Add(s));

            var members = new List<Member> 
            {
                new Member(){ UserName = "roger", Password = Tools.GetMD5("123456"), States="1", Email = "rogerxing90@gmail.com"},
                new Member(){ UserName = "frank", Password = Tools.GetMD5("123456"), States="0", Email = "leoxing90@sina.com"}
            
            };
            members.ForEach(s => context.Members.Add(s));



            context.SaveChanges();
        }
    }
}