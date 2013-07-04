using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class VProductCraftTypeDAL:BaseDAL
    {
        #region 通过大师id查找大师的产品(每种分别显示前8个)
        /// <summary>
        /// 通过大师id查找大师的产品(每种分别显示前8个)
        /// </summary>
        /// <param name="MasterId">大师id</param>
        /// <returns></returns>
        public List<VProductCraftType> ListAllByMasterIdToDatable(string MasterId)
        {
            Int32 masterId = Convert.ToInt32(MasterId);
            return db.VProductCraftTypes.Where(s => s.Master.Id == masterId).ToList();
            //return SqlHelper.ExecuteDataTable("select * from(select *,row_number() over (partition by TypeId order by TypeId) total from VProductCraftType where Masterid=@Masterid and Belongstype='0') rn where rn.total<=8", (DbParameter)new SqlParameter("Masterid", MasterId));
        }
        #endregion
    }
}