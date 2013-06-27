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
    public class MemberController : Controller
    {
        private MemberBLL bll = new MemberBLL();
        //
        // GET: /Member/

        public ActionResult RegisterMember()
        {
            return View();
        }

        public ActionResult MemberLogin()
        {
            return View();
        }

        public ActionResult MemberInfo()
        {
            return View();
        }

        //Ajax
        public string CheckExistUserName()
        {
            string username = Request["username"];
            if (Tools.IsValidInput(ref username, true))
            {
                return bll.CheckExistUserName(username) ? "True" : "False";
            }
            return string.Empty;
        }

        /// <summary>
        /// 保存用户信息 
        /// </summary>
        /// <param name="context"></param>
        public string SaveMemberInfo()
        {
            string errorString = bll.WriteJsonForReturn(false, "");
            try
            {
                //表单读取
                string txtUserName = Request["txtUserName"];
                string txtPwd = Request["txtPwd"];
                string txtEmail = Request["txtEmail"];
                string txtCheckCode = Request["txtCheckCode"];
                //验证码校验
                //if (!txtCheckCode.Equals(Session["checkcode"].ToString()))
                //{
                //    Response.Write(bll.WriteJsonForReturn(false, ""));
                //}
                //字符串sql注入检测
                if (Tools.IsValidInput(ref txtUserName, true) && Tools.IsValidInput(ref txtPwd, true) && Tools.IsValidInput(ref txtEmail, true))
                {
                    Member info = new Member();
                    info.UserName = txtUserName;
                    info.Password = Tools.GetMD5(txtPwd);
                    info.Email = txtEmail;
                    info.States = "0";

                    //加随机验证码
                    //info.VCode = Guid.NewGuid().ToString("N");
                    //验证失效(1小时以内激活有效)
                    //info.VTime = DateTime.Now.AddHours(1);

                    //验证用户名
                    if (!bll.CheckExistUserName(info.UserName))
                    {
                        return errorString;
                    }
                    if (bll.AddNew(info))
                    {
                        return bll.WriteJsonForReturn(true, info.Email);
                    }
                    else
                    {
                        return errorString;
                    }
                    //if (bll.AddNew(info) > 0)
                    //{
                    //    SMTP smtp = new SMTP(info.Email);
                    //    //激活网址生成
                    //    string webpath = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + System.Web.VirtualPathUtility.ToAbsolute("~/Member/EmailChecking.aspx") + "?UserName=" + context.Server.UrlEncode(info.username) + "&YZM=" + info.VCode;
                    //    //发送激活邮件
                    //    if (smtp.Activation(webpath, info.username))
                    //    {

                    //        context.Response.Write(bll.WriteJsonForReturn(true, Tools.GetEmail(info.Email)));

                    //    }
                    //    else
                    //    {
                    //        context.Response.Write(bll.WriteJsonForReturn(false, ""));
                    //    }
                    //}
                    //else
                    //{
                    //    context.Response.Write(bll.WriteJsonForReturn(false, ""));
                    //}
                }
                else
                {
                    return errorString;
                }
            }
            catch (Exception ex)
            {
                //logger.Error("错误!", ex);
                return errorString;
            }
        }

        /// <summary>
        /// 检查用户登录状态
        /// </summary>
        public string CheckLoginStatus()
        {
            string UserName = (string)Session["UserName"];
            bool Status = false;
            string jsonString;
            if (!Tools.IsNullOrEmpty(UserName))
            {
                //如果session存在,直接返回用户状态
                jsonString = bll.WriteJsonForReturn(true, UserName);
            }
            else
            {
                //用户自动登录状态检测
                jsonString = bll.CheckLoginStatus(out Status);
                //登录成功!
                if (Status)
                {
                    Session["UserName"] = CookieHelper.GetCookieValue("UserName");
                }


            }
            //如果登录成功,则把用户ID放在Session中
            if (Status == true && Tools.IsNullOrEmpty(Session["UserId"]))
            {
                Session["UserId"] = bll.GetMemberId(UserName);
            }

            return jsonString;

        }

        //避免与上面MemberLogin重名出错
        public string MemberAjaxLogin()
        {
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
                string ReturnJson = "";
                //sql注入检测
                if (Tools.IsValidInput(ref Name, true) && (Tools.IsValidInput(ref Pwd, true)) && (Tools.IsValidInput(ref IsSaveName, true)) && (Tools.IsValidInput(ref IsSavePwd, true)))
                {
                    Member info = new Member();
                    MemberBLL bll = new MemberBLL();
                    info.UserName = Name;
                    info.Password = Pwd;
                    ReturnJson = bll.ReturnJson(info, out Status);
                    if (Status) //如果成功登陆
                    {
                        //记住帐号和密码
                        bll.RememberUserInfo(info, bll.GetRememberType(IsSaveName, IsSavePwd));

                        //保存登录状态
                        Session["UserName"] = info.UserName;
                        //如果登录成功,则把用户ID放在Session中
                        if (Tools.IsNullOrEmpty(Session["UserId"]))
                        {
                            Session["UserId"] = bll.GetMemberId(info.UserName);
                        }
                    }
                }

                return ReturnJson;
            }
            catch (Exception ex)
            {
                //logger.Error("会员登录出错!", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取地区(省市联动)
        /// </summary>
        /// <param name="context"></param>
        public string GetArea()
        {
            //省市联动(根据父级id获取子级)
            string Pid = Request["CodeId"];
            if (Tools.IsValidInput(ref Pid, true))
            {
                return new ProvinceBLL().GetAreaByJson(Pid);
            }
            else
            {
                return "";
            }
        }

        public string GetUserInfo()
        {
            string UserName = (string)Session["UserName"];
            if (!Tools.IsNullOrEmpty(UserName))
            {
                //如果session存在,直接返回用户状态
                return bll.GetMemberInfoByJson(UserName);
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
