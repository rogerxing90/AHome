using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class Web_UserDAL : BaseDAL
    {
        #region 登录验证,返回用户组信息
        /// <summary>
        /// 登录验证,返回用户组Id信息
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>返回用户组信息</returns>
        public int IsLogin(Web_User user)
        {
            try
            {
                return db.Web_Users.Include("GROUP").First(s => s.LOGNAME == user.LOGNAME && s.PASSWORD == user.PASSWORD && s.STATE == "1").GROUP.Group_ID;
            }
            catch
            {
                return 0;
            }
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
        public IEnumerable<Web_User> ListByPagination(string tableName, string InnerJoin, string strGetFields, string sortId, int PageSize, int PageIndex, string OrderType, string realName)
        {
            List<Web_User> list = new List<Web_User>();
            if (string.IsNullOrEmpty(realName))
            {
                list = db.Web_Users.ToList();
            }
            else
            {
                list = db.Web_Users.Where(s => s.REALNAME.Contains(realName)).ToList();
            }

            return list;
        }
        /// <summary>
        ///获取表总记录个数(不用加where)
        /// <param name="strWhere">查询条件(不用加where)</param>
        /// <summary>
        public int GetCount(string realName)
        {
            if (string.IsNullOrEmpty(realName))
            {
                return db.Web_Users.Count();
            }
            else
            {
                return db.Web_Users.Count(s => s.REALNAME.Contains(realName));
            }
        }

        public bool AddNew(Web_User model, RogerContext dbParm = null)
        {
            try
            {
                if (dbParm == null)
                {
                    dbParm = db;
                }
                dbParm.Web_Users.Add(model);
                dbParm.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                Web_User delUser = db.Web_Users.First(s => s.ID == id);
                db.Web_Users.Remove(delUser);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool DeleteMoreID(string strID)
        {
            try
            {
                int state = 0;
                string sql = ("delete from Web_User where ID in (" + strID + ") and STATE=@state");
                DbParameter[] paras = new DbParameter[] { new SqlParameter("state", state) };
                //注此处parameter没用，但为空时报错
                db.Database.ExecuteSqlCommand(sql, paras);

                //法二
                //String[] ids = strID.Split(',');
                //foreach (string id in ids)
                //{
                //    Web_User disUser = db.Web_Users.First(s => s.ID == Convert.ToInt32(id));
                //    db.Web_Users.Remove(disUser);
                //    db.SaveChanges();
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }


        public int Update(Web_User model, RogerContext dbParm = null)
        {
            return base.Update<Web_User>(model, model.ID, dbParm);
            //if (dbParm == null)
            //{
            //    dbParm = db;
            //}
            //dbParm.Entry(model).State = System.Data.EntityState.Modified;
            //return dbParm.SaveChanges();
        }

        public Web_User Get(int id)
        {
            return db.Web_Users.First(s => s.ID == id);
        }

        #region 更新状态信息
        /// <summary>
        /// 更新状态信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateState(Web_User user)
        {
            try
            {
                db.Web_Users.First(s => s.ID == user.ID).STATE = user.STATE;
                db.SaveChanges();
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