using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AHome.BLL;
using AHome.Models;
using Common;

namespace AHome.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private CacheStorage.ICacheStorage Cache = CacheStorage.CacheFactory.CreateCacheFactory();

        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult DealLogin()
        {
            //string LoginIp = Request.UserHostAddress;//获取用户ip地址
            //string checkcode = Request["vdcode"];
            Web_User user = new Web_User();
            try
            {
                user.LOGNAME = Request["username"];
                user.PASSWORD = Request["userpass"];
                string sUser = Convert.ToString(Cache.Get(user.LOGNAME));
                //if (Session["checkcode"].ToString().Equals(checkcode))
                //{
                int groupID = new Web_UserBLL().IsLogin(user);
                if (groupID > 0)
                {
                    //这里要判断单点登录的情况
                    if (sUser == null || sUser == string.Empty)
                    {
                        TimeSpan SessTimeOut = new TimeSpan(0, 0, 1, 0, 0);//取得Session的过期时间System.Web.HttpContext.Current.Session.Timeout(这里设置为1分钟)
                        Cache.Insert(user.LOGNAME, user.LOGNAME, DateTime.MaxValue, SessTimeOut, System.Web.Caching.CacheItemPriority.NotRemovable);//将值放入cache以方便单点登录
                        user.GROUP = new Web_UserGroup();
                        user.GROUP.ID = groupID;
                        Session["User"] = user;
                        //new czcraft.BLL.SystemLogBLL().SaveSystemLog("登录成功!");

                        //logger.Info(user.LOGNAME + "登录成功!");
                        //  WriteSysLog log = new WriteSysLog();
                        // log.WriteSystemLog(LoginIp+"于"+DateTime.Now.ToString ()+"以"+user.LOGNAME+"帐号登录系统");
                        //JScript.JavaScriptLocationHref("../main.htm");
                        return RedirectToAction("Main", "Home");
                    }
                    else if (Cache.Get(sUser).ToString() == user.LOGNAME)//如果这个账号已经登录
                    {
                        JScript.Alert("该用户已登录!!");
                    }
                    else
                    {
                        Session.Abandon();//这段主要是为了避免不必要的错误导致不能登录
                    }
                }
                else
                {
                    JScript.Alert("帐号或密码错误,或者用户组被禁用!");
                }
                //}
                //else
                //    JScript.Alert("验证码出错!!");
            }
            catch (Exception ex)
            {
                //logger.Error("登录出错!登录ip:" + LoginIp + "登录时间:" + DateTime.Now.ToString(), ex);

                JScript.Alert("系统出错!");
            }
            return RedirectToAction("Login");
        }

        public string DealExit()
        {
            Web_User user = (Web_User)Session["User"];
            Cache.Remove(user.LOGNAME);
            Session.Remove("User");
            return "安全退出成功!";
        }

        public string DealMenu()
        {
            //优化：此处取session中存的user
            //(WEB_USER)context.Session["User"]
            Web_User user = new Web_User();
            user.LOGNAME = "tianzhuanghu";
            user.PASSWORD = "tian815100";
            user.GROUP = new Web_UserGroup();
            user.GROUP.ID = 1;

            return new Web_UserBLL().GetMenuByJson(user);
 
        }

    }
}
