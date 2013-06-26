using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AHome.DAL;
using AHome.Models;
using Newtonsoft.Json;

namespace AHome.BLL
{
    public class MemberBLL
    {
        /// <summary>
        /// 检验用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckExistUserName(string userName)
        {
            return new MemberDAL().GetSpecifiedUserNameCount(userName) == 0;
        }

        /// <summary>
        /// 增加member
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public bool AddNew(Member model)
        {
            return new MemberDAL().AddNew(model);
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
                jsonWriter.WritePropertyName("Email");
                jsonWriter.WriteValue(UserName);
                jsonWriter.WriteEndObject();
            }
            return json.ToString();
        }
        #endregion
    }
}