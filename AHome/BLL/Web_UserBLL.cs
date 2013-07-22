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
    public class Web_UserBLL
    {
        private Web_UserDAL dal = new Web_UserDAL();

        #region 登录验证,返回用户组信息
        /// <summary>
        /// 登录验证,返回用户组信息
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>返回用户组信息</returns>
        public int IsLogin(Web_User user)
        {
            int num = 0;
            num = dal.IsLogin(user);
            return num;
        }
        #endregion

        #region 获取后台首页菜单数据(格式为json)
        /// <summary>
        /// 获取后台首页菜单数据(格式为json)
        /// </summary>
        /// <param name="user">user的model实体</param>
        /// <returns></returns>
        public string GetMenuByJson(Web_User user)
        {
            //这里要获取功能表中,一级菜单
            IEnumerable<Web_Sys_Function> functions = new Web_Sys_FunctionDAL().ListAllTopMenu();
            List<Web_Sys_Function> userGroupFunctions = new Web_UserGroupDAL().GetUserGroupFunction(user.GROUP.Group_ID);
            StringBuilder Json = new StringBuilder();
            StringWriter sw = new StringWriter(Json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartArray();
                foreach (Web_Sys_Function funtion in functions)
                {
                    jsonWriter.WriteStartObject();
                    jsonWriter.WritePropertyName("id");
                    jsonWriter.WriteValue(funtion.ID);
                    jsonWriter.WritePropertyName("text");
                    jsonWriter.WriteValue(funtion.NAME);
                    jsonWriter.WritePropertyName("expanded");
                    jsonWriter.WriteValue("false");
                    jsonWriter.WritePropertyName("children");//接下来是一个集合
                    jsonWriter.WriteStartArray();
                    foreach (Web_Sys_Function func in userGroupFunctions)
                    {
                        if ((int)func.FATHER_ID == funtion.ID)
                        {

                            jsonWriter.WriteStartObject();
                            jsonWriter.WritePropertyName("id");
                            jsonWriter.WriteValue(func.FATHER_ID.ToString());
                            jsonWriter.WritePropertyName("url");
                            jsonWriter.WriteValue(func.URL);
                            jsonWriter.WritePropertyName("text");
                            jsonWriter.WriteValue(func.NAME);
                            jsonWriter.WritePropertyName("expanded");
                            jsonWriter.WriteValue("false");
                            jsonWriter.WriteEndObject();
                        }
                    }
                    jsonWriter.WriteEndArray();
                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndArray();
            }
            return Json.ToString();

        }
        #endregion
    }
}