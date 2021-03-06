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
using System.Collections;

namespace AdminPortal.Controllers
{
    [AuthorizeUser]
    public class MessageController : Controller
    {
        MessageProcessing m = new MessageProcessing();
        List<string> list = new List<string>();
        IDictionary<string,string> dict = new Dictionary<string, string>();
        DataTable dt = new DataTable();
        public ActionResult QuickSMS()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult QuickSMS(Campaign campaign)
        {
            ModelState.Remove("camp_name");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Fields are missing" });

            if (campaign.receiver.Split(',').Count() < 0 || campaign.receiver.Split(',').Count() > 50) 
                return Json(new { status = false, message = "please enter less than 50 or greater than 0 contacts" });

            var validReceivers = "";
            list.Clear();

            foreach (var v in campaign.receiver.Split(','))
            {
                if (Validation.ValidateRecipient(v.ToString(), out string validNum) == true)
                    list.Add(validNum);
            }

            list = list.Distinct().ToList();

            if (list.Count <= 0) return Json(new { status = false, message = "Please insert atleast 1 number" });

            foreach (var v in list) 
            {
                validReceivers += v + ',';
            }

            try
            {
                campaign.user_id = Convert.ToInt32(Session["userid"]);
                campaign.receiver = validReceivers;
                campaign.camp_smstype = 1;

                m.COR_WEB_createMessage(campaign);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex });
            }
            
            return Json(new { status = true, message = "Sent: " + list.Count() + " Failed: 0" });
        }
        public ActionResult CampaignSMS()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CampaignSMS(Campaign campaign)
        {
            ModelState.Remove("camp_time");
            ModelState.Remove("receiver");
            if (!ModelState.IsValid) return Json(new { status = false, message = "Fields are empty" });

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
                    string path = Path.Combine(Server.MapPath("~/UploadFiles/"), fileName);

                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        file.SaveAs(path);

                        campaign.user_id = Convert.ToInt32(Session["UserId"]);
                        campaign.camp_smstype = 4;

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

                        campaign.user_id = Convert.ToInt32(Session["UserId"]);
                        campaign.camp_smstype = 4;

                        var result = m.COR_WEB_createCampaign(campaign);
                        campaign.id = result.camp_id;

                        if (CSVExtension.getContactsFromCSV(path, out list, out int count, out DataTable dt, campaign))
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

                            m.COR_WEB_updateCampaign(campaign.id, count);
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
                    System.IO.File.Delete(path);
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
            
            ViewBag.result = "Sent "+list.Count()+" Failed: 0";
            ViewBag.status = "info";
            return View();

        }
        public ActionResult PersonalizedSMS()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PersonalizedSMS(Campaign campaign)
        {
            ModelState.Remove("receiver");
            if (!ModelState.IsValid)
                return View();

            if (Request.Files.Count == 1)
            {
                try
                {
                    string msgdata = campaign.msgdata;
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];

                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);
                    string path = Path.Combine(Server.MapPath("~/UploadFiles/"), fileName);
                        
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        file.SaveAs(path);
                        
                        campaign.msgdata = "";
                        campaign.user_id = Convert.ToInt32(Session["UserId"]);
                        campaign.camp_smstype = 3;

                        var result = m.COR_WEB_createCampaign(campaign);
                        campaign.id = result.camp_id;
                        campaign.msgdata = msgdata;

                        if (CSVExtension.getDataFromExcel(path, out dict, out DataTable dt, campaign, out int count, out int flag))
                        {
                            if (flag == 1)
                            {
                                ViewBag.result = "Invalid/Corrupt file";
                                ViewBag.status = "danger";
                                System.IO.File.Delete(path);
                                return View();
                            }
                            else if (flag == 2) 
                            {
                                ViewBag.result = "Please select a column or Add a selected column in the file";
                                ViewBag.status = "danger";
                                System.IO.File.Delete(path);
                                return View();
                            }
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

                            m.COR_WEB_updateCampaign(campaign.id, count);
                        }
                        else
                        {
                            ViewBag.result = "Please select a column or Add a selected column in the file";
                            ViewBag.status = "danger";
                            return View();
                        }
                    }
                    else if (extension == ".csv")
                    {
                        file.SaveAs(path);

                        campaign.msgdata = "";
                        campaign.user_id = Convert.ToInt32(Session["UserId"]);
                        campaign.camp_smstype = 3;

                        var result = m.COR_WEB_createCampaign(campaign);
                        campaign.id = result.camp_id;
                        campaign.msgdata = msgdata;
                        
                        if (CSVExtension.getDataFromCSV(path, out dict, out DataTable dt,campaign ,out int count, out int flag))
                        {
                            if (flag == 1)
                            {
                                ViewBag.result = "Invalid/Corrupt file";
                                ViewBag.status = "danger";
                                System.IO.File.Delete(path);
                                return View();
                            }
                            else if (flag == 2)
                            {
                                ViewBag.result = "Please select a column or Add a selected column in the file";
                                ViewBag.status = "danger";
                                System.IO.File.Delete(path);
                                return View();
                            }
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

                            m.COR_WEB_updateCampaign(campaign.id, count);
                        }
                        else
                        {
                            ViewBag.result = "Please select a column or Add a column in the file";
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
                    System.IO.File.Delete(path);
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
            
            ViewBag.result = "Sent " + dict.Count() + " Failed: 0";
            ViewBag.status = "info";

            return View();
        }
        public ActionResult CampaignManagement() 
        {
            m.COR_WEB_CampaignManagement(Convert.ToInt32(Session["UserId"]), out List<dynamic> stats, out List<dynamic> details);
            return View(Tuple.Create(stats, details));
        }
        [ChildActionOnly]
        public ActionResult _AddGroup() 
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult _ViewContacts(FormCollection collection)
        {
            try
            {
                var list = ContactsProcessing.ViewContacts(Convert.ToInt32(Session["UserId"]), Convert.ToInt32(collection["Id"]));
                return Json(new { status = true, message = list });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, message = ex.Message.ToString() });
            }
        }
    }
}