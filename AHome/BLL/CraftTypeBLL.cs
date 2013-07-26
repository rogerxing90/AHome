using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AHome.DAL;
using AHome.Models;
using Common;

namespace AHome.BLL
{
    public class CraftTypeBLL
    {
        private CraftTypeDAL dal = new CraftTypeDAL();

        public string GetCraftTypesByJson()
        {
            List<CraftType> craftTypeList = new CraftTypeDAL().ListAll();
            return JsonHelper.JsonSerializer<List<CraftType>>(craftTypeList);
        }

        #region 工艺品类别树转化为json格式
        /// <summary>
        /// 工艺品类别树转化为json格式
        /// </summary>
        /// <returns></returns>
        public string craftTypeTreeToJson()
        {
            //传递的json格式

            IEnumerable<CraftType> craftTypeList = dal.ListAll();
            StringBuilder sb = new StringBuilder("[");

            foreach (CraftType root in craftTypeList)
            {
                if (root.Belongsid == -1)
                {
                    sb.Append("{id:\"" + root.ID + "\",text:\"" + root.Name + "\"");
                    sb.Append(",pid:\"-1\"");//添加父节点
                    sb.Append(",expanded:\"false\"");
                    if (root.IsLeaf == "0")//如果是不是叶子节点,那么,就要递归添加children:[{xxx},内容
                    {
                        sb.Append(",children:");
                        GetLeafTree(ref sb, (int)root.ID, craftTypeList);//递归追加叶子
                    }
                    sb.Append("},");
                }

            }
            sb.Remove(sb.Length - 1, 1);   //去除掉最后一个多余的,
            sb.Append("]");
            return FormatToJson.MiniUiToJsonForTree(sb.ToString(), "工艺品类别");

        }
        #endregion

        #region 递归获得父级ID下的所有类别json数据
        /// <summary>
        /// 递归获得父级ID下的所有类别json数据
        /// </summary>
        /// <param name="sb">json字符串</param>
        /// <param name="parentID">父级id</param>
        /// <param name="craftTypeList">类别信息集合</param>
        public void GetLeafTree(ref StringBuilder sb, int parentID, IEnumerable<CraftType> craftTypeList)
        {
            sb.Append("[");
            foreach (CraftType leaf in craftTypeList)
            {
                if (leaf.Belongsid == parentID) //根据双亲节点查找叶子
                {
                    sb.Append("{id:\"" + leaf.ID + "\",text:\"" + leaf.Name + "\"");
                    sb.Append(",pid:\"" + parentID + "\"");//添加父节点
                    sb.Append(",expanded:\"false\"");
                    if (leaf.IsLeaf == "0")//如果是不是叶子节点,那么,就要递归添加children:[{xxx},内容
                    {
                        sb.Append(",children:");
                        GetLeafTree(ref sb, (int)leaf.ID, craftTypeList);//递归追加叶子

                    }
                    sb.Append("},");
                }
            }
            sb.Remove(sb.Length - 1, 1);   //去除掉最后一个多余的,
            sb.Append("]");
        }
        #endregion

        #region 增加crafttype
        /// <summary>
        /// 增加crafttype
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public bool AddNewAndUpdateLeaf(CraftType model)
        {
            model.FId = GetFId((int)model.Belongsid);
            return dal.AddNewAndUpdateLeaf(model);
        }
        #endregion

        #region 获取自己的Fid
        /// <summary>
        /// 获取自己的Fid
        /// </summary>
        /// <param name="Belongsid"></param>
        /// <returns></returns>
        public string GetFId(int Belongsid)
        {
            string MaxBortherFId = dal.GetFatherMaxChildFId(Belongsid);//获取兄弟最大Fid的属性(格式F0101)
            //前缀F01
            string FirstStr = MaxBortherFId.Substring(0, MaxBortherFId.Length - 2);
            //后缀01
            string LastStr = MaxBortherFId.Substring(MaxBortherFId.Length - 2, 2);
            int LastNum = Convert.ToInt32(LastStr) + 1;
            //拼接自己的FId
            string FId = FirstStr + string.Format("{0:D2}", LastNum);//拼接字符串F0102
            return FId;
        }
        #endregion

        /// <summary>
        /// 更新crafttype实体
        /// </summary>
        /// <param name="model">tableName实体</param>
        /// <returns>执行状态</returns>
        public bool Update(CraftType model)
        {
            return dal.Update(model);
        }

        public bool DeleteMoreID(string strID)
        {
            return dal.DeleteMoreID(strID);
        }
    }
}