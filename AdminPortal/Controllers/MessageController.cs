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
            ModelState.Remove("camp_time");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Fields are empty" }) ;

            dt.Clear();
            dt.Columns.Add(new DataColumn("camp_id", typeof(Int32)));
            dt.Columns.Add(new DataColumn("user_id", typeof(Int32)));
            dt.Columns.Add(new DataColumn("sender", typeof(string)));
            dt.Columns.Add(new DataColumn("receiver", typeof(string)));
            dt.Columns.Add(new DataColumn("status", typeof(bool)));
            dt.Columns.Add(new DataColumn("route", typeof(Int32)));
            dt.Columns.Add(new DataColumn("cost", typeof(Int32)));
            dt.Columns.Add(new DataColumn("senttime", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("smstype", typeof(string)));
            dt.Columns.Add(new DataColumn("operator", typeof(Int32)));
            dt.Columns.Add(new DataColumn("isswallow", typeof(bool)));
            dt.Columns.Add(new DataColumn("isotpallow", typeof(bool)));
            dt.Columns.Add(new DataColumn("RemoteIP", typeof(string)));
            dt.Columns.Add(new DataColumn("CurrentDateTime", typeof(DateTime)));

            try 
            {
                int count = 0;
                campaign.user_id = Convert.ToInt32(Session["UserId"]);

                var result = m.COR_WEB_createCampaign(campaign);
                var camp_id = result.camp_id;

                foreach (var v in campaign.receiver.Split(','))
                {
                    if (Validation.ValidateRecipient(v.ToString(), out string validNum) == true)
                    {
                        DataRow dr = dt.NewRow();
                    
                        if (dr["receiver"].ToString() != validNum)
                        {
                            dr["camp_id"] = camp_id;
                            dr["user_id"] = campaign.user_id;
                            dr["sender"] = campaign.sender;
                            dr["receiver"] = validNum;
                            dr["status"] = 1;
                            dr["route"] = 4;
                            dr["cost"] = 0;
                            dr["senttime"] = "2021-08-31";
                            dr["smstype"] = campaign.camp_smstype;
                            dr["operator"] = 4;
                            dr["isswallow"] = 1;
                            dr["isotpallow"] = 1;
                            dr["RemoteIP"] = "";
                            dr["CurrentDateTime"] = DateTime.Now;
                            dt.Rows.Add(dr);
                            ++count;
                        }
                    }
                }

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
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex });
            }

            return Json(new { status = true, message = "successfully sent" });
        }

        [HttpPost]
        public ActionResult FetchContacts()
        {
            list.Clear();
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
                        if (!CSVExtension.geContactsFromExcel(path, out list)) return Json(new { status = false, message = "Empty / corrupt File" });
                    }
                    else if (extension == ".csv")
                    {
                        file.SaveAs(path);
                        if (!CSVExtension.getContactsFromCSV(path, out list)) return Json(new { status = false, message = "Empty / corrupt File" });
                    }
                    else
                    {
                        return Json(new { status = false, message = "Please attach csv, xls or xlsx files" });
                    }
                }
                catch (Exception ex) 
                {
                    return Json(new { status = false, message = "Error while Fetching" });
                }
            }

            return Json(new { status = true, message = list });
        }

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

                        if (CSVExtension.getDataFromExcel2(path, campaign.msgdata, out IDictionary<string, string> list,out DataTable dt, campaign, out int count, camp_id))
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
                        
                        if (CSVExtension.getDataFromCSV2(path, campaign.msgdata, out IDictionary<string,string> list, out DataTable dt,campaign ,out int count, camp_id))
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