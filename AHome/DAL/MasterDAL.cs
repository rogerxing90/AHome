using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class MasterDAL : BaseDAL
    {

        /// <summary>
        /// 获取指定userName的个数
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetSpecifiedUserNameCount(string username)
        {
            return db.Masters.Count(s => s.Username == username);
        }

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="info">会员model</param>
        /// <returns></returns>
        public bool MasterLogin(Master info)
        {
            return db.Masters.Count(s => s.Username == info.Username && s.Password == info.Password && s.state == "1") > 0;
        }
        #endregion

        #region 根据用户名获取用户信息
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public Master GetMasterInfo(string UserName)
        {
            return db.Masters.First(s => s.Username == UserName);
        }
        #endregion

        #region 获取用户密码
        /// <summary>
        /// 获取用户密码
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public string GetPassword(string UserName)
        {
            try
            {
                return db.Masters.First(s => s.Username == UserName && s.state == "1").Password;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
		/// 增加master
		/// </summary>
		/// <param name="model">tableName实体</param>
		/// <returns>执行状态</returns>
        public int AddNew(Master model)
        {
            db.Masters.Add(model);
            return db.SaveChanges();
        }
    }
}