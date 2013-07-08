using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AHome.BLL;

namespace AHome.Controllers
{
    public class MasterPageController : Controller
    {
        public string GetNewsTypes()
        {
            //创建缓存工厂
            CacheStorage.ICacheStorage Cache = CacheStorage.CacheFactory.CreateCacheFactory();
            string cache = Convert.ToString(Cache.Get("GetNewsType"));
            string JsonData = "";//返回的json数据
            if (string.IsNullOrEmpty(cache))//如果缓存为空
            {

                //读取数据并转化为json格式
                JsonData = new NewsTypeBLL().GetNewsTypeInfoForNewsByJson();
                //插入缓存(时间从配置文件中读取)
                Cache.Insert("GetNewsType", JsonData, CacheManage.GetTimeConfig("NewsType"));

            }
            else
            {
                JsonData = cache;
            }
            return JsonData;
        }

        public string GetProductTypes()
        {
            string productJson = new CraftTypeBLL().GetCraftTypesByJson();
            return productJson;
 
        }

        public string CheckLoginStatus()
        {
            //string UserName = Session["UserName"].ToString();

            return string.Empty;
        }

        //
        // GET: /MasterPage/

        public ActionResult Index()
        {
            return View();
        }

    }
}
