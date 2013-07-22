using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class Web_UserGroupDAL : BaseDAL
    {
        #region 根据用户组ID查找该用户组所有功能(视图中已做作权限判断)
        /// <summary>
        /// 根据用户组ID查找该用户组所有功能(视图中已做作权限判断)
        /// </summary>
        /// <param name="groupID">用户组信息id</param>
        /// <returns></returns>
        public List<Web_Sys_Function> GetUserGroupFunction(int groupID)
        {
            return db.Web_UserGroups.First(s => s.Group_ID == groupID).Web_Sys_Functions;
        }
        #endregion

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
        public IEnumerable<Web_UserGroup> ListByPagination(string tableName, string InnerJoin, string strGetFields, string sortId, int PageSize, int PageIndex, string OrderType, string strWhere)
        {
            IEnumerable<Web_UserGroup> list = new List<Web_UserGroup>();
            DbParameter[] paras = new DbParameter[]
            {
                (DbParameter)new SqlParameter("tableName", tableName), 
                (DbParameter)new SqlParameter("@InnerJoin", InnerJoin), 
                (DbParameter)new SqlParameter("@strGetFields", strGetFields), 
                (DbParameter)new SqlParameter("@sortId", sortId), 
                (DbParameter)new SqlParameter("@PageSize", PageSize), 
                (DbParameter)new SqlParameter("@PageIndex", PageIndex), 
                (DbParameter)new SqlParameter("@doCount", "0"), 
                (DbParameter)new SqlParameter("@OrderType", OrderType), 
                (DbParameter)new SqlParameter("@strWhere", strWhere),
            };

            list =  db.Database.SqlQuery <Web_UserGroup>("exec[pagination]  @tableName,@InnerJoin,@strGetFields,@sortId,@PageSize,@PageIndex,@doCount,@OrderType,@strWhere", paras);

            //此处分页存储过程pagination未找到

            return db.Web_UserGroups.ToList();
        }


        /// <summary>
        ///获取表总记录个数(不用加where)
        /// <param name="strWhere">查询条件(不用加where)</param>
        /// <summary>
        public int GetCount(string strWhere)
        {
            //if (!string.IsNullOrEmpty(strWhere))
            //    strWhere = " where " + strWhere;
            //return SqlHelper.ExecuteSelectFirstNum("select count(1) from WEB_USERGROUP" + strWhere);
            return db.Web_UserGroups.Count();
        }

        /// <summary>
        /// 增加WEB_USERGROUP
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public int AddNew(Web_UserGroup model,RogerContext dbParam=null)
        {
            if (dbParam == null)
            {
                dbParam = db;
            }
            dbParam.Web_UserGroups.Add(model);
            return dbParam.SaveChanges();
        }

        /// <summary>
        /// 更新WEB_USERGROUP实体
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public int Update(Web_UserGroup model)
        {
            Web_UserGroup oldGroup = GetGroupInfoById(model.Group_ID);
            oldGroup = model;
            return db.SaveChanges();
        }

        public Web_UserGroup GetGroupInfoById(int id)
        {
            return db.Web_UserGroups.First(s => s.Group_ID == id);
        }

        public Web_UserGroup Get(int id)
        {
            return db.Web_UserGroups.First(s => s.Group_ID == id);
        }

        /// <summary>
        /// 删除WEB_USERGROUP
        /// </summary>
        /// <param name="strID">strID,记得多个用,隔开</param>
        /// <returns>执行状态</returns>
        public int DeleteMoreID(string strID)
        {

            bool status = false;
            string[] IDs = strID.Split(',');
            foreach (string each in IDs)
            {
                int id = Convert.ToInt32(each);
                db.Web_UserGroups.Remove(Get(id));
            }
            return db.SaveChanges();

        }
    }
}