using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using AdminPortal.Codebase;
using AdminPortal.Models;
using AdminPortal.Helpers;
using System.Data;
using CSVLibraryAK;
using System.Diagnostics;
using System.Data.SqlClient;

namespace AdminPortal.Controllers
{
    [AuthorizeUser]
    public class MessageController : Controller
    {
        List<Masking> maskings;
        MessageProcessing m = new MessageProcessing();
        List<string> list = new List<string>();
        
        DataTable dt = new DataTable();

        public ActionResult QuickSMS()
        {
            UserProcessing.SelectedMaskings(Convert.ToInt32(Session["userId"]), out maskings);
            ViewBag.maskings = new SelectList(maskings,"id", "masking");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult QuickSMS(Campaign campaign)
        {
            ModelState.Remove("camp_name");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "sending failed" });

            if (campaign.receiver.Split(',').Count() < 0 || campaign.receiver.Split(',').Count() > 50) 
                return Json(new { status = false, message = "please enter less than 50 or greater than 0 contacts" });

            var validReceivers = "";

            foreach (var v in campaign.receiver.Split(','))
            {
                if (Validation.ValidateRecipient(v.ToString(), out string validNum) == true)
                    validReceivers += validNum + ',';
            }

            if (validReceivers.Equals("")) return Json(new { status = false, message = "Please insert atleast 1 number" });

            try
            {
                campaign.user_id = Convert.ToInt32(Session["userid"]);
                campaign.receiver = validReceivers;

                m.COR_WEB_createMessage(campaign);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex });
            }

            return Json(new { status = true, message = "sent successfully"});
        }

        public ActionResult CampaignSMS()
        {
            UserProcessing.SelectedMaskings(Convert.ToInt32(Session["userId"]), out maskings);
            ViewBag.maskings = new SelectList(maskings, "id", "masking");

            return View();
        }

        [HttpPost]
        public ActionResult CampaignSMS(Campaign campaign)
        {
            UserProcessing.SelectedMaskings(Convert.ToInt32(Session["userId"]), out maskings);
            ViewBag.maskings = new SelectList(maskings, "id", "masking");

            ModelState.Remove("camp_time");
            ModelState.Remove("receiver");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Fields are empty" });

            dt.Clear();

            CSVExtension.makeColumns(out dt);
            dt.Columns.Remove("msgdata");

            if (Request.Files.Count == 1)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];

                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);
                    string path = Path.Combine(Server.MapPath("~/Media/"), fileName);

                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        file.SaveAs(path);

                        var result = m.COR_WEB_createCampaign(campaign);
                        int camp_id = result.camp_id;

                        if (CSVExtension.getContactsFromExcel(path,out list, out int count, out DataTable dt, campaign,camp_id))
                        {
                            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString))
                            {
                                using (SqlBulkCopy sqlbulk = new SqlBulkCopy(con))
                                {
                                    sqlbulk.DestinationTableName = "outbox_Camp";
                                    sqlbulk.ColumnMappings.Add("camp_id", "camp_id");
                                    sqlbulk.ColumnMappings.Add("user_id", "user_id");
                                    sqlbulk.ColumnMappings.Add("sender", "sender");
                                    sqlbulk.ColumnMappings.Add("receiver", "receiver");
                                    sqlbulk.ColumnMappings.Add("status", "status");
                                    sqlbulk.ColumnMappings.Add("route", "route");
                                    sqlbulk.ColumnMappings.Add("cost", "cost");
                                    sqlbulk.ColumnMappings.Add("senttime", "senttime");
                                    sqlbulk.ColumnMappings.Add("smstype", "smstype");
                                    sqlbulk.ColumnMappings.Add("operator", "operator");
                                    sqlbulk.ColumnMappings.Add("isswallow", "isswallow");
                                    sqlbulk.ColumnMappings.Add("isotpallow", "isotpallow");
                                    sqlbulk.ColumnMappings.Add("RemoteIP", "RemoteIP");
                                    sqlbulk.ColumnMappings.Add("CurrentDateTime", "CurrentDateTime");

                                    con.Open();
                                    sqlbulk.WriteToServer(dt);
                                    sqlbulk.BatchSize = 5000;
                                    sqlbulk.Close();
                                    con.Close();
                                }
                            }

                            m.COR_WEB_updateCampaign(camp_id, count);
                        }
                        else
                        {
                            ViewBag.result = "Empty / Corrupt File";
                            ViewBag.status = "danger";
                            return View();
                        }
                    }
                    else if (extension == ".csv")
                    {
                        file.SaveAs(path);

                        var result = m.COR_WEB_createCampaign(campaign);
                        int camp_id = result.camp_id;

                        if (CSVExtension.getContactsFromCSV(path, out list, out int count, out DataTable dt, campaign, camp_id))
                        {
                            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString))
                            {
                                using (SqlBulkCopy sqlbulk = new SqlBulkCopy(con))
                                {
                                    sqlbulk.DestinationTableName = "outbox_Camp";
                                    sqlbulk.ColumnMappings.Add("camp_id", "camp_id");
                                    sqlbulk.ColumnMappings.Add("user_id", "user_id");
                                    sqlbulk.ColumnMappings.Add("sender", "sender");
                                    sqlbulk.ColumnMappings.Add("receiver", "receiver");
                                    sqlbulk.ColumnMappings.Add("status", "status");
                                    sqlbulk.ColumnMappings.Add("route", "route");
                                    sqlbulk.ColumnMappings.Add("cost", "cost");
                                    sqlbulk.ColumnMappings.Add("senttime", "senttime");
                                    sqlbulk.ColumnMappings.Add("smstype", "smstype");
                                    sqlbulk.ColumnMappings.Add("operator", "operator");
                                    sqlbulk.ColumnMappings.Add("isswallow", "isswallow");
                                    sqlbulk.ColumnMappings.Add("isotpallow", "isotpallow");
                                    sqlbulk.ColumnMappings.Add("RemoteIP", "RemoteIP");
                                    sqlbulk.ColumnMappings.Add("CurrentDateTime", "CurrentDateTime");

                                    con.Open();
                                    sqlbulk.WriteToServer(dt);
                                    sqlbulk.BatchSize = 5000;
                                    sqlbulk.Close();
                                    con.Close();
                                }
                            }

                            m.COR_WEB_updateCampaign(camp_id, count);
                        }
                        else
                        {
                            ViewBag.result = "Empty / Corrupt File";
                            ViewBag.status = "danger";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.result = "Please attach csv, xls or xlsx files";
                        ViewBag.status = "danger";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.result = ex;
                    ViewBag.status = "danger";
                    return View();
                }
            }
            else
            {
                ViewBag.result = "Please upload single file only";
                ViewBag.status = "danger";
                return View();
            }

            ViewBag.result = "successfully sent";
            ViewBag.status = "success";
            return View();

        }

        //[HttpPost]
        //public ActionResult FetchContacts()
        //{
        //    list.Clear();
        //    if (Request.Files.Count == 1) 
        //    {
        //        try
        //        {
        //            HttpFileCollectionBase files = Request.Files;
        //            HttpPostedFileBase file = files[0];

        //            string fileName = Path.GetFileName(file.FileName);
        //            string extension = Path.GetExtension(fileName);
        //            string path = Path.Combine(Server.MapPath("~/Media/"), fileName);

        //            if (extension == ".xls" || extension == ".xlsx")
        //            {
        //                file.SaveAs(path);
        //                if (!CSVExtension.geContactsFromExcel(path, out list)) return Json(new { status = false, message = "Empty / corrupt File" });
        //            }
        //            else if (extension == ".csv")
        //            {
        //                file.SaveAs(path);
        //                if (!CSVExtension.getContactsFromCSV(path, out list)) return Json(new { status = false, message = "Empty / corrupt File" });
        //            }
        //            else
        //            {
        //                return Json(new { status = false, message = "Please attach csv, xls or xlsx files" });
        //            }
        //        }
        //        catch (Exception ex) 
        //        {
        //            return Json(new { status = false, message = "Error while Fetching" });
        //        }
        //    }

        //    return Json(new { status = true, message = list });
        //}

        public ActionResult PersonalizedSMS()
        {
            UserProcessing.SelectedMaskings(Convert.ToInt32(Session["userId"]), out maskings);
            ViewBag.maskings = new SelectList(maskings, "id", "masking");
            return View();
        }

        [HttpPost]
        public ActionResult PersonalizedSMS(Campaign campaign)
        {
            UserProcessing.SelectedMaskings(Convert.ToInt32(Session["userId"]), out maskings);
            ViewBag.maskings = new SelectList(maskings, "id", "masking");

            ModelState.Remove("receiver");
            if (!ModelState.IsValid)
                return View();

            if (Request.Files.Count == 1)
            {
                try
                {
                    string numbers = "";
                    string msg = "";
                    string msgdata = campaign.msgdata;
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];

                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);
                    string path = Path.Combine(Server.MapPath("~/Media/"), fileName);
                        
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        file.SaveAs(path);
                        
                        campaign.msgdata = "";
                        var result = m.COR_WEB_createCampaign(campaign);
                        int camp_id = result.camp_id;
                        campaign.msgdata = msgdata;

                        if (CSVExtension.getDataFromExcel(path, campaign.msgdata, out IDictionary<string, string> list,out DataTable dt, campaign, out int count, camp_id))
                        {
                            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString))
                            {
                                using (SqlBulkCopy sqlbulk = new SqlBulkCopy(con))
                                {
                                    sqlbulk.DestinationTableName = "outbox";
                                    sqlbulk.ColumnMappings.Add("camp_id", "camp_id");
                                    sqlbulk.ColumnMappings.Add("user_id", "user_id");
                                    sqlbulk.ColumnMappings.Add("sender", "sender");
                                    sqlbulk.ColumnMappings.Add("receiver", "receiver");
                                    sqlbulk.ColumnMappings.Add("msgdata", "msgdata");
                                    sqlbulk.ColumnMappings.Add("status", "status");
                                    sqlbulk.ColumnMappings.Add("route", "route");
                                    sqlbulk.ColumnMappings.Add("cost", "cost");
                                    sqlbulk.ColumnMappings.Add("senttime", "senttime");
                                    sqlbulk.ColumnMappings.Add("smstype", "smstype");
                                    sqlbulk.ColumnMappings.Add("operator", "operator");
                                    sqlbulk.ColumnMappings.Add("isswallow", "isswallow");
                                    sqlbulk.ColumnMappings.Add("isotpallow", "isotpallow");
                                    sqlbulk.ColumnMappings.Add("RemoteIP", "RemoteIP");
                                    sqlbulk.ColumnMappings.Add("CurrentDateTime", "CurrentDateTime");

                                    con.Open();
                                    sqlbulk.WriteToServer(dt);
                                    sqlbulk.BatchSize = 5000;
                                    sqlbulk.Close();
                                    con.Close();
                                }
                            }

                            m.COR_WEB_updateCampaign(camp_id,count);
                        }
                        else
                        {
                            ViewBag.result = "Empty / Corrupt File";
                            ViewBag.status = "danger";
                            return View();
                        }

                        campaign.receiver = numbers;
                        campaign.msgdata = msg;
                    }
                    else if (extension == ".csv")
                    {
                        file.SaveAs(path);

                        campaign.msgdata = "";
                        var result = m.COR_WEB_createCampaign(campaign);
                        int camp_id = result.camp_id;
                        campaign.msgdata = msgdata;
                        
                        if (CSVExtension.getDataFromCSV(path, campaign.msgdata, out IDictionary<string,string> list, out DataTable dt,campaign ,out int count, camp_id))
                        {
                            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString))
                            {
                                using (SqlBulkCopy sqlbulk = new SqlBulkCopy(con))
                                {
                                    sqlbulk.DestinationTableName = "outbox";
                                    sqlbulk.ColumnMappings.Add("camp_id", "camp_id");
                                    sqlbulk.ColumnMappings.Add("user_id", "user_id");
                                    sqlbulk.ColumnMappings.Add("sender", "sender");
                                    sqlbulk.ColumnMappings.Add("receiver", "receiver");
                                    sqlbulk.ColumnMappings.Add("msgdata", "msgdata");
                                    sqlbulk.ColumnMappings.Add("status", "status");
                                    sqlbulk.ColumnMappings.Add("route", "route");
                                    sqlbulk.ColumnMappings.Add("cost", "cost");
                                    sqlbulk.ColumnMappings.Add("senttime", "senttime");
                                    sqlbulk.ColumnMappings.Add("smstype", "smstype");
                                    sqlbulk.ColumnMappings.Add("operator", "operator");
                                    sqlbulk.ColumnMappings.Add("isswallow", "isswallow");
                                    sqlbulk.ColumnMappings.Add("isotpallow", "isotpallow");
                                    sqlbulk.ColumnMappings.Add("RemoteIP", "RemoteIP");
                                    sqlbulk.ColumnMappings.Add("CurrentDateTime", "CurrentDateTime");

                                    con.Open();
                                    sqlbulk.WriteToServer(dt);
                                    sqlbulk.BatchSize = 5000;
                                    sqlbulk.Close();
                                    con.Close();
                                }
                            }

                            m.COR_WEB_updateCampaign(camp_id, count);
                        }
                        else
                        {
                            ViewBag.result = "Empty / Corrupt File";
                            ViewBag.status = "danger";
                            return View();
                        }

                        campaign.receiver = numbers;
                        campaign.msgdata = msg;
                    }
                    else
                    {
                        ViewBag.result = "Please attach csv, xls or xlsx files";
                        ViewBag.status = "danger";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.result = ex;
                    ViewBag.status = "danger";
                    return View();
                }
            }
            else
            {
                ViewBag.result = "Please upload single file only";
                ViewBag.status = "danger";
                return View();
            }

            ViewBag.result = "successfully sent";
            ViewBag.status = "success";
            return View();
        }
    }
}