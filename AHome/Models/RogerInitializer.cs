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
    public class RogerInitializer : DropCreateDatabaseAlways<RogerContext>
    {
        private RogerContext _context = new RogerContext();

        protected override void Seed(RogerContext context)
        {
            _context = context;
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
                new CraftType(){ Name = "木雕1",level =3,Belongsid=7,IsLeaf="1",FId ="F010202" },
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


            //创建管理员User
            var web_Users = new List<Web_User> 
            {
                new Web_User(){ LOGNAME="admin", PASSWORD="admin", STATE="1",
                    GROUP= GetGroupsByType(GroupType.SuperAdministrator).First()},
            };
            web_Users.ForEach(s => context.Web_Users.Add(s));
            context.SaveChanges();


            //初始化Groups
            var Web_UserGroups = GetGroupsByType(GroupType.All);
            foreach (Web_UserGroup group in Web_UserGroups)
            {
                if (!context.Web_UserGroups.Local.Contains(group))
                {
                    context.Web_UserGroups.Add(group);
                }
            }
            context.SaveChanges();

            //初始化Functions
            var Web_Sys_Functions = GetFunctionsByGroupType(GroupType.All);
            foreach (Web_Sys_Function func in Web_Sys_Functions)
            {
                if (!context.Web_Sys_Functions.Local.Contains(func))
                {
                    context.Web_Sys_Functions.Add(func);
                }
            }

            context.SaveChanges();

            

            //Web_UserGroups[0].Web_Sys_Functions = GetFunctionsByGroupType(GroupType.SuperAdministrator);
            //Web_UserGroups[1].Web_Sys_Functions = GetFunctionsByGroupType(GroupType.Manager);
            //context.SaveChanges();

            
        }

        private List<Web_UserGroup> GetGroupsByType(GroupType type)
        {
            List<Web_UserGroup> groups = new List<Web_UserGroup>()
            {
                new Web_UserGroup(){ Group_ID = 1, USERGROUP = "超级管理员", DESCRIPTION = "Super Administrator", Web_Sys_Functions=GetFunctionsByGroupType(GroupType.SuperAdministrator)},
                //new Web_UserGroup(){ Group_ID = 2, USERGROUP = "普通管理员",DESCRIPTION = "Manager", Web_Sys_Functions= new List<Web_Sys_Function>()}
            };//_context.Web_Sys_Functions.Where(f => f.ID == 101 || f.ID == 201).ToList()

            switch (type)
            {
                case GroupType.SuperAdministrator:
                    return groups.Where(s => s.Group_ID == 1).ToList();
                case GroupType.Manager:
                    return groups.Where(s => s.Group_ID == 2).ToList();
                default:
                    return groups;
            }
        }

        private List<Web_Sys_Function> GetFunctionsByGroupType(GroupType type)
        {
            List<Web_Sys_Function> functions = new List<Web_Sys_Function>() 
            {
                new Web_Sys_Function() { ID = 1, NAME = "管理员", FATHER_ID = 0, DESCRIPTION = "一级菜单"},
                new Web_Sys_Function() { ID = 2, NAME = "系统功能", FATHER_ID = 0, DESCRIPTION = "一级菜单" },
                new Web_Sys_Function() { ID = 3, NAME = "用户组", FATHER_ID = 0, DESCRIPTION = "一级菜单" },
                new Web_Sys_Function() { ID = 4, NAME = "用户组功能", FATHER_ID = 0, DESCRIPTION = "一级菜单" },
                new Web_Sys_Function() { ID = 5, NAME = "类别管理", FATHER_ID = 0, DESCRIPTION = "一级菜单"},
                new Web_Sys_Function() { ID = 6, NAME = "文章管理", FATHER_ID = 0, DESCRIPTION = "一级菜单"},
                new Web_Sys_Function() { ID = 7, NAME = "客户管理", FATHER_ID = 0, DESCRIPTION = "一级菜单"},
                new Web_Sys_Function() { ID = 8, NAME = "产品管理", FATHER_ID = 0, DESCRIPTION = "一级菜单" },
                new Web_Sys_Function() { ID = 9, NAME = "管理中心", FATHER_ID = 0, DESCRIPTION = "一级菜单" },


                new Web_Sys_Function() { ID = 101, NAME = "管理员信息管理", URL = "/Admin/Manage/UserInfo", FATHER_ID = 1, DESCRIPTION = "二级菜单"},
                new Web_Sys_Function() { ID = 201, NAME = "系统功能管理", URL = "Manage/UserInfo.html", FATHER_ID = 2, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 301, NAME = "用户组管理", URL = "/Admin/Manage/UserGroupInfo", FATHER_ID = 3, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 401, NAME = "用户组功能管理", URL = "Manage/UserInfo.html", FATHER_ID = 4, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 501, NAME = "产品类别管理", URL = "Manage/UserInfo.html", FATHER_ID = 5, DESCRIPTION = "二级菜单"},
                new Web_Sys_Function() { ID = 502, NAME = "新闻类别管理", URL = "Manage/UserInfo.html", FATHER_ID = 5, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 601, NAME = "新闻管理", URL = "Manage/UserInfo.html", FATHER_ID = 6, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 602, NAME = "工艺知识管理", URL = "Manage/UserInfo.html", FATHER_ID = 6, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 701, NAME = "企业信息管理", URL = "Manage/UserInfo.html", FATHER_ID = 7, DESCRIPTION = "二级菜单"},
                new Web_Sys_Function() { ID = 702, NAME = "大师信息管理", URL = "Manage/UserInfo.html", FATHER_ID = 7, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 703, NAME = "会员管理", URL = "Manage/UserInfo.html", FATHER_ID = 7, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 801, NAME = "产品信息管理", URL = "Manage/UserInfo.html", FATHER_ID = 8, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 901, NAME = "公司信息", URL = "Manage/UserInfo.html", FATHER_ID = 9, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 902, NAME = "管理中心首页", URL = "Manage/UserInfo.html", FATHER_ID = 9, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 903, NAME = "系统日志管理", URL = "Manage/UserInfo.html", FATHER_ID = 9, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 904, NAME = "缓存管理", URL = "Manage/UserInfo.html", FATHER_ID = 9, DESCRIPTION = "二级菜单" },
                new Web_Sys_Function() { ID = 905, NAME = "页面静态化管理", URL = "Manage/UserInfo.html", FATHER_ID = 9, DESCRIPTION = "二级菜单" },
            };


            switch (type)
            {
                case GroupType.Manager:
                    return functions.Where(f => f.ID == 101 || f.ID == 201).ToList();
                case GroupType.SuperAdministrator:
                    return functions;
                default:
                    return functions;
            }
        }
    }

    public enum GroupType
    {
        SuperAdministrator,

        Manager,

        All
    }
}