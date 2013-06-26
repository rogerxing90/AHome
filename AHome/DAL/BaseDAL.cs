using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public abstract class BaseDAL
    {
        public RogerContext db = new RogerContext();
    }
}