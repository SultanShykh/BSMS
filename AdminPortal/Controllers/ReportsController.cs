using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPortal.Codebase;
using AdminPortal.Models;

namespace AdminPortal.Controllers
{
    public class ReportsController : Controller
    {
        OutboxProcessing op = new OutboxProcessing();
        public ActionResult OutboxLive()
        {
            return View("Outbox");
        }
        public ActionResult OutboxArchive()
        {
            return View("Outbox");
        }
        public ActionResult Outbox(int Id = 1)
        {
            OutboxProcessing.COR_USP_Outbox(Convert.ToInt32(Session["UserId"]), Id, out List<dynamic> outbox,out int totalPages);
            
            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;

            return View("Outbox", outbox);
        }
    }
}