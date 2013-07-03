using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace AHome.BLL
{
    public static class BLLUtil
    {
        #region 为用户登录写入json数据
        /// <summary>
        /// 为用户登录写入json数据
        /// </summary>
        /// <param name="Status">登录状态</param>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public static string WriteJsonForReturn(bool Status, string UserName)
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

    }
}