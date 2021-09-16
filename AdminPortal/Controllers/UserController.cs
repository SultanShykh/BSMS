using AdminPortal;
using AdminPortal.Codebase;
using AdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPortal.Controllers
{
    public class UserController : Controller
    {
        public const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";
        CommonProcessing c = new CommonProcessing();
        List<GroupMasterModel> groups;
        // GET: Users
        public ActionResult Index()
        {
            List<UserModel> userModel = new List<UserModel>();
            List<Pagination> pagination = new List<Pagination>();
            var pageNum = 0;
            var search = "";
            int userId = Convert.ToInt32(Session["UserId"]);

            try
            {
                search = (Server.UrlEncode(HttpContext.Request.QueryString["search"]) != null) ? Server.UrlEncode(HttpContext.Request.QueryString["search"]) : "";
                if (Server.UrlEncode(HttpContext.Request.QueryString["page"]) != null)
                {
                    pageNum = Convert.ToInt32(Server.UrlEncode(HttpContext.Request.QueryString["page"]));
                }
                else
                {
                    pageNum = 1;
                }

                UserProcessing.GetUsers(pageNum, search, out userModel, out pagination, userId);
                ViewBag.search = search;
                return View(Tuple.Create(userModel, pagination));
            }
            catch (Exception ex)
            {
                ViewBag.search = search;
                return View(Tuple.Create(userModel, pagination));
            }
        }

        [HttpPost]
        public ActionResult CheckUsername(string username)
        {
            bool result;
            UserProcessing.CheckUser(username, out result);
            return Json(result);
        }

        public ActionResult CreateUser()
        {
            try
            {
                CommonProcessing.GetAllGroups(out groups);
                ViewBag.userGroup = new SelectList(groups, "Id", "Name");

                var GetMaskings = c.GetMaskings();
                ViewBag.maskings = new SelectList(GetMaskings,"id","masking");
                return View();
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult CreateUser(UserModel userModel)
        {
           
            bool result;
            string msg;

            if (!ModelState.IsValid)
                return Json(new { status = true, message = "Fields are missing" });
            try
            {
                userModel.parentAccountId = Convert.ToInt32(Session["UserId"]);
                UserProcessing.CreateUser(userModel, out result, out msg);
                return Json(new { status = result, message = msg });
            }
            catch (Exception ex)
            {
                msg = "Error, Please try again.";
                return Json(new { status = false, message = msg });
            }
        }

        [HttpPost]
        public ActionResult UpdateUserGroup(string GroupId, string UserId ) {

            try
            {
                bool result;
                UserProcessing.UpdateUserGroup(UserId, GroupId, out result);
                return Json(result);
            }
            catch(Exception e)
            {
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(string Id)
        {
            try
            {
                bool result;
                string msg;
                UserProcessing.DeleteUser(Id,out result, out msg);
                return Json(new { status = result, message = msg});

            }
            catch(Exception e)
            {
                return Json(new { status = false, message = "Error" });
            }
        }

        [ChildActionOnly]
        public ActionResult _updateUser()
        {
            CommonProcessing.GetAllGroups(out groups);
            ViewBag.userGroup = new SelectList(groups, "Id", "Name");

            var GetMaskings = c.GetMaskings();
            ViewBag.maskings = new SelectList(GetMaskings, "id", "masking");

            return PartialView(PARTIAL_VIEW_FOLDER + "_updateUser.cshtml");
        }

        [HttpPost]
        public ActionResult UpdateUser(UserModel userModel)
        {
            ModelState.Remove("username");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Fields are empty" });
            try
            {
                bool result;

                UserProcessing.UpdateUser(userModel, out result);
                return Json(new { status = result, message = "Successfully Updated!!!" });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "Error" });
            }
        }

        [HttpPost]
        public ActionResult SelectedMaskings(int id)
        {
            string selectedMaskings = "";
            List<Masking> maskings;
            UserProcessing.SelectedUserMaskings(id,out selectedMaskings,out maskings);
            return Json(new { selectedMaskings,maskings });
        }
    }
}