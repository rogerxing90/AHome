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

        #region 专门生成为MiniUi生成json数据(List->json)
        /// <summary>
        /// 专门生成为MiniUi生成json数据(List->json)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">实现了Ilist接口的list</param>
        /// <param name="total">记录总数</param>
        /// <param name="paramMaxMin">这里放排序的参数例如,string para="\"maxAge\":37,\"avgAge\":27,\"minAge\":24"</param>
        /// <returns></returns>
        public string MiniUiListToJson(IEnumerable<Web_User> webUser, int total, string paramMaxMinAvg)
        {

            StringBuilder Json = new StringBuilder();
            StringWriter sw = new StringWriter(Json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {

                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("total");
                jsonWriter.WriteValue(total);
                jsonWriter.WritePropertyName("data");
                jsonWriter.WriteStartArray();
                foreach (Web_User user in webUser)
                {
                    jsonWriter.WriteStartObject();
                    jsonWriter.WritePropertyName("LOGNAME");
                    jsonWriter.WriteValue(user.LOGNAME);
                    //jsonWriter.WritePropertyName("PASSWORD");
                    //jsonWriter.WriteValue(user.PASSWORD);
                    jsonWriter.WritePropertyName("USERGROUP");
                    jsonWriter.WriteValue(user.GROUP.USERGROUP);
                    jsonWriter.WritePropertyName("REALNAME");
                    jsonWriter.WriteValue(user.REALNAME);
                    jsonWriter.WritePropertyName("STATE");
                    jsonWriter.WriteValue(user.STATE);
                    if (user.REG_DATE != null)
                    {
                        jsonWriter.WritePropertyName("REG_DATE");
                        jsonWriter.WriteValue(user.REG_DATE.Value.GetDateTimeFormats('s')[0].ToString());
                    }
                    if (user.LAST_LOG_DATE != null)
                    {
                        jsonWriter.WritePropertyName("LAST_LOG_DATE");
                        jsonWriter.WriteValue(user.LAST_LOG_DATE.Value.GetDateTimeFormats('s')[0].ToString());
                    }
                    jsonWriter.WritePropertyName("LOG_TIMES");
                    jsonWriter.WriteValue(user.LOG_TIMES.ToString());
                    jsonWriter.WritePropertyName("MEMO");
                    jsonWriter.WriteValue(user.MEMO);
                    jsonWriter.WritePropertyName("id");
                    jsonWriter.WriteValue(user.ID);

                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndArray();
                jsonWriter.WriteEndObject();

            }
            return Json.ToString();

        }
        #endregion

        #region 更新状态信息
        /// <summary>
        /// 返回编辑用户信息的的json格式
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string EditUserInfoToJson(Web_User user)
        {

            StringBuilder Json = new StringBuilder();
            StringWriter sw = new StringWriter(Json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {

                jsonWriter.Formatting = Formatting.Indented;

                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("LOGNAME");
                jsonWriter.WriteValue(user.LOGNAME);
                jsonWriter.WritePropertyName("REALNAME");
                jsonWriter.WriteValue(user.REALNAME);
                jsonWriter.WritePropertyName("USERGROUPID");
                jsonWriter.WriteValue(user.GROUP.Group_ID);
                //jsonWriter.WritePropertyName("USERGROUP");
                //jsonWriter.WriteValue(user.GROUP.USERGROUP);
                jsonWriter.WritePropertyName("STATE");
                jsonWriter.WriteValue(user.STATE);
                jsonWriter.WritePropertyName("MEMO");
                jsonWriter.WriteValue(user.MEMO);
                jsonWriter.WritePropertyName("id");
                jsonWriter.WriteValue(user.ID);
                jsonWriter.WriteEndObject();


            }
            return Json.ToString();

        }

        #endregion

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

        /// <summary>
        ///分页获取数据
        /// </summary>
        /// <param name="sortId">排序的列名</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="OrderType">排序类型排序类型, 非0 值则降序</param>
        /// <param name="strWhere">查询条件(注意: 不要加where) </param>
        public IEnumerable<Web_User> ListByPagination(string sortId, int PageSize, int PageIndex, string OrderType, string realName)
        {
            return dal.ListByPagination("WEB_USER", "", "*", sortId, PageSize, PageIndex, OrderType, realName);
        }

        /// <summary>
        ///分页获取数据
        /// </summary>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// 增加WEB_USER
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public bool AddNew(Web_User model, RogerContext dbParm)
        {
            return dal.AddNew(model, dbParm);
        }
        /// <summary>
        /// 删除WEB_USER
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>执行状态</returns>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }
        /// <summary>
        /// 删除WEB_USER
        /// </summary>
        /// <param name="strID">strID,记得多个用,隔开</param>
        /// <returns>执行状态</returns>
        public bool DeleteMoreID(string strID)
        {
            return dal.DeleteMoreID(strID);
        }
        /// <summary>
        /// 更新WEB_USER实体
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public bool Update(Web_User model, RogerContext dbParm)
        {
            return dal.Update(model, dbParm);
        }

        public Web_User Get(int id)
        {
            return dal.Get(id);
        }

        #region 更新状态信息
        /// <summary>
        /// 更新状态信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateState(Web_User user)
        {
            return dal.UpdateState(user);
        }
        #endregion

    }
}