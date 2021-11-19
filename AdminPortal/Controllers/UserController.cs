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
        [HttpGet]
        public ActionResult Index(int Id = 1, string username = null, string email = null, string user_mobile = null)
        {
            List<UserModel> userModel = new List<UserModel>();
            List<Pagination> pagination = new List<Pagination>();
            username = string.IsNullOrEmpty(username) ? null : username;
            email = string.IsNullOrEmpty(email) ? null : email;
            user_mobile = string.IsNullOrEmpty(user_mobile) ? null : user_mobile;

            ViewBag.pageNumber = Id;
            ViewBag.username = username;
            ViewBag.email = email;
            ViewBag.user_mobile = user_mobile;

            try
            {
                UserProcessing.GetUsers(Id, username, email, user_mobile, out userModel, out pagination, Convert.ToInt32(Session["UserId"]));
                
                return View(Tuple.Create(userModel, pagination));
            }
            catch (Exception ex)
            {
                return View(Tuple.Create(userModel, pagination));
            }
        }
        [HttpPost]
        public ActionResult CheckUsername(string username)
        {
            UserProcessing.CheckUser(username, out bool result);
            return Json(result);
        }
        public ActionResult CreateUser()
        {
            return View();
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
                msg = "Please try again.";
                return Json(new { status = false, message = msg });
            }
        }
        [ChildActionOnly]
        public ActionResult _updateUser()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult UpdateUser(UserModel userModel)
        {
            ModelState.Remove("username");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Fields are empty" });
            try
            {
                UserProcessing.UpdateUser(userModel, out bool result);
                return Json(new { status = result, message = "Successfully Updated!!!" });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "Error" });
            }
        }
        [HttpPost]
        public ActionResult UpdateMask(List<Masking> masking)
        {
            try
            {
                UserProcessing.UpdateMask(masking, Convert.ToInt32(Session["UserId"]));
                return Json(new { status = true, message = "Successfully Updated!!!" });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "Error" });
            }
        }
        [HttpPost]
        public ActionResult SelectedMaskings(int id)
        {
            string selectedMaskings;
            List<Masking> maskings;
            UserProcessing.SelectedUserMaskings(id,out selectedMaskings,out maskings);
            return Json(new { selectedMaskings,maskings });
        }
    }
}