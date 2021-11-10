using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using AdminPortal.Codebase;

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
        public ActionResult Download(string sender = null, string receiver = null, string datetime = null, string smstype = null)
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
        public ActionResult DownloadCamp(string sender = null, string receiver = null, string datetime = null, string smstype = null)
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