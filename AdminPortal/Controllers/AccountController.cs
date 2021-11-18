using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPortal.Models;
using AdminPortal.Codebase;

namespace AdminPortal.Controllers
{
    public class AccountController : Controller
    {
        AccountProcessing ac = new AccountProcessing();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(UserModel userModel)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                var user = ac.UserLogin(userModel.username, userModel.password).FirstOrDefault();
                string message = string.Empty;
                UserModel u = user;
                switch (user.Response)
                {
                    case -1:
                        message = "Username and/or password is incorrect.";
                        break;
                    case -2:
                        message = "Account has not been activated.";
                        break;
                    case -3:
                        message = "Account has been expired.";
                        break;
                    default:
                        {
                            Session["UserId"] = u.Id;
                            Session["name"] = u.username;
                            return RedirectToAction("Index", "Home");
                        }
                }
                ViewBag.form = "login";
                ModelState.AddModelError("", message);
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { reponse = true});
            }
        }
        public ActionResult Logout() 
        {
            Session["UserId"] = Session["name"] = null;
            return RedirectToAction("Login","Account");
        }
    }
}