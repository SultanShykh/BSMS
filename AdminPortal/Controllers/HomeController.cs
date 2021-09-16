using AdminPortal.AccountProcessing;
using AdminPortal.Helpers;
using AdminPortal.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using AdminPortal.Codebase;

namespace AdminPortal.Controllers
{
    [AuthorizeUser]
    public class HomeController : Controller
    {
        AccountProcessingController apc = new AccountProcessingController();
        public ActionResult Index()
        {
            List<User> users = apc.dbusers() ?? new List<User>();
            return View(users);
        }

        [HttpPost]
        public ActionResult GetUsers()
        {
            List<UserModel> users = apc.dbusers() ?? new List<UserModel>();
            return Json(users);
        }

        public ActionResult Profile(User user) 
        {
            var id = (int)Session["userid"];
            user = apc.dbuserinfo(id).SingleOrDefault();
            return View(user);
        }

        public ActionResult Users()
        {
            if (Session["usertype"].ToString().Equals("4")) 
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        public ActionResult Admin()
        {
            if (Session["usertype"].ToString().Equals("4") || Session["usertype"].ToString().Equals("3") || Session["usertype"].ToString().Equals("3"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Reseller()
        {
            if (Session["usertype"].ToString().Equals("4") || Session["usertype"].ToString().Equals("3"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<Reseller> resellers = apc.dbresellers() ?? new List<Reseller>();
            return View(resellers);
        }

        [HttpPost]
        public ActionResult EditReseller(Reseller reseller)
        {
            var info = apc.dbresellerupdate(reseller.id, reseller.username, reseller.email, reseller.pass, reseller.userrole, reseller.ph_no);
           
            if (!ModelState.IsValid) 
            {
                ModelState.AddModelError("", "Not Updated Successfully");
            }
            ModelState.AddModelError("", "Updated Successfully");
            return RedirectToAction("Reseller", "Home");
        }

        [HttpPost]
        public ActionResult CreateReseller(Reseller reseller, HttpPostedFileBase file = null)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Media"), filename);
                    file.SaveAs(path);

                    var info = apc.dbresellercreate(reseller.username, reseller.email, reseller.pass, reseller.userrole, reseller.ph_no, file.FileName);
                    
                    //send mail
                    MailMessage mm = new MailMessage();
                    mm.To.Add(Session["useremail"].ToString());
                    mm.From = new MailAddress("connect_sultan@outlook.com");
                    mm.Subject = "New user created";
                    mm.Body = "User Name is:"+ reseller.username + "<br> Email is: " + reseller.email +"";
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.outlook.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential("connect_sultan@outlook.com", "killerxx..1");
                    smtp.EnableSsl = true;
                    smtp.Send(mm);
                }
                else
                {
                    var info = apc.dbresellercreate(reseller.username, reseller.email, reseller.pass, reseller.userrole, reseller.ph_no);
                }
            }
            catch (Exception ex)
            { 

            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Not Updated Successfully");
            }
            ModelState.AddModelError("", "Updated Successfully");
            return RedirectToAction("Reseller", "Home");
        }

        [HttpGet]
        public ActionResult DeleteReseller(Reseller reseller)
        {
            var info = apc.dbresellerdelete(reseller.id);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Not Updated Successfully");
            }
            ModelState.AddModelError("", "Updated Successfully");
            return RedirectToAction("Reseller", "Home");
        }

        public FileResult DownloadFile()
        {
            string filename = Path.GetFileName("bg.jpg");
            string path = Path.Combine(Server.MapPath("~/Media"), filename);
            return File(path, "image/jpg","bg.jpg");
        }

    } 
}