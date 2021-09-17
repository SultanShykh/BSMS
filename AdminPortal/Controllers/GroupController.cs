using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPortal.Models;
using AdminPortal.Codebase;

namespace AdminPortal.Controllers
{
    [AuthorizeUser]
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetGroups(DTParameters param)
        {
            try
            {
                DTResult<GroupMasterModel> result;
                GroupProcessing.Get(param, out result);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }


        [HttpGet]
        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetgroupPermissions()
        {
            var list = new List<AuthorizeModel>();
            CommonProcessing.GetGroupWithPermissionsTree(out list);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckGroupName(string groupName)
        {
            bool result;
            GroupProcessing.checkGroupName(groupName, out result);
            return Json(result);
        }

        [HttpPost]
        public ActionResult CreateGroup(FormCollection model)
        {
            try
            {
                var users = model["Users"] == null ? null : model["Users"].ToString();
                bool result = true;

                GroupMasterModel Model = new GroupMasterModel();
                GroupMenuModel groupMenu = new GroupMenuModel();

                if (!ModelState.IsValid)
                    return Json(new { status = false, message = "Fields are empty"});

                int count = 0;

                Model.Name = model["Name"].ToString();
                string MenuName = "";
                foreach (var v in model)
                {
                    if (MenuName != "" && !v.ToString().Contains(MenuName) && count > 0 && count < 5)
                    {
                        GroupProcessing.CreateGroup(Model, groupMenu, out result);
                        groupMenu.AllowCreate = false;
                        groupMenu.AllowView = false;
                        groupMenu.AllowEdit = false;
                        groupMenu.AllowDelete = false;
                        count = 0;
                    }
                    if (v.ToString().Contains("Menus"))
                    {
                        MenuName = new string(v.ToString().SkipWhile(c => c != '-')
                           .Skip(1)
                           .TakeWhile(c => c != '[')
                           .ToArray()).Trim();
                        string menuid = new string(v.ToString().SkipWhile(c => c != '[')
                           .Skip(1)
                           .TakeWhile(c => c != ']')
                           .ToArray()).Trim();
                        groupMenu.MenuId = Convert.ToInt32(menuid); count++;
                    }
                    else if (v.ToString().Contains("Create") && v.ToString().Contains(MenuName))
                    {
                        var val = (model["" + v].ToString());
                        groupMenu.AllowCreate = model["" + v].ToString() == "1" ? true : false;
                        count++;
                    }
                    else if (v.ToString().Contains("Delete") && v.ToString().Contains(MenuName))
                    {
                        groupMenu.AllowDelete = model["" + v].ToString() == "1" ? true : false;
                        count++;
                    }
                    else if (v.ToString().Contains("Edit") && v.ToString().Contains(MenuName))
                    {
                        groupMenu.AllowEdit = model["" + v].ToString() == "1" ? true : false;
                        count++;
                    }
                    else if (v.ToString().Contains("View") && v.ToString().Contains(MenuName))
                    {
                        groupMenu.AllowView = model["" + v].ToString() == "1" ? true : false;
                        count++;
                    }
                    if (count == 5)
                    {
                        GroupProcessing.CreateGroup(Model, groupMenu, out result);
                        groupMenu.AllowCreate = false;
                        groupMenu.AllowView = false;
                        groupMenu.AllowEdit = false;
                        groupMenu.AllowDelete = false;
                        count = 0;

                    }

                }

                if (count > 0 && count < 5)
                {
                    GroupProcessing.CreateGroup(Model, groupMenu, out result);
                }

                return Json(new { status = result, message = "Successfully Created!!!" });
            }

            catch (Exception ex)
            {
                string msg = "Error, Please try again.";
                return Json(new { status = false, message = msg });
            }

        }

        [HttpPost]
        public ActionResult DeleteGroup(string Id)
        {
            try
            {
                bool result;
                string msg;
                GroupProcessing.DeleteGroup(Id, out result, out msg);

                return Json(true);

            }
            catch (Exception e)
            {
                return Json("Error");
            }
        }

        [HttpGet]
        public ActionResult EditGroup(string Id = null)
        {
            if (Id == null)
            {
                return View();
            }
            else
            {
                Session["GroupId"] = Id;
                return Json("/Group/EditGroup", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetGroupsbyId()
        {
            string Id = Session["GroupId"].ToString();
            var list = new List<dynamic>();
            CommonProcessing.GetGroupByIdWithPermissionsTree(out list, Id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateGroup(FormCollection model)
        {
            try
            {
                bool result = true;
                GroupMasterModel Model = new GroupMasterModel();
                GroupMenuModel groupMenu = new GroupMenuModel();
                Model.id = Convert.ToInt32(model["GroupId"]);
                Model.Name = model["GroupName"].ToString();
                Model.Status = Convert.ToInt32(model["Status"]);
                int count = 0;
                string MenuName = "";
                GroupProcessing.DeleteGroupMenu(out result, Model.id);

                foreach (var v in model)
                {
                    if (MenuName != "" && !v.ToString().Contains(MenuName) && count > 0 && count < 5)
                    {
                        GroupProcessing.UpdateGroup(Model, groupMenu, out result);
                        groupMenu.AllowCreate = false;
                        groupMenu.AllowView = false;
                        groupMenu.AllowEdit = false;
                        groupMenu.AllowDelete = false;
                        count = 0;
                    }

                    if (v.ToString().Contains("Menus"))
                    {
                        MenuName = new string(v.ToString().SkipWhile(c => c != '-')
                           .Skip(1)
                           .TakeWhile(c => c != '[')
                           .ToArray()).Trim();
                        string menuid = new string(v.ToString().SkipWhile(c => c != '[')
                           .Skip(1)
                           .TakeWhile(c => c != ']')
                           .ToArray()).Trim();
                        groupMenu.MenuId = Convert.ToInt32(menuid); count++;
                    }
                    else if (v.ToString().Contains("AllowCreate") && v.ToString().Contains(MenuName))
                    {
                        var val = (model["" + v].ToString());
                        groupMenu.AllowCreate = model["" + v].ToString() == "1" ? true : false;
                        count++;
                    }
                    else if (v.ToString().Contains("AllowDelete") && v.ToString().Contains(MenuName))
                    {
                        groupMenu.AllowDelete = model["" + v].ToString() == "1" ? true : false;
                        count++;
                    }
                    else if (v.ToString().Contains("AllowEdit") && v.ToString().Contains(MenuName))
                    {
                        groupMenu.AllowEdit = model["" + v].ToString() == "1" ? true : false;
                        count++;
                    }
                    else if (v.ToString().Contains("AllowView") && v.ToString().Contains(MenuName))
                    {
                        groupMenu.AllowView = model["" + v].ToString() == "1" ? true : false;
                        count++;
                    }

                    if (count == 5)
                    {
                        GroupProcessing.UpdateGroup(Model, groupMenu, out result);
                        groupMenu.AllowCreate = false;
                        groupMenu.AllowView = false;
                        groupMenu.AllowEdit = false;
                        groupMenu.AllowDelete = false;
                        count = 0;
                    }
                }

                if (count > 0 && count < 5)
                {
                    GroupProcessing.UpdateGroup(Model, groupMenu, out result);
                }

                return Json(new { status = result, message = "Successfully Updated!!!" });
            }
            catch (Exception e)
            {
                return Json("Error");
            }

        }

        [HttpPost]
        public ActionResult GetAllGroups() 
        {
            List<GroupMasterModel> groups;
            CommonProcessing.GetAllGroups(out groups);
            return Json(new { groups });
        }
    }
}