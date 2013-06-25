using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AHome.DAL;
using AHome.Models;
using Common;
using System.IO;

namespace AHome.BLL
{
    public class NewsTypeBLL
    {
        /// <summary>
        /// 为新闻模块下拉框生成类别选择的数据
        /// </summary>
        /// <returns></returns>
        public string GetNewsTypeInfoForNewsByJson()
        {
            List<NewsType> NewsTypeList = new NewsTypeDAL().ListAll();
            return JsonHelper.JsonSerializer<List<NewsType>>(NewsTypeList);

            //IEnumerable<NewsType> NewsTypeList = new NewsTypeDAL().ListAll();
            //StringBuilder sb = new StringBuilder("[");
            //foreach (NewsType type in NewsTypeList)
            //{
            //    sb.Append("{\"TypeId\":\"" + type.Id + "\",\"TypeName\":\"" + type.Name + "\"}");
            //    if (type != NewsTypeList.Last())
            //    {
            //        sb.Append(",");

            //    }
            //}
            //sb.Append("]");
            //return sb.ToString();
        } 
    }
}