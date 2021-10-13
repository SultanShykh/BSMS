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
        public ActionResult Index()
        {
            return View();
        }

        //public FileResult DownloadFile()
        //{
        //    string filename = Path.GetFileName("bg.jpg");
        //    string path = Path.Combine(Server.MapPath("~/Media"), filename);
        //    return File(path, "image/jpg","bg.jpg");
        //}
    } 
}