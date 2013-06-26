using System;
using System.Collections.Generic;
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
    }
}