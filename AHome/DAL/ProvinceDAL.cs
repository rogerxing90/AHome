using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class ProvinceDAL :BaseDAL
    {
        #region 根据父级id获取下级
        /// <summary>
        /// 根据父级id获取下级
        /// </summary>
        /// <param name="id">父级id</param>
        /// <returns></returns>
        public List<Province> GetArea(string id)
        {
            Int32 parentId = Convert.ToInt32(id);
            return db.Provinces.Where(s => s.ParentId == parentId).ToList();
        }
        #endregion
    }
}