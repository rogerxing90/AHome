using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AHome.BLL;
using AHome.Models;
using Common;

namespace AHome.Controllers
{
    public class MasterController : Controller
    {
        private MasterBLL masterBLL = new MasterBLL();
        //
        // GET: /MasterLogin/

        public ActionResult MasterLogin()
        {
            return View();
        }

        public ActionResult MasterInfo()
        {
            return View();
        }

        public ActionResult MasterRegister()
        {
            return View();
        }

        public ActionResult MasterList()
        {
            return View();
        }

        public string CheckExistUserName()
        {
            string username = Request["username"];
            if (Tools.IsValidInput(ref username, true))
            {
                return masterBLL.CheckExistUserName(username) ? "True" : "False"; ;
            }
            else
            {
                return string.Empty;
            }

        }


        /// <summary>
        /// 大师登录
        /// </summary>
        public string MasterAjaxLogin()
        {
            string ReturnJson = BLLUtil.WriteJsonForReturn(false, "");
            try
            {
                //获取数据
                string Name = Request["Name"];
                string Pwd = Request["Pwd"];
                string IsSaveName = Request["cbName"];
                string IsSavePwd = Request["cbPwd"];
                //用户登录状态
                bool Status = false;
                //返回给客户端的json数据
                //sql注入检测
                if (Tools.IsValidInput(ref Name, true) && (Tools.IsValidInput(ref Pwd, true)) && (Tools.IsValidInput(ref IsSaveName, true)) && (Tools.IsValidInput(ref IsSavePwd, true)))
                {
                    Master info = new Master();
                    info.Username = Name;
                    info.Password = Pwd;
                    ReturnJson = masterBLL.ReturnJson(info, out Status);
                    if (Status) //如果成功登陆
                    {
                        //记住帐号和密码
                        masterBLL.RememberUserInfo(info, masterBLL.GetRememberType(IsSaveName, IsSavePwd));

                        //保存登录状态
                        Session["MasterName"] = info.Username;
                        //如果登录成功,则把用户ID放在Session中
                        if (Tools.IsNullOrEmpty(Session["MasterId"]))
                        {
                            Session["MasterId"] = masterBLL.GetMasterId(info.Username);
                        }
                    }
                }
                return ReturnJson;
            }
            catch (Exception ex)
            {
                //logger.Error("大师登录出错!", ex);
                return string.Empty;
            }

        }

        public string CheckLoginStatus()
        {
            string MasterName = (string)Session["MasterName"];
            string ReturnJson;
            bool Status = false;
            if (!Tools.IsNullOrEmpty(MasterName))
            {
                //如果session存在,直接返回用户状态
                ReturnJson = BLLUtil.WriteJsonForReturn(true, MasterName);
            }
            else
            {
                //用户自动登录状态检测
                ReturnJson = masterBLL.CheckLoginStatus(out Status);
                //登录成功!
                if (Status)
                {
                    Session["MasterName"] = Common.CookieHelper.GetCookieValue("MasterName");
                }
            }
            //如果登录成功,则把用户ID放在Session中
            if (Status == true && Tools.IsNullOrEmpty(Session["MasterId"]))
            {
                Session["MasterId"] = masterBLL.GetMasterId(MasterName);
            }
            return ReturnJson;
        }

        /// <summary>
        /// 保存大师信息
        /// </summary>
        public string SaveMasterInfo()
        {
            string ReturnJson =string.Empty; 
            try
            {
                //表单读取
                string UserName = Request["UserName"];
                string Pwd = Request["Pwd"];
                string Birthday = Request["Birthday"];
                string Name = Request["Name"];
                string Sex = Request["Sex"];
                string MobilePhone = Request["MobilePhone"];
                string TelePhone = Request["TelePhone"];
                string QQ = Request["QQ"];
                string Introduce = Request["Introduce"];
                string Email = Request["Email"];
                //string CheckCode = Request["CheckCode"];
                string PicturePath = Request["Picturepath"];

                //验证码校验
                //if (Tools.IsNullOrEmpty(Session["checkcode"]) || !CheckCode.Equals(Session["checkcode"].ToString()))
                //{
                //    ReturnJson =Tools.WriteJsonForReturn(false, "验证码错误!");
                //}
                //字符串sql注入检测
                if (Tools.IsValidInput(ref UserName, true) && Tools.IsValidInput(ref Pwd, true) && Tools.IsValidInput(ref Email, true))
                {
                    //元素赋值
                    Master info = new Master();
                    info.Username = UserName;
                    info.Email = Email;
                    info.Name = Name;
                    info.Password = Tools.GetMD5(Pwd);
                    info.BirthDay = Convert.ToDateTime(Birthday);
                    info.Introduction = Introduce;
                    info.mobilephone = MobilePhone;
                    info.Picturepath = PicturePath;
                    info.QQ = QQ;
                    info.Nation = "";
                    info.Reward = "";
                    info.appreciation = "";
                    info.Zipcode = "";
                    info.Address = "";
                    info.Sex = Sex;
                    info.rank = 0;
                    info.hit = 0;
                    info.Isrecommend = "0";
                    info.Isshow = "0";
                    info.state = "0";
                    info.state1 = "0";
                    info.Telephone = TelePhone;
                    info.website = ""; //context.Request.Url.Scheme + "://" + context.Request.Url.Authority + System.Web.VirtualPathUtility.ToAbsolute("~/Master/MasterInfo.aspx?MasterId="+info.Id);
                    //加随机验证码
                    info.VCode = Guid.NewGuid().ToString("N");
                    //验证失效(1小时以内激活有效)
                    info.VTime = DateTime.Now.AddHours(1);

                    //验证用户名
                    if (!masterBLL.CheckExistUserName(info.Username))
                    {
                        ReturnJson = Tools.WriteJsonForReturn(false, "用户名重复");
                    }
                    if (masterBLL.AddNew(info) > 0)
                    {
                         //激活网址生成
                        string webpath = Request.Url.Scheme + "://" + Request.Url.Authority + System.Web.VirtualPathUtility.ToAbsolute("~/MasterZone/EmailChecking.aspx") + "?UserName=" + Server.UrlEncode(info.Username) + "&YZM=" + info.VCode;
                        string body = "尊敬的" + info.Username + "用户:请点击些链接激活:";
                        body += "<a href=" + webpath + ">" + webpath + "</a>";

                        RogerSMTP roger = new RogerSMTP(info.Email, "大师用户激活", body);
                        if (roger.Send())

                        //SMTP smtp = new SMTP(info.Email);
                        ////激活网址生成
                        //string webpath = Request.Url.Scheme + "://" + Request.Url.Authority + System.Web.VirtualPathUtility.ToAbsolute("~/MasterZone/EmailChecking.aspx") + "?UserName=" + Server.UrlEncode(info.Username) + "&YZM=" + info.VCode;
                        ////发送激活邮件
                        //if (smtp.Activation(webpath, info.Username))
                        {

                            ReturnJson = Tools.WriteJsonForReturn(true, Tools.GetEmail(info.Email));

                        }
                        else
                        {
                            ReturnJson = Tools.WriteJsonForReturn(false, "发送激活邮件失败!");
                        }
                    }
                    else
                    {
                        ReturnJson = Tools.WriteJsonForReturn(false, "注册失败!");
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.Error("错误!", ex);
                ReturnJson = Tools.WriteJsonForReturn(false, "系统出错!");
            }
            return ReturnJson;
        }

        /// <summary>
        /// 获取大师基本信息
        /// </summary>
        /// <param name="context"></param>
        public string GetMasterInfo()
        {
            //获取大师id
            string MasterId = Request["MasterId"];
            if (Tools.IsValidInput(ref MasterId, true))
            {
                return masterBLL.GetMasterInfoForJson(MasterId);
            }
            else
            {
                return Tools.WriteJsonForReturn(false, "MasterId为空!");
            }
        }

        /// <summary>
        /// 获取大师简介信息
        /// </summary>
        /// <param name="context"></param>
        public string GetMasterIntro()
        {
            //获取大师id
            string MasterId = Request["MasterId"];
            if (Tools.IsValidInput(ref MasterId, true))
            {
                return masterBLL.GetMasterIntroForJson(MasterId);
            }
            else
            {
                return Tools.WriteJsonForReturn(false, "MasterId为空!");
            }
        }

        /// <summary>
        /// 获取大师获奖信息
        /// </summary>
        /// <param name="context"></param>
        public string GetMasterReward()
        {
            //获取大师id
            string MasterId = Request["MasterId"];
            if (Tools.IsValidInput(ref MasterId, true))
            {
                return masterBLL.GetMasterRewardForJson(MasterId);
            }
            else
            {
                return Tools.WriteJsonForReturn(false, "MasterId为空!");
            }
        }

        /// <summary>
        /// 获取大师作品信息
        /// </summary>
        /// <param name="context"></param>
        public string GetMasterWork()
        {
            //获取大师id
            string MasterId = Request["MasterId"];
            if (Tools.IsValidInput(ref MasterId, true))
            {
                return new VProductCraftTypeBLL().GetMasterWorkForJson(MasterId);
            }
            else
            {
                return Tools.WriteJsonForReturn(false, "MasterId为空!");
            }
        }

    }
}
