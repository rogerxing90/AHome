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
            string userName = Request["UserName"];

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
                    info.VCode = Guid.NewGuid().ToString("N");
                    //验证失效(1小时以内激活有效)
                    info.VTime = DateTime.Now.AddHours(1);

                    //验证用户名
                    if (!bll.CheckExistUserName(info.UserName))
                    {
                        return errorString;
                    }
                    if (bll.AddNew(info))
                    {
                        string activationUrl = Request.Url.Scheme + "://" + Request.Url.Authority + System.Web.VirtualPathUtility.ToAbsolute("~/Member/EmailChecking") + "?UserName=" + Server.UrlEncode(info.UserName) + "&YZM=" + info.VCode;
                        string body = "尊敬的" + info.UserName + "用户:请点击些链接激活:";
                        body += "<a href=" + activationUrl + ">" + activationUrl + "</a>";

                        SMTP smtp = new SMTP(info.Email);
                    if (smtp.sendemail("用户激活",body))

                        //RogerSMTP roger = new RogerSMTP(info.Email, "用户激活", body);
                        //if (roger.Send())
                    {
                            return bll.WriteJsonForReturn(true, Tools.GetEmail(info.Email));
                        }
                        else
                        {
                            return errorString;
                        }
                        //SMTP smtp = new SMTP(info.Email);
                        ////激活网址生成
                        //string webpath = Request.Url.Scheme + "://" + Request.Url.Authority + System.Web.VirtualPathUtility.ToAbsolute("~/Member/EmailChecking.aspx") + "?UserName=" + Server.UrlEncode(info.UserName) + "&YZM=" + info.VCode;
                        ////发送激活邮件
                        //if (smtp.Activation(webpath, info.UserName))
                        //{
                        //    return bll.WriteJsonForReturn(true, Tools.GetEmail(info.Email));

                        //}
                        //else
                        //{
                        //    return errorString;
                        //}
                    }
                    else
                    {
                        return errorString;
                    }
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

        /// <summary>
        /// 安全退出
        /// </summary>
        /// <param name="context"></param>
        public void MemberLogout()
        {
            string UserName = (string)Session["UserName"];
            if (!Tools.IsNullOrEmpty(UserName))
            {
                //如果session存在,清除session
                Session.Remove("UserName");
                Session.Remove("UserId");
            }
            //清除cookies
            CookieHelper.ClearCookie("UserName");
            CookieHelper.ClearCookie("Pwd");
            //页面跳转
            JScript.AlertAndRedirect("安全退出成功!欢迎下次前来访问!", "/Home/Index");
        }


        public string ForgetPwd()
        {
            string returnJsonString =string.Empty;
            string UserName = Request["UserName"];
            //string CheckCode = Request["CheckCode"];
            string Email = Request["Email"];
            //验证码校验
            //if (!CheckCode.Equals(Session["checkcode"].ToString()))
            //{
            //    returnJsonString = bll.WriteJsonForReturn(false, "验证码不正确!");
            //}
            //字符串sql注入检测
            if (Tools.IsValidInput(ref UserName, true) && Tools.IsValidInput(ref Email, true))
            {
                //获取用户邮箱

                //邮箱和用户名状态(正确与否)
                bool status = bll.CheckUserNameAndEmail(UserName, Email);
                //随机生成一个6位的新密码
                string NewPwd = bll.CreateNewPwd();
                if (!string.IsNullOrEmpty(Email) && status && bll.UpdatePwd(UserName, NewPwd))
                {
                    SMTP smtp = new SMTP(Email);
                    if (smtp.sendemail("潮州工艺品平台", "尊敬的" + UserName + "用户:恭喜您,您在" + DateTime.Now.ToString() + "使用找回密码功能重置密码,您的密码:" + NewPwd + ",请尽快修改密码并妥善保管!"))
                    {
                        returnJsonString = bll.WriteJsonForReturn(true, Tools.GetEmail(Email));
                    }
                    else
                    {
                        returnJsonString = bll.WriteJsonForReturn(false, "邮箱发送失败!");
                    }
                }
                else
                {
                    returnJsonString = bll.WriteJsonForReturn(false, "用户名或邮箱不正确!");
                }


            }
            else
            {
                returnJsonString = bll.WriteJsonForReturn(false, "输入非法内容!");
            }

            return returnJsonString;
        }

        /// <summary>
        /// 激活帐号
        /// </summary>
        /// <returns></returns>
        public string ActivationMemberNumber()
        {
            //注意要对url解码
            string UserName = Server.UrlDecode(Request["UserName"]);

            string VCode = Request["VCode"];
            //激活
            if (Tools.IsValidInput(ref UserName, true) && Tools.IsValidInput(ref VCode, true))
            {
                return bll.WriteJsonForReturn(bll.ActivationMemberNumber(UserName, VCode), ""); ;
            }
            else
            {
                return bll.WriteJsonForReturn(false, "");
            }
        }
    }
}
