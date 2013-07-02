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
        /// <summary>
        /// 根据username获取password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetPassword(string username)
        {
            try
            {
                return db.Members.First(s => s.UserName == username &&s.States=="1").Password;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetMemberId(string username)
        {
            try
            {
                return db.Members.First(s => s.UserName == username).Id.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        #region 根据用户名获取用户信息
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public Member GetMemberInfo(string UserName)
        {
            try
            {
                return db.Members.First(s => s.UserName == UserName);
            }
            catch
            {
                return null;
            }
        } 
        #endregion

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="info">会员model</param>
        /// <returns></returns>
        public bool MemberLogin(Member info)
        {
            return db.Members.Count(s => s.UserName == info.UserName && s.Password == info.Password && s.States == "1") > 0;
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public bool UpdatePassword(string UserName, string Password)
        {
            try
            {
                Member updateM = GetMemberInfo(UserName);
                updateM.Password = Password;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 根据用户名和随即验证码查找用户激活帐号的过期时间
        /// <summary>
        /// 根据用户名和随即验证码查找用户激活帐号的过期时间
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="VCode">激活码</param>
        /// <returns></returns>
        public DateTime GetMemberVTime(string UserName, string VCode)
        {
            return (DateTime)db.Members.First(s => s.UserName == UserName && s.VCode == VCode).VTime;
        }
        #endregion

        #region 激活帐号
        /// <summary>
        /// 激活帐号
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public bool ActivationMemberStatus(string UserName)
        {
            try
            {
                Member member = GetMemberInfo(UserName);
                member.States = "1";
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion
    }
}