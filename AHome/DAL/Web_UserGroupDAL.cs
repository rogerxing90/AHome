using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class Web_UserGroupDAL:BaseDAL
    {
        #region 根据用户组ID查找该用户组所有功能(视图中已做作权限判断)
        /// <summary>
        /// 根据用户组ID查找该用户组所有功能(视图中已做作权限判断)
        /// </summary>
        /// <param name="groupID">用户组信息id</param>
        /// <returns></returns>
        public List<Web_Sys_Function> GetUserGroupFunction(int groupID)
        {
            return db.Web_UserGroups.First(s => s.ID == groupID).Web_Sys_Functions;
        }
        #endregion
    }
}