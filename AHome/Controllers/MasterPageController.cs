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
            //CacheStorage.ICacheStorage Cache = CacheStorage.CacheFactory.CreateCacheFactory();
            string JsonData = new NewsTypeBLL().GetNewsTypeInfoForNewsByJson();
            return JsonData;
            //return View();
        }

        public string GetProductTypes()
        {
            string productJson = new CraftTypeBLL().GetCraftTypesByJson();
            return productJson;
 
        }

        public string CheckLoginStatus()
        {
            string UserName = Session["UserName"].ToString();

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
