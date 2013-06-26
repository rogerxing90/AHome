using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AHome.Models;

namespace AHome.DAL
{
    public class MemberDAL : BaseDAL
    {
        /// <summary>
        /// 获取指定userName的个数
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetSpecifiedUserNameCount(string username)
        {
            return db.Members.Count(s => s.UserName == username);
        }

        public bool AddNew(Member member) 
        {
            try
            {
                db.Members.Add(member);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}