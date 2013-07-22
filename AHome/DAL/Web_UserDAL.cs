using System;
using System.Collections.Generic;
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
    }
}