using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using AdminPortal.Codebase;
using AdminPortal.Models;

namespace AdminPortal.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Outbox(int Id = 1,string sender = null, string receiver = null, string datetime = null, string smstype = null)
        {
            ViewBag.datetime = datetime;
            datetime = string.IsNullOrEmpty(datetime) ? null: datetime.Replace(" ", "");
            sender = string.IsNullOrEmpty(sender) ? null : sender;
            receiver = string.IsNullOrEmpty(receiver) ? null : receiver;
            smstype = string.IsNullOrEmpty(smstype) ? null : smstype;

            OutboxProcessing.COR_USP_Outbox(Convert.ToInt32(Session["UserId"]), Id, sender, receiver, datetime, smstype, out List<dynamic> outbox, out int totalPages);
            
            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "Outbox";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;
            ViewBag.smstype = smstype;

            return View("Outbox", outbox);
        }
        public ActionResult OutboxCamp(int Id = 1, string sender = null, string receiver = null, string datetime = null, string smstype = null)
        {
            ViewBag.datetime = datetime;
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = string.IsNullOrEmpty(sender) ? null : sender;
            receiver = string.IsNullOrEmpty(receiver) ? null : receiver;
            smstype = string.IsNullOrEmpty(smstype) ? null : smstype;

            OutboxProcessing.COR_USP_Outbox_Camp(Convert.ToInt32(Session["UserId"]), Id, sender, receiver, datetime, smstype, out List<dynamic> outbox, out int totalPages);

            ViewBag.PageNumber = Id;    
            ViewBag.totalPages = totalPages;
            ViewBag.action = "OutboxCamp";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }
        public ActionResult Inbox(int Id = 1, string sender = null, string receiver = null)
        {
            sender = string.IsNullOrEmpty(sender) ? null : sender;
            receiver = string.IsNullOrEmpty(receiver) ? null : receiver;

            OutboxProcessing.COR_USP_Inbox(Id, sender, receiver, out List<Inbox> inbox, out int totalPages);

            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "Inbox";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View(inbox);
        }
        public ActionResult ActivityLog(int Id = 1)
        {
            OutboxProcessing.COR_USP_ActivityLogs(Id, out List<ActivityLog> ActivityLog, out int totalPages);
            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "ActivityLog";

            return View(ActivityLog);
        }
        public ActionResult SMSSummary(int Id = 1, string sender = null)
        {
            sender = string.IsNullOrEmpty(sender) ? null : sender;
            //receiver = string.IsNullOrEmpty(receiver) ? null : receiver;
            //smstype = string.IsNullOrEmpty(smstype) ? null : smstype;

            OutboxProcessing.COR_USP_SMSSummary(Convert.ToInt32(Session["UserId"]), Id, sender, out List<SMSSummaryModel> SMSSummary, out int totalPages);

            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "SMSSummary";
            ViewBag.sender = sender;

            return View(SMSSummary);
        }
        public FileResult Download(string sender = null, string receiver = null, string datetime = null, string smstype = null)
        {
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = string.IsNullOrEmpty(sender) ? null : sender;
            receiver = string.IsNullOrEmpty(receiver) ? null : receiver;
            smstype = string.IsNullOrEmpty(smstype) ? null : smstype;

            OutboxProcessing.COR_USP_OutboxDownload(Convert.ToInt32(Session["UserId"]), sender, receiver, datetime, smstype, out List<dynamic> outbox);

            StringBuilder sb = new StringBuilder();
            foreach (var v in outbox) 
            {
                sb.Append(v.username+","+v.smstype+","+v.masking+","+v.receiver+","+v.msgdata+","+v.senttime+","+v.status+","+v.cost+","+v.route+",\r\n");
            }
            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "outbox.csv");
        }
        public FileResult DownloadCamp(string sender = null, string receiver = null, string datetime = null, string smstype = null)
        {
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = string.IsNullOrEmpty(sender) ? null : sender;
            receiver = string.IsNullOrEmpty(receiver) ? null : receiver;
            smstype = string.IsNullOrEmpty(smstype) ? null : smstype;

            OutboxProcessing.COR_USP_OutboxCampDownload(Convert.ToInt32(Session["UserId"]), sender, receiver, datetime, smstype, out List<dynamic> outbox);
            StringBuilder sb = new StringBuilder();
            foreach (var v in outbox)
            {
                sb.Append(v.username + "," + v.smstype + "," + v.masking + "," + v.receiver + "," + v.msgdata + "," + v.senttime + "," + v.status + "," + v.cost + "," + v.route + ",\r\n");
            }
            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "outboxCampaign.csv");
        }
    }
}