using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class SystemLogDAL:BaseDAL
    {

        /// <summary>
        /// 增加SystemLog
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public bool AddNew(SystemLog model)
        {
            try
            {
                db.SystemLogs.Add(model);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///分页获取数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="InnerJoin">内连接</param>
        /// <param name="strGetFields">返回的列信息</param>
        /// <param name="sortId">排序的列名</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="OrderType">排序类型排序类型, 非0 值则降序</param>
        /// <param name="strWhere">查询条件(注意: 不要加where) </param>
        public IEnumerable<SystemLog> ListByPagination(string tableName, string InnerJoin, string strGetFields, string sortId, int PageSize, int PageIndex, string OrderType, string strWhere)
        {
            List<SystemLog> list = new List<SystemLog>();
            //list = db.SystemLogs.ToList();
            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " where " + strWhere;
            }
            string sql = ("select * from SystemLogs" + strWhere);
            list = db.SystemLogs.SqlQuery(sql).ToList();
            return list;
        }

        /// <summary>
        ///获取表总记录个数(不用加where)
        /// <param name="strWhere">查询条件(不用加where)</param>
        /// <summary>
        public int GetCount(string strWhere)
        {
            //if (!string.IsNullOrEmpty(strWhere))
            //    strWhere = " where " + strWhere;
            //return SqlHelper.ExecuteSelectFirstNum("select count(1) from SystemLog" + strWhere);
            return db.SystemLogs.Count();
        }

        #region 清空日志内容(不用加where)
        /// <summary>
        ///  清空日志内容(不用加where)
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public bool ClearSystemLog()
        {
            try
            {

                //DateTime date = DateTime.Now.AddDays(-3);
                //db.SystemLogs.Where(s => s.AddTime < date).ToList().ForEach(each=>db.SystemLogs.Remove(each));
                
                //表不可以用parameter@
                //DbParameter[] paras = new DbParameter[] { new SqlParameter("table", "SystemLogs") };
                string sql = ("delete from SystemLogs where AddTime < dateadd(DAY,-3,GETDATE())");
                db.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}