using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class VMasterTypeDAL :BaseDAL
    {
        #region 根据id获取所有信息
        /// <summary>
        /// 根据id获取所有信息
        /// </summary>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public List<VMasterType> ListAllById(string id)
        {
            Int32 masterId = Convert.ToInt32(id);
            return db.VMasterTypes.Where(s => s.Master.Id == masterId).ToList();
        }
        #endregion
    }
}