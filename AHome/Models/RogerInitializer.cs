using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
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

            var craftTypes = new List<CraftType>
            {
                new CraftType(){ Name="潮州木雕", level=1,Belongsid=-1, IsLeaf="0", FId ="F01" },
                new CraftType(){ Name = "花木雕",level =2,Belongsid=1,IsLeaf="0",FId ="F0101" },
                new CraftType(){ Name = "红花木雕",level =3,Belongsid=2,IsLeaf="0",FId ="F010101" },
                new CraftType(){ Name = "潮州刺绣",level =1,Belongsid=-1,IsLeaf="1",FId ="F04" },
                new CraftType(){ Name = "石雕",level =1,Belongsid=-1,IsLeaf="1",FId ="F05" },
                new CraftType(){ Name = "大吴泥塑",level =1,Belongsid=-1,IsLeaf="1",FId ="F06" },
                new CraftType(){ Name = "木雕",level =2,Belongsid=1,IsLeaf="0",FId ="F0102" },
                new CraftType(){ Name = ",木雕1",level =3,Belongsid=7,IsLeaf="1",FId ="F010202" },
            };
            craftTypes.ForEach(s => context.CraftTypes.Add(s));

            #region ==Sql读取工程下CSV文件导入到数据库中==

            //string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "App_Data\\DBCSVFile\\province_unicode.csv";
            //string mySqlPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "App_Data\\DBCSVFile\\province_UTF8.csv";
            ////string sql = string.Format("BULK INSERT [Provinces] FROM '{0}' WITH( FIELDTERMINATOR =',', ROWTERMINATOR = '\n')", path);
            //string mysql = string.Format("load data local infile '{0}'  into table roger.provinces   fields terminated by ','    lines terminated by '\r\n'", mySqlPath);
            //DbParameter[] paras = new DbParameter[]
            //{
            //    new SqlParameter("csvPath" , path),
            //};

            //string sql1 = "BULK INSERT [Provinces] FROM '" + @path + "' WITH( FIELDTERMINATOR =',', ROWTERMINATOR = '\n')";
            //try
            //{
            //    //context.Database.ExecuteSqlCommand(sql1,paras);
                
            //    context.Database.ExecuteSqlCommand(mysql);
            //}
            //catch (Exception e)
            //{

            //}
            #endregion


            var web_Users = new List<Web_User> 
            {
                new Web_User(){ LOGNAME="admin", PASSWORD="admin", STATE="1",
                    GROUP= new Web_UserGroup(){ USERGROUP = "超级管理员", DESCRIPTION = "Super Administrator"}},
            };
            web_Users.ForEach(s => context.Web_Users.Add(s));

            context.SaveChanges();
        }
    }
}