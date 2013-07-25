using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AHome.DAL;
using AHome.Models;
using Common;

namespace AHome.BLL
{
    public class SystemLogBLL
    {
        private SystemLogDAL dal = new SystemLogDAL();

        /// <summary>
        /// 专门生成为MiniUi生成json数据(List->json)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">实现了Ilist接口的list</param>
        /// <param name="total">记录总数</param>
        /// <param name="paramMaxMin">这里放排序的参数例如,string para=""maxAge":37,"avgAge":27,"minAge":24"</param>
        /// <returns>返回json数据</returns>
        public static string MiniUiListToJson(IEnumerable<SystemLog> SystemLogInfo, int total, string paramMaxMinAvg)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"total\":" + total + ",\"data\":");
            Json.Append("[");
            foreach (SystemLog Info in SystemLogInfo)
            {
                Json.Append("{");
                Json.Append("\"Id\":\"" + Info.Id + "\"");
                Json.Append(",");
                Json.Append("\"Title\":\"" + Info.Title + "\"");
                Json.Append(",");
                Json.Append("\"AddTime\":\"" + Info.AddTime.Value.GetDateTimeFormats('s')[0].ToString() + "\"");
                Json.Append(",");
                Json.Append("\"Url\":\"" + Info.Url + "\"");
                Json.Append(",");
                Json.Append("\"UserName\":\"" + Info.UserName + "\"");
                Json.Append("}");
                if (Info != SystemLogInfo.Last())
                {
                    Json.Append(",");
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }


        #region 保存系统日志
        /// <summary>
        /// 保存系统日志
        /// </summary>
        /// <param name="Title">日志信息</param>
        /// <returns></returns>
        public bool SaveSystemLog(string Title)
        {
            SystemLog log = new SystemLog();
            log.AddTime = DateTime.Now;
            log.Title = Title;
            //获取远程ip地址
            log.Url = HttpContext.Current.Request.Url.Authority;
            Web_User user = (Web_User)HttpContext.Current.Session["User"];
            log.UserName = user.LOGNAME;
            return dal.AddNew(log);
        }
        #endregion

        #region 判定查询条件
        /// <summary>
        /// 判定查询条件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string ConfirmCondition(string info)
        {
            string condition = "";//查询条件
            if (Tools.IsNumber(info)) //如果是数字,则查询id
            {
                condition = "Id like '%" + info + "%'";
            }
            else condition = "UserName like '%" + info + "%'"; //查询用户名
            return condition;
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
        public IEnumerable<SystemLog> ListByPagination(string sortId, int PageSize, int PageIndex, string OrderType, string strWhere)
        {
            return dal.ListByPagination("SystemLog", "", "*", sortId, PageSize, PageIndex, OrderType, strWhere);
        }

        /// <summary>
        ///分页获取数据
        /// </summary>
        public int GetCount(string strWhere)
        {
            return new SystemLogDAL().GetCount(strWhere);
        }

        #region 清空日志(强制保留三天内的日志)
        /// <summary>
        /// 清空日志(强制保留三天内的日志)
        /// </summary>
        /// <returns></returns>
        public bool ClearSystemLog()
        {
            //清空三天前的日志
            return new SystemLogDAL().ClearSystemLog();
        }
        #endregion
    }
}