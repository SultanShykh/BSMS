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
        
        public ActionResult Outbox(int Id = 1,string sender = null, string receiver = null, string datetime = null)
        {
            OutboxProcessing.COR_USP_Outbox(Convert.ToInt32(Session["UserId"]), Id, sender,receiver,datetime, out List<dynamic> outbox, out int totalPages);

            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "Outbox";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }

        public ActionResult OutboxCamp(int Id = 1, string sender = "", string receiver = "",  string datetime = "")
        {
            OutboxProcessing.COR_USP_Outbox_Camp(Convert.ToInt32(Session["UserId"]), Id, sender, receiver, datetime, out List<dynamic> outbox, out int totalPages);

            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "OutboxCamp";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }

        public ActionResult OutboxArchive(int Id = 1, string sender = "", string receiver = "",  string datetime = "")
        {
            OutboxProcessing.COR_USP_Outbox(Convert.ToInt32(Session["UserId"]), Id, sender, receiver, datetime, out List<dynamic> outbox, out int totalPages);

            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "Outbox";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }

        public ActionResult OutboxLive(int Id = 1, string sender = "", string receiver = "", string datetime = "")
        {
            OutboxProcessing.COR_USP_Outbox(Convert.ToInt32(Session["UserId"]), Id, sender, receiver, datetime, out List<dynamic> outbox, out int totalPages);

            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "Outbox";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }
    }
}