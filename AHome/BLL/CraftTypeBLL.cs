using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.DAL;
using AHome.Models;
using Common;

namespace AHome.BLL
{
    public class CraftTypeBLL
    {
        public string GetCraftTypesByJson()
        {
            List<CraftType> craftTypeList = new CraftTypeDAL().ListAll();
            return JsonHelper.JsonSerializer<List<CraftType>>(craftTypeList);

        }
    }
}