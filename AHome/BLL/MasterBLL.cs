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
    public class MasterBLL
    {
        private MasterDAL masterDAL = new MasterDAL();

        //登录信息相关
        #region 检验用户名是否存在
        /// <summary>
        /// 检验用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckExistUserName(string userName)
        {
            return masterDAL.GetSpecifiedUserNameCount(userName) == 0;
        }
        #endregion

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="info">model</param>
        /// <returns></returns>
        public bool MasterLogin(Master info)
        {
            info.Password = Tools.GetMD5(info.Password);
            return masterDAL.MasterLogin(info);
        }
        #endregion

        #region 返回给客户端的json格式数据(用于根据用户登录状态决定)
        /// <summary>
        /// 返回给客户端的json格式数据(用于根据用户登录状态决定)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string ReturnJson(Master info, out bool Status)
        {
            //登录状态
            Status = MasterLogin(info);
            //生成json格式数据
            return BLLUtil.WriteJsonForReturn(Status, info.Username);
        }
        #endregion

        #region 记住用户信息
        /// <summary>
        /// 记住用户信息
        /// </summary>
        /// <param name="Type">记住用户信息类别</param>
        /// <returns></returns>
        public bool RememberUserInfo(Master info, RememberType type)
        {
            if (type == RememberType.RememberName)
            {
                //记住帐号7天
                CookieHelper.SetCookie("MasterName", info.Username, DateTime.Now.AddDays(7));
            }
            else if (type == RememberType.RememberNameAndPwd)
            {

                //md5哈希加密
                string sercret = Tools.GetMD5(info.Password);
                //同时记住帐号和密码7天
                CookieHelper.SetCookie("MasterName", info.Username, DateTime.Now.AddDays(7));
                CookieHelper.SetCookie("MasterPwd", sercret, DateTime.Now.AddDays(7));
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

        #region 获取用户id
        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public string GetMasterId(string UserName)
        {
            Master info = masterDAL.GetMasterInfo(UserName);
            return info.Id.ToString();
        }
        #endregion

        #region 检查用户登录状态,用于验证自动登录(并返回json格式)
        /// <summary>
        /// 检查用户登录状态,用于验证自动登录(并返回json格式)
        /// </summary>
        /// <returns></returns>
        public string CheckLoginStatus(out bool Status)
        {

            //登录状态
            Status = true;
            string UserName = Common.CookieHelper.GetCookieValue("MasterName");
            //如果cookies为空,直接返回
            if (Tools.IsNullOrEmpty(UserName))
            {
                Status = false;
            }
            string Pwd = Common.CookieHelper.GetCookieValue("MasterPwd");
            if (Tools.IsNullOrEmpty(Pwd))
            {
                Status = false;
            }
            else
            {
                //查找该用户真实密码,并进行md5加密
                string password = masterDAL.GetPassword(UserName);
                //如果两次密码相同则可以自动登陆了
                if (password != Pwd)
                {
                    Status = false;
                }
            }

            //生成json格式数据
            return BLLUtil.WriteJsonForReturn(Status, UserName);
        }
        #endregion

        /// <summary>
        /// 增加master
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public int AddNew(Master model)
        {
            return masterDAL.AddNew(model);
        }

        #region 获取大师基本信息
        /// <summary>
        /// 获取大师基本信息
        /// </summary>
        /// <param name="id">大师id</param>
        /// <returns></returns>
        public string GetMasterInfoForJson(string id)
        {
            //加载状态
            bool Status = false;
            //获取大师类别信息
            List<VMasterType> masterTypeList = new VMasterTypeDAL().ListAllById(id);
            //拼接大师类别信息
            StringBuilder sbTypeName = new StringBuilder();
            foreach (var masterType in masterTypeList)
            {
                sbTypeName.Append(masterType.TypeName);

                if (masterType != masterTypeList.Last())
                {
                    sbTypeName.Append(",");
                }
            }

            int MasterId = Convert.ToInt32(id);
            //获取大师基本信息
            Master Info = masterDAL.Get(MasterId);

            //转化为json格式
            StringBuilder json = new StringBuilder();
            StringWriter sw = new StringWriter(json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                //判断数据读取状态
                if (Info != null)
                {
                    Status = true;
                }
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Status");
                jsonWriter.WriteValue(Status);
                jsonWriter.WritePropertyName("Data");

                jsonWriter.WriteStartArray();
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Name");
                jsonWriter.WriteValue(Info.Name);
                jsonWriter.WritePropertyName("Sex");
                jsonWriter.WriteValue(Info.Sex == "1" ? "男" : "女");
                jsonWriter.WritePropertyName("Birthday");
                jsonWriter.WriteValue(Info.BirthDay.ToShortDateString());
                jsonWriter.WritePropertyName("PicturePath");
                jsonWriter.WriteValue(Info.Picturepath);
                jsonWriter.WritePropertyName("TypeName");
                jsonWriter.WriteValue(sbTypeName.ToString());
                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndArray();

                jsonWriter.WriteEndObject();

            }
            return json.ToString();
        }
        #endregion

        #region 获取大师简介信息
        /// <summary>
        /// 获取大师简介信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetMasterIntroForJson(string id)
        {
            //加载状态
            bool Status = false;
            int MasterId = Convert.ToInt32(id);
            //获取大师基本信息
            Master Info = masterDAL.Get(MasterId);

            //转化为json格式
            StringBuilder json = new StringBuilder();
            StringWriter sw = new StringWriter(json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                //判断数据读取状态
                if (Info != null)
                {
                    Status = true;
                }
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Status");
                jsonWriter.WriteValue(Status);
                jsonWriter.WritePropertyName("Data");

                jsonWriter.WriteStartArray();
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Introduction");
                jsonWriter.WriteValue(Info.Introduction);
                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndArray();

                jsonWriter.WriteEndObject();

            }
            return json.ToString();
        }
        #endregion

        #region 获取大师获奖信息
        /// <summary>
        /// 获取大师获奖信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetMasterRewardForJson(string id)
        {
            //加载状态
            bool Status = false;
            //获取大师荣誉证书
            int MasterId = Convert.ToInt32(id);
            IEnumerable<Master_cert> masterCertList = new Master_certDAL().ListAllById(MasterId);
            //获取大师基本信息
            Master Info = masterDAL.Get(MasterId);

            //转化为json格式
            StringBuilder json = new StringBuilder();
            StringWriter sw = new StringWriter(json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                //判断数据读取状态
                if (Info != null)
                {
                    Status = true;
                }
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Status");
                jsonWriter.WriteValue(Status);
                jsonWriter.WritePropertyName("Data");

                jsonWriter.WriteStartArray();
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Reward");
                jsonWriter.WriteValue(Info.Reward);

                jsonWriter.WritePropertyName("CertPicList");
                jsonWriter.WriteStartArray();
                foreach (Master_cert cert in masterCertList)
                {
                    jsonWriter.WriteStartObject();
                    jsonWriter.WritePropertyName("CertName");
                    jsonWriter.WriteValue(cert.Name);
                    jsonWriter.WritePropertyName("CertPic");
                    jsonWriter.WriteValue(cert.Picpath);
                    jsonWriter.WriteEndObject();

                }
                jsonWriter.WriteEndArray();
                jsonWriter.WriteEndObject();


                jsonWriter.WriteEndObject();

            }
            return json.ToString();
        }
        #endregion

        #region 验证用户信息
        /// <summary>
        /// 验证用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="GuidInfo">guid随机码</param>
        public bool ActivationMasterNumber(string UserName, string GuidInfo)
        {
            //获取过期时间
            DateTime dt = masterDAL.GetMasterVTime(UserName, GuidInfo);
            //如果已经过期
            if (dt < DateTime.Now)
            {
                return false;
            }
            else
            {
                //激活帐号
                return masterDAL.ActivationMasterStatus(UserName);
            }
        }
        #endregion


    }
}