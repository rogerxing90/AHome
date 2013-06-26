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

    }
}
