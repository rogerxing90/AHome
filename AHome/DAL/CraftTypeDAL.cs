using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class CraftTypeDAL : BaseDAL
    {
        public List<CraftType> ListAll()
        {
            List<CraftType> cragtTypes = new List<CraftType>();
            cragtTypes = db.CraftTypes.ToList();
            return cragtTypes;
        }

        #region 增加crafttype
        /// <summary>
        /// 增加crafttype
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public bool AddNewAndUpdateLeaf(CraftType model)
        {
            try
            {
                db.CraftTypes.Add(model);
                db.SaveChanges();
                //更新父节点为folder样式，即IsLeaf = "0"
                if (model.Belongsid != -1)//第一层在数据库中没有父节点，不需要更新
                {
                    CraftType parentNode = db.CraftTypes.First(s => s.ID == model.Belongsid);
                    if (parentNode != null && !parentNode.IsLeaf.Equals("0"))
                    {
                        parentNode.IsLeaf = "0";
                        base.Update<CraftType>(parentNode, parentNode.ID);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 查询父级功能id信息
        /// <summary>
        /// 查询父级功能id信息
        /// </summary>
        /// <returns></returns>
        public string GetFatherMaxChildFId(int belongsid)
        {

            string sql = "select ISNULL(max(FId),(select Fid+'00' from CraftTypes where ID=@belongsid)) as MaxChild from CraftTypes  where belongsid=@belongsid";  //查询出最大的子元素的FId如果没有,就拼接父级Fid+'00'
            DbParameter[] paras = new DbParameter[] { new SqlParameter("belongsid", belongsid) };
            return db.ExecuteScalar(sql, paras).ToString();
        }
        #endregion

        public bool Update(CraftType model)
        {
            try
            {
                base.Update<CraftType>(model, model.ID);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public CraftType Get(int id)
        {
            return db.CraftTypes.First(s => s.ID == id);
        }

        public bool DeleteMoreID(string strID)
        {
            try
            {
                string[] IDs = strID.Split(',');
                foreach (string each in IDs)
                {
                    CraftType type = Get(Convert.ToInt32(each));
                    DeleteAllSubNodes(type);//删除父节点时，遍历所有节点，从最底层没有子节点的开始删
                }
                db.SaveChanges();


                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteAllSubNodes(CraftType node)
        {
            List<CraftType> deleteNodes = GetAllSubNodes(node.ID);
            foreach (CraftType each in deleteNodes)
            {
                DeleteAllSubNodes(each);
            }
            db.CraftTypes.Remove(node);
        }

        public List<CraftType> GetAllSubNodes(int id)
        {
            return ListAll().Where(s => s.Belongsid == id).ToList();
        }
    }
}