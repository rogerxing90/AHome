using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.DAL;
using AHome.Models;

namespace AHome.BLL
{
    public class Web_Sys_FunctionBLL
    {
        private Web_Sys_FunctionDAL dal = new Web_Sys_FunctionDAL();

        public IEnumerable<Web_Sys_Function> GetAllFunctions(RogerContext dbParm)
        {
            return dal.GetAllFunctions(dbParm);
        }

    }
}