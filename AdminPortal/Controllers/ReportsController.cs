using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPortal.Codebase;
using AdminPortal.Helpers;
using AdminPortal.Models;
using ClosedXML.Excel;

namespace AdminPortal.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Outbox(int Id = 1,string sender = null, string receiver = null, string datetime = null)
        {
            ViewBag.datetime = datetime;
            datetime = string.IsNullOrEmpty(datetime) ? null: datetime.Replace(" ", "");
            sender = sender == "" ? null : sender;
            receiver = receiver == "" ? null : receiver;
            OutboxProcessing.COR_USP_Outbox(Convert.ToInt32(Session["UserId"]), Id, sender,receiver,datetime, out List<dynamic> outbox, out int totalPages);
            
            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "Outbox";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }
        public ActionResult OutboxCamp(int Id = 1, string sender = null, string receiver = null, string datetime = null)
        {
            ViewBag.datetime = datetime;
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = sender == "" ? null : sender;
            receiver = receiver == "" ? null : receiver;
            OutboxProcessing.COR_USP_Outbox_Camp(Convert.ToInt32(Session["UserId"]), Id, sender, receiver, datetime, out List<dynamic> outbox, out int totalPages);

            ViewBag.PageNumber = Id;    
            ViewBag.totalPages = totalPages;
            ViewBag.action = "OutboxCamp";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }
        public ActionResult Download(string sender = null, string receiver = null, string datetime = null)
        {
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = sender == "" ? null : sender;
            receiver = receiver == "" ? null : receiver;
            OutboxProcessing.COR_USP_OutboxDownload(Convert.ToInt32(Session["UserId"]), sender, receiver, datetime, out List<dynamic> outbox);

            DataTable dt = new DataTable();
            CSVExtension.getData(dt);
            string fileName = "outbox.xlsx";

            foreach (var v in outbox) 
            {
                DataRow dr = dt.NewRow();

                dr["username"] = v.username;
                dr["smstype"] = Convert.ToInt32(v.smstype);
                dr["masking"] = v.masking;
                dr["receiver"] = v.receiver;
                dr["msgdata"] = v.msgdata;
                dr["senttime"] = Convert.ToDateTime(v.senttime).ToString("dd/MM/yyyy HH:mm:ss");
                dr["status"] = v.status;
                dr["cost"] = Convert.ToInt32(v.cost);
                dr["route"] = Convert.ToInt32(v.route);
                dt.Rows.Add(dr);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                //Response.Clear();
                //Response.Buffer = true;
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xlsx");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        public ActionResult DownloadCamp(string sender = null, string receiver = null, string datetime = null)
        {
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = sender == "" ? null : sender;
            receiver = receiver == "" ? null : receiver;
            OutboxProcessing.COR_USP_OutboxCampDownload(Convert.ToInt32(Session["UserId"]), sender, receiver, datetime, out List<dynamic> outbox);

            DataTable dt = new DataTable();
            CSVExtension.getData(dt);
            string fileName = "outbox campaign.xlsx";

            foreach (var v in outbox)
            {
                DataRow dr = dt.NewRow();

                dr["username"] = v.username;
                dr["smstype"] = Convert.ToInt32(v.smstype);
                dr["masking"] = v.masking;
                dr["receiver"] = v.receiver;
                dr["msgdata"] = v.msgdata;
                dr["senttime"] = Convert.ToDateTime(v.senttime).ToString("dd/MM/yyyy HH:mm:ss");
                dr["status"] = v.status;
                dr["cost"] = Convert.ToInt32(v.cost);
                dr["route"] = Convert.ToInt32(v.route);
                dt.Rows.Add(dr);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                //Response.Clear();
                //Response.Buffer = true;
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xlsx");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //stream.WriteTo(Response.OutputStream);
                    //Response.Flush();
                    //Response.End();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }
    }
}