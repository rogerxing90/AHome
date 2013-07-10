using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class Web_Sys_FunctionDAL:BaseDAL
    {
        #region 列出一级菜单
        /// <summary>
        /// 列出一级菜单
        /// </summary>
        /// <returns>执行状态</returns>
        public IEnumerable<Web_Sys_Function> ListAllTopMenu()
        {
            List<Web_Sys_Function> list = new List<Web_Sys_Function>();
            list = db.Web_Sys_Functions.Where(s => s.FATHER_ID == 0).ToList();
            return list;
        }
        #endregion
    }
}