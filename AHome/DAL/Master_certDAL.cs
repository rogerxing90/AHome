using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class Master_certDAL:BaseDAL
    {
        #region 通过MasterId获取所有荣誉证书
        /// <summary>
        /// 通过MasterId获取所有荣誉证书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Master_cert> ListAllById(int MasterId)
        {
            return db.MasterCerts.Where(s => s.Master.Id == MasterId).ToList();
        }
        #endregion
    }
}