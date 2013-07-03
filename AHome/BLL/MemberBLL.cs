using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AHome.DAL;
using AHome.Models;
using Common;
using Newtonsoft.Json;

namespace AHome.BLL
{
    public class MemberBLL
    {
        private MemberDAL mdal = new MemberDAL();
        /// <summary>
        /// 检验用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckExistUserName(string userName)
        {
            return mdal.GetSpecifiedUserNameCount(userName) == 0;
        }

        /// <summary>
        /// 增加member
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public bool AddNew(Member model)
        {
            return mdal.AddNew(model);
        }

        #region 为用户登录写入json数据
        /// <summary>
        /// 为用户登录写入json数据
        /// </summary>
        /// <param name="Status">登录状态</param>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public string WriteJsonForReturn(bool Status, string UserName)
        {
            StringBuilder json = new StringBuilder();
            StringWriter sw = new StringWriter(json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Status");
                jsonWriter.WriteValue(Status);
                jsonWriter.WritePropertyName("UserName");
                jsonWriter.WriteValue(UserName);
                jsonWriter.WriteEndObject();
            }
            return json.ToString();
        }
        #endregion

        #region 检查用户登录状态,用于验证自动登录
        /// <summary>
        /// 检查用户登录状态,用于验证自动登录(并返回json格式)
        /// </summary>
        /// <returns></returns>
        public string CheckLoginStatus(out bool Status)
        {

            //登录状态
            Status = true;
            string UserName = CookieHelper.GetCookieValue("UserName");
            //如果cookies为空,直接返回
            if (Tools.IsNullOrEmpty(UserName))
            {
                Status = false;
            }
            string Pwd = CookieHelper.GetCookieValue("Pwd");
            if (Tools.IsNullOrEmpty(Pwd))
            {
                Status = false;
            }
            else
            {
                //查找该用户真实密码,并进行md5加密
                string password = mdal.GetPassword(UserName);
                //如果两次密码相同则可以自动登陆了
                if (password != Pwd)
                {
                    Status = false;
                }
            }

            //生成json格式数据
            return WriteJsonForReturn(Status, UserName);
        }
        #endregion

        public string GetMemberId(string username)
        {
            return mdal.GetMemberId(username);
        }

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="info">会员model</param>
        /// <returns></returns>
        public bool MemberLogin(Member info)
        {
            info.Password = Tools.GetMD5(info.Password);
            return mdal.MemberLogin(info);
        }
        #endregion

        #region 返回给客户端的json格式数据
        /// <summary>
        /// 返回给客户端的json格式数据(用于根据用户登录状态决定)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string ReturnJson(Member info, out bool Status)
        {
            //登录状态
            Status = MemberLogin(info);
            //生成json格式数据
            return WriteJsonForReturn(Status, info.UserName);

        }
        #endregion

        #region 记住用户信息
        /// <summary>
        /// 记住用户信息
        /// </summary>
        /// <param name="Type">记住用户信息类别</param>
        /// <returns></returns>
        public bool RememberUserInfo(Member info, RememberType type)
        {
            if (type == RememberType.RememberName)
            {
                //记住帐号7天
                CookieHelper.SetCookie("UserName", info.UserName, DateTime.Now.AddDays(7));
            }
            else if (type == RememberType.RememberNameAndPwd)
            {

                //md5哈希加密
                string sercret = Tools.GetMD5(info.Password);
                //同时记住帐号和密码7天
                CookieHelper.SetCookie("UserName", info.UserName, DateTime.Now.AddDays(7));
                CookieHelper.SetCookie("Pwd", sercret, DateTime.Now.AddDays(7));
            }
            else
            {
                return false;
            }
            return true;

        }

        #endregion

        #region 根据保存帐号密码状态判断是保存帐号还是同时保存帐号和密码
        /// <summary>
        /// 根据保存帐号密码状态判断是保存帐号还是同时保存帐号和密码
        /// </summary>
        /// <param name="IsSaveName">"1"代表保存,"0"代表不保存</param>
        /// <param name="IsSavePwd">"1"代表保存,"0"代表不保存</param>
        /// <returns></returns>
        public RememberType GetRememberType(string IsSaveName, string IsSavePwd)
        {
            RememberType SaveType = RememberType.NoRemember;

            //保存帐号和密码
            if (IsSaveName.Equals("1") && IsSavePwd.Equals("1"))
            {
                SaveType = RememberType.RememberNameAndPwd;
            }
            //保存帐号
            if (IsSaveName.Equals("1") && !IsSavePwd.Equals("1"))
            {
                SaveType = RememberType.RememberName;
            }
            else if (!IsSaveName.Equals("1"))
            {
                SaveType = RememberType.NoRemember;
            }
            return SaveType;
        }
        #endregion

        #region 地址分割
        /// <summary>
        /// 地址分割(将用户信息表中的地址分割成 省(编号) 市(编号) 县(编号) 家住址
        /// </summary>
        /// <param name="Address">地址</param>
        /// <returns></returns>
        public string[] GetSplitAddress(string Address)
        {
            string[] str = Address.Split('|');
            return str;
        }
        #endregion

        #region 根据用户名获取用户信息(返回json数据)
        /// <summary>
        /// 根据用户名获取用户信息(返回json数据)
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public string GetMemberInfoByJson(string UserName)
        {
            bool Status = false;
            Member info = mdal.GetMemberInfo(UserName);
            if (info != null)
            {
                Status = true;
            }
            StringBuilder json = new StringBuilder();
            StringWriter sw = new StringWriter(json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Status");
                jsonWriter.WriteValue(Status);
                jsonWriter.WritePropertyName("Data");
                jsonWriter.WriteStartArray();
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("UserName");
                jsonWriter.WriteValue(info.UserName);
                jsonWriter.WritePropertyName("Sex");
                jsonWriter.WriteValue(info.Sex);
                jsonWriter.WritePropertyName("Nation");
                jsonWriter.WriteValue(info.nation);
                //mobilephone Telephone Email qq Zipcode Address
                jsonWriter.WritePropertyName("MobilePhone");
                jsonWriter.WriteValue(info.mobilephone);
                jsonWriter.WritePropertyName("TelePhone");
                jsonWriter.WriteValue(info.Telephone);
                jsonWriter.WritePropertyName("Email");
                jsonWriter.WriteValue(info.Email);
                jsonWriter.WritePropertyName("QQ");
                jsonWriter.WriteValue(info.qq);
                jsonWriter.WritePropertyName("ZipCode");
                jsonWriter.WriteValue(info.Zipcode);
                //地址处理

                string[] strAddresss = GetSplitAddress(info.Address);
                string Province = "";
                string City = "";
                string Country = "";
                string Address = "";
                if (strAddresss.Count() > 4)
                {
                    Province = strAddresss[0];
                    City = strAddresss[1];
                    Country = strAddresss[2];
                    Address = strAddresss[3];

                }
                jsonWriter.WritePropertyName("Province");
                jsonWriter.WriteValue(Province);
                jsonWriter.WritePropertyName("City");
                jsonWriter.WriteValue(City);
                jsonWriter.WritePropertyName("Country");
                jsonWriter.WriteValue(Country);
                jsonWriter.WritePropertyName("Address");
                jsonWriter.WriteValue(Address);
                jsonWriter.WriteEndObject();

                jsonWriter.WriteEndArray();
                jsonWriter.WriteEndObject();
            }
            return json.ToString();
        }
        #endregion

        #region 检查用户名和邮箱
        /// <summary>
        /// 检查用户名和邮箱
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Email">邮箱</param>
        /// <returns></returns>
        public bool CheckUserNameAndEmail(string UserName, string Email)
        {
            Member info = mdal.GetMemberInfo(UserName);
            return Email == info.Email;
        }
        #endregion

        #region 随机生成一个6位的密码
        /// <summary>
        /// 随机生成一个6位的密码
        /// </summary>
        /// <returns></returns>
        public string CreateNewPwd()
        {
            string Pwd = "";
            Random ran = new Random(DateTime.Now.Second);
            for (int i = 0; i < 6; i++)
            {
                Pwd += ran.Next(1, 10);
            }
            return Pwd;
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Pwd">密码</param>
        /// <returns></returns>
        public bool UpdatePwd(string UserName, string Pwd)
        {
            return mdal.UpdatePassword(UserName, Tools.GetMD5(Pwd));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        public bool UpdatePassword(string UserName, string oldPwd, string newPwd)
        {
            string Pwd = mdal.GetPassword(UserName);
            if (Pwd == Tools.GetMD5(oldPwd))
            {
                //加密并且更新
                return mdal.UpdatePassword(UserName, Tools.GetMD5(newPwd));
            }
            return false;
        }
        #endregion

        #region 验证用户信息
        /// <summary>
        /// 验证用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="GuidInfo">guid随机码</param>
        public bool ActivationMemberNumber(string UserName, string GuidInfo)
        {
            //获取过期时间
            DateTime dt = mdal.GetMemberVTime(UserName, GuidInfo);
            //如果已经过期
            if (dt < DateTime.Now)
            {
                return false;
            }
            else
            {
                //激活帐号
                return mdal.ActivationMemberStatus(UserName);
            }
        }
        #endregion
    }

    #region 记住帐号和密码的枚举
    /// <summary>
    /// 记住帐号和密码的枚举
    /// </summary>
    public enum RememberType
    {
        /// <summary>
        /// 记住帐号
        /// </summary>
        RememberName = 0,
        /// <summary>
        /// 同时记住帐号和密码
        /// </summary>
        RememberNameAndPwd = 1,
        /// <summary>
        /// 不记住帐号密码
        /// </summary>
        NoRemember = 2
    }
    #endregion
}