using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AHome.BLL;
using AHome.DAL;
using AHome.Models;
using Common;
using Newtonsoft.Json.Linq;

namespace AHome.Areas.Admin.Controllers
{
    public class ManageController : Controller
    {

        private Web_UserGroupBLL groupBLL = new Web_UserGroupBLL();
        private Web_UserBLL userBLL = new Web_UserBLL();

        private RogerContext dbParm = new RogerContext();


        //
        // GET: /Admin/Manage/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SystemLogInfo()
        {
            return View();
        }

        public ActionResult AddUser()
        {
            return View();
        }

        public void SaveEditGroupId2Cache()
        {
            string groupId = Request["id"];
            Session["EditGroupIdCache"] = groupId;
        }

        public ActionResult AddGroup()
        {
            ViewBag.ALLFunctions = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm);
            string groupId = Session["EditGroupIdCache"].ToString();
            if (string.IsNullOrEmpty(groupId))
            {
                ViewBag.EditFunctions = null;
                return View();
            }
            else
            {
                Web_UserGroup model = groupBLL.Get(Convert.ToInt32(groupId), dbParm);
                ViewBag.EditFunctions = model.Web_Sys_Functions;
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult AddGroup(Web_UserGroup model)
        {
            ViewBag.ALLFunctions = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm);
            try
            {
                string t = Request["FunctionId"].ToString();//获取选中的分类ID格式为("1，2，3”)不包含括号。
                List<Int32> idlist = new List<int>();
                if (!string.IsNullOrEmpty(t))
                {
                    if (t.Contains(","))
                    {
                        foreach (var each in t.Split(',').ToList())
                        {
                            idlist.Add(Convert.ToInt32(each));
                        }
                    }
                    else
                    {
                        idlist.Add(Convert.ToInt32(t));
                    }
                }
                IEnumerable<Web_Sys_Function> selectedFuncs = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm).Where(p => idlist.Contains(p.ID));
                if (model.Group_ID == 0) //New
                {
                    model.Web_Sys_Functions = selectedFuncs.ToList();
                    //model.Web_Sys_Functions = new List<Web_Sys_Function>();
                    //selectedFuncs.ToList().ForEach(f => model.Web_Sys_Functions.Add(f));
                    groupBLL.AddNew(model, dbParm);
                }
                else                    //Edit
                {
                    List<Web_Sys_Function> originalFuncs = groupBLL.Get(model.Group_ID, dbParm).Web_Sys_Functions;
                    model.Web_Sys_Functions = originalFuncs;
                    //要删除的function  
                    var dlist = originalFuncs.Where(p => !selectedFuncs.Contains(p)).ToList();
                    //要添加的function  
                    var alist = selectedFuncs.Where(p => !originalFuncs.Contains(p)).ToList();
                    foreach (var item in dlist)
                    {
                        model.Web_Sys_Functions.Remove(item);
                    }
                    foreach (var item in alist)
                    {
                        model.Web_Sys_Functions.Add(item);
                    }
                    groupBLL.Update(model, dbParm);
                }


                ViewBag.ALLFunctions = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm);
                return null;
                //return View();
            }
            catch (Exception ex)
            {
                var y = ex.InnerException;
                return View(model);
            }  
        }

        public ActionResult UserGroupInfo()
        {
            //ViewBag.ID = new SelectList(new Web_Sys_FunctionBLL().GetAllFunctions(), "ID", "NAME");
            //ViewBag.Functions = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm);
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
                    model.Web_Sys_Functions.Add(item);
                }
                new Web_UserGroupDAL().AddNew(model, dbParm);

                ViewBag.Functions = new Web_Sys_FunctionBLL().GetAllFunctions(dbParm);
                return View();
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
                groupBLL.Update(group, dbParm);
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
            //查询条件
            string key = Request["key"];
            //分页
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            //字段排序
            String sortField = Request["sortField"];
            String sortOrder = Request["sortOrder"];
            string realName = "";
            //对搜索内容进行验证
            if (!Common.Tools.IsValidInput(ref key, false))
            {
                return string.Empty;
            }
            else if (!string.IsNullOrEmpty(key))
                realName = key;//+= "REALNAME like '%" + key + "%'";
            //分页数据读取
            IEnumerable<Web_User> list = userBLL.ListByPagination(sortField, pageSize, pageIndex + 1, sortOrder == "asc" ? "1" : "0", realName);
            //获取总页数
            int totalPage = userBLL.GetCount(realName);
            //JSON 序列化
            string json = userBLL.MiniUiListToJson(list, totalPage, "");
            return json;
        }

        /// <summary>
        /// 更新用户权限信息
        /// </summary>
        /// <param name="context"></param>
        public void UpdateUserInfo()
        {
            Web_User user = new Web_User();
            string id = Request["id"];
            string state = Request["state"];
            if (Tools.IsValidInput(ref id, true) && Tools.IsValidInput(ref state, true))
            {
                user.ID = Convert.ToInt32(id);
                user.STATE = (state == "1" ? "0" : "1");//这里要反向操作
                userBLL.UpdateState(user);
            }
        }

        public void SaveUserInfo()
        {
            //用户json数据读取
            //数据为[{"LOGNAME":"tianzh","USERGROUPID":"1","PASSWORD":"tian815100","REALNAME":"tianzhuanghu","MEMO":"tianzhuanghu"}]
            String UserInfoStr = Request["UserInfo"];
            //System.IO.StreamWriter sw = new System.IO.StreamWriter(context.Server.MapPath("tzh.txt"));
            //sw.Write(UserInfoStr);
            //sw.Close();
            //使用Newtonsoft.Json.dll组件解析json对象
            //首先过滤掉json中的[和]
            string info = UserInfoStr.TrimStart('[');
            info = info.TrimEnd(']');
            JObject o = JObject.Parse(info);
            string LOGNAME = (string)o.SelectToken("LOGNAME");
            string USERGROUPID = (string)o.SelectToken("USERGROUPID");
            string PASSWORD = (string)o.SelectToken("PASSWORD");
            string REALNAME = (string)o.SelectToken("REALNAME");
            string MEMO = (string)o.SelectToken("MEMO");

            //保存用户数据的model对象
            Web_User user = new Web_User();
            //  //对象的赋值
            user.LOGNAME = LOGNAME;

            //Group需要特殊处理，从数据库中取出
            Web_UserGroup group = groupBLL.Get(Convert.ToInt32(USERGROUPID), dbParm);
            user.GROUP = group;
            user.PASSWORD = PASSWORD;
            user.REALNAME = REALNAME;
            user.MEMO = MEMO;
            user.REG_DATE = DateTime.Now;
            user.LAST_LOG_DATE = DateTime.Now;
            user.LOG_TIMES = 0;
            user.STATE = "0";
            if (o.SelectToken("id") != null)
            {
                user.ID = (int)o.SelectToken("id");
                userBLL.Update(user, dbParm);
            }
            else
            {
                userBLL.AddNew(user, dbParm);
            }
        }

        public string GetAllUserGroup()
        {
            //获取用户组信息
            IEnumerable<Web_UserGroup> list = groupBLL.ListAll();
            return Common.FormatToJson.ListToJsonNoName((IList<Web_UserGroup>)list);

        }

        public void RemoveUserInfo()
        {
            String idStr = Request["id"];
            if (String.IsNullOrEmpty(idStr))
                return;
            //  String[] ids = idStr.Split(',');
            //检验客户端字符串
            if (Common.Tools.IsValidInput(ref idStr, true))
            {
                userBLL.DeleteMoreID(idStr);
            }
        }

        /// <summary>
        /// 根据UserId获取User信息
        /// </summary>
        /// <param name="context"></param>
        public string GetUserInfo()
        {
            string strid = Request["id"];
            int id = 0;
            if (!string.IsNullOrEmpty(strid) && int.TryParse(strid, out id))
            {
                Web_User user = userBLL.Get(id);
                string json = userBLL.EditUserInfoToJson(user);
                return json;
            }
            return string.Empty;
        }


        #endregion

        #region [== SystemLogInfo ==]

        public string SearchSystemLog()
        {
            //查询条件
            string key = Request["key"];
            //分页
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            //字段排序
            String sortField = Request["sortField"];
            String sortOrder = Request["sortOrder"];
            string strCondition = "";
            //对搜索内容进行验证
            if (!Common.Tools.IsValidInput(ref key, false))
            {
                return string.Empty;
            }
            else
                strCondition = SystemLogBLL.ConfirmCondition(key);//判断查询条件
            SystemLogBLL bll = new SystemLogBLL();
            //分页数据读取
            IEnumerable<SystemLog> list = bll.ListByPagination(sortField, pageSize, pageIndex + 1, sortOrder == "asc" ? "0" : "1", strCondition);
            //获取总页数
            int totalPage = bll.GetCount(strCondition);
            //JSON 序列化
            string json = SystemLogBLL.MiniUiListToJson(list, totalPage, "");


            return json;
        }

        public void RemoveSystemLog()
        {
            //清空三天前的日志
            new SystemLogBLL().ClearSystemLog();
        }



        #endregion
    }
}
