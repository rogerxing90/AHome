using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class NewsTypeDAL
    {
        private RogerContext db = new RogerContext();
        /// <summary>
        /// 列出tableName所有的实体信息
        /// </summary>
        /// <returns>执行状态</returns>
        public List<NewsType> ListAll()
        {
            List<NewsType> list = new List<NewsType>();
            list = db.NewsTypes.ToList();
            return list;
        }
    }
}