using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AHome.BLL;
using AHome.DAL;
using AHome.Models;
using Newtonsoft.Json.Linq;

namespace AHome.Areas.Admin.Controllers
{
    public class ManageController : Controller
    {
        
        private Web_UserGroupBLL groupBLL = new Web_UserGroupBLL();

        private RogerContext dbParm = new RogerContext();

        //
        // GET: /Admin/Manage/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserGroupInfo()
        {
            //ViewBag.ID = new SelectList(new Web_Sys_FunctionBLL().GetAllFunctions(), "ID", "NAME");
            ViewBag.Functions = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm);
            return View();
        }

        [HttpPost]
        public ActionResult UserGroupInfo(Web_UserGroup model)
        {

            try
            {
                var t = Request["FunctionId"];//获取选中的分类ID格式为("1，2，3”)不包含括号。
                List<Int32> idlist = new List<int>();
                foreach (var each in t.Split(',').ToList())
                {
                    idlist.Add(Convert.ToInt32(each));
                }
                var cate = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm).Where(p => idlist.Contains(p.ID));
                if (model.Web_Sys_Functions == null)
                {
                    model.Web_Sys_Functions = new List<Web_Sys_Function>();
                }
                foreach (var item in cate)
                {
                    model.Web_Sys_Functions.Add(item);//给新闻添加分类  
                }
                new Web_UserGroupDAL().AddNew(model,dbParm);
                //groupBLL.AddNew(model);

                ViewBag.Functions = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm);
                return View();

                //using (var db = new dbContent())
                //{
                //    if (t != null)
                //    {
                //        List<Int32> idlist = new List<int>();//t;//将传来的分类ID转成List<int>  
                //        var cate = db.Categories.Where(p => idlist.Contains(p.Id));
                //        foreach (var item in cate)
                //        {
                //            model.Categories.Add(item);//给新闻添加分类  
                //        }
                //    }
                //    db.News.AddObject(model);
                //    db.SaveChanges();
                //    return View();
                //}
            }
            catch (Exception ex)
            {
                var y = ex.InnerException;
                return View(model);
            }  
        }

        public ActionResult UserInfo()
        {
            return View();
        }

        #region [==UserGroupInfoPage CRUD==]
        public string SearchUserGroup()
        {
            //分页
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            //字段排序
            String sortField = Request["sortField"];
            String sortOrder = Request["sortOrder"];
            string strCondition = "";
            //分页数据读取
            IEnumerable<Web_UserGroup> list = groupBLL.ListByPagination(sortField, pageSize, pageIndex + 1, sortOrder == "asc" ? "1" : "0", strCondition);
            //获取总页数
            int totalPage = groupBLL.GetCount(strCondition);
            //JSON 序列化
            string json = Common.FormatToJson.MiniUiListToJson<Web_UserGroup>((IList<Web_UserGroup>)list, totalPage, "");
            return json;
        }

        /// <summary>
        /// 保存资料业务
        /// </summary>
        /// <param name="context"></param>
        public void SaveUserGroup()
        {
            //用户json数据读取
            //数据为[{"LOGNAME":"tianzh","USERGROUPID":"1","PASSWORD":"tian815100","REALNAME":"tianzhuanghu","MEMO":"tianzhuanghu"}]
            String UserGroupStr = Request["UserGroup"];
            if (string.IsNullOrEmpty(UserGroupStr))
                return;

            ////使用Newtonsoft.Json.dll组件解析json对象
            ////首先过滤掉json中的[和]
            string info = UserGroupStr.TrimStart('[');
            info = info.TrimEnd(']');
            JObject o = JObject.Parse(info);
            string USERGROUP = (string)o.SelectToken("USERGROUP");
            string DESCRIPTION = (string)o.SelectToken("DESCRIPTION");

            Web_UserGroup group = new Web_UserGroup();


            //保存用户数据的model对象
            group.USERGROUP = USERGROUP;
            group.DESCRIPTION = DESCRIPTION;
            if (o.SelectToken("ID") != null)
            {
                group.Group_ID = (int)o.SelectToken("ID");
                groupBLL.Update(group);
            }
            else
            {
                groupBLL.AddNew(group);
            }
        }

        public string GetUserGroup()
        {
            string strid = Request["id"];
            int id = 0;
            if (!string.IsNullOrEmpty(strid) && int.TryParse(strid, out id))
            {
                //获取用户组信息
                Web_UserGroup group = groupBLL.Get(id);
                return Common.FormatToJson.ScriptSerializationToJson(group);
            }
            return string.Empty;
        }

        public void RemoveUserGroup()
        {
            String idStr = Request["id"];
            //检验客户端字符串
            if (Common.Tools.IsValidInput(ref idStr, true))
            {
                groupBLL.DeleteMoreID(idStr);
            }
        }


        #endregion

        #region [== UserInfoPage CRUD ==]

        public string SearchUserInfo()
        {

            return "";
        }

        public void UpdateUserInfo()
        {
 
        }

        #endregion
    }
}
