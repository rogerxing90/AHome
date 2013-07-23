using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.DAL;
using AHome.Models;

namespace AHome.BLL
{
    public class Web_UserGroupBLL
    {
        private Web_UserGroupDAL dal = new Web_UserGroupDAL();

        /// <summary>
        ///分页获取数据
        /// </summary>
        /// <param name="sortId">排序的列名</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="OrderType">排序类型排序类型, 非0 值则降序</param>
        /// <param name="strWhere">查询条件(注意: 不要加where) </param>
        public IEnumerable<Web_UserGroup> ListByPagination(string sortId, int PageSize, int PageIndex, string OrderType, string strWhere)
        {
            return dal.ListByPagination("WEB_USERGROUP", "", "*", sortId, PageSize, PageIndex, OrderType, strWhere);
        }

        public IEnumerable<Web_UserGroup> ListAll()
        {
            return dal.ListAll();
        }


        /// <summary>
        ///分页获取数据
        /// </summary>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// 增加WEB_USERGROUP
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public int AddNew(Web_UserGroup model, RogerContext dbParam = null)
        {
            return dal.AddNew(model,dbParam);
        }

        /// <summary>
        /// 更新WEB_USERGROUP实体
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public int Update(Web_UserGroup model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 根据id获取WEB_USERGROUP实体信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>执行状态</returns>
        public Web_UserGroup Get(int id, RogerContext dbParm=null)
        {
            return dal.Get(id,dbParm);
        }

        /// <summary>
        /// 删除WEB_USERGROUP
        /// </summary>
        /// <param name="strID">strID,记得多个用,隔开</param>
        /// <returns>执行状态</returns>
        public int DeleteMoreID(string strID)
        {
            return dal.DeleteMoreID(strID);
        }
    }
}