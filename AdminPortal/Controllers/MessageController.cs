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

namespace AdminPortal.Controllers
{
    [AuthorizeUser]
    public class MessageController : Controller
    {
        List<Masking> maskings;
        MessageProcessing m = new MessageProcessing();
        public ActionResult QuickSMS()
        {
            UserProcessing.SelectedMaskings(Convert.ToInt32(Session["userId"]), out maskings);
            ViewBag.maskings = new SelectList(maskings,"id", "masking");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult QuickSMS(Campaign campaign)
        {
            UserProcessing.SelectedMaskings(Convert.ToInt32(Session["userId"]), out maskings);
            ViewBag.maskings = new SelectList(maskings, "id", "masking");

            ModelState.Remove("camp_name");

            if (!ModelState.IsValid)
                return Json(new { status = false, message = "sending failed" });

            var validRes = "";
            var numbers = campaign.receiver.Split(',');
            campaign.user_id = Convert.ToInt32(Session["userid"]);
            
                if (numbers.Count() > 0 && numbers.Count() <= 50)
                {
                    foreach (var v in numbers)
                    {
                        if (Validation.ValidateRecipient(v.ToString(), out string validNum) == true) validRes += validNum + ',';
                    }
                    campaign.receiver = validRes;

                    try
                    { 
                        m.createMessage(campaign); 
                    }
                    catch (Exception ex) 
                    {
                        return Json(new { status = false, message = "sending failed server error" });
                    }
                }
                else 
                {
                    return Json(new { status = false, message = "please enter less than 50 contacts" });
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
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Fields are empty" }) ;

            var numbers = campaign.receiver.Split(',');
            campaign.user_id = Convert.ToInt32(Session["userid"]);
            var validRes = "";

            foreach (var v in numbers)
            {
                if (Validation.ValidateRecipient(v.ToString(), out string validNum) == true)
                    validRes += validNum + ',';
            }
            try
            {
                campaign.receiver = validRes;
                //for () 
                //{

                //}
                m.createCampaign(campaign);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "server error" });
            } 
            
            return Json(new { status = true, message = "successfully sent" });
        }

        [HttpPost]
        public ActionResult FetchContacts()
        {
            List<string> list = new List<string>();
            if (Request.Files.Count > 0) 
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++) 
                    {
                        HttpPostedFileBase file = files[i];
                        string fileName = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(fileName);
                        string path = Path.Combine(Server.MapPath("~/Media/"), fileName);

                        if (extension == ".xls" || extension == ".xlsx")
                        {
                            file.SaveAs(path);
                            CSVExtension.getDataFromExcel(path, out list);
                        }
                        else if (extension == ".csv")
                        {
                            file.SaveAs(path);
                            CSVExtension.getContactsFromCSV(path, out list);
                        }
                        else
                        {
                            return Json(new { status = false, message = "Please attach csv, xls or xlsx files" });
                        }
                    }
                }
                catch (Exception ex) 
                {
                    return Json(new { status = false, message = "Corrupt File" });
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

            if (Request.Files.Count > 0)
            {
                try
                {
                    string numbers = "";
                    string msg = "";
                    HttpFileCollectionBase files = Request.Files;

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fileName = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(fileName);

                        if (extension.Contains("csv") || extension.Contains("xls") || extension.Contains("xlsx"))
                        {
                            string path = Path.Combine(Server.MapPath("~/Media/"), fileName);
                            file.SaveAs(path);

                            if (extension == ".xls" || extension == ".xlsx")
                            {
                                if (CSVExtension.getDataFromExcel1(path, campaign.msgdata, out IDictionary<string, string> list))
                                {
                                    foreach (KeyValuePair<string, string> keyVal in list)
                                    {
                                        numbers += keyVal.Key;
                                        msg += keyVal.Value;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = "Corrupt File";
                                    ViewBag.status = "danger";
                                    return View();
                                }

                                campaign.receiver = numbers;
                                campaign.msgdata = msg;
                            }
                            else if (extension == ".csv")
                            {
                                if (CSVExtension.getDataFromCSV(path, campaign.msgdata, out IDictionary<string, string> list))
                                {
                                    foreach (KeyValuePair<string, string> keyVal in list)
                                    {
                                        numbers += keyVal.Key + ",";
                                        msg += keyVal.Value + ",";
                                    }
                                }
                                else
                                {
                                    ViewBag.result = "Corrupt File";
                                    ViewBag.status = "danger";
                                    return View();
                                }

                                campaign.receiver = numbers;
                                campaign.msgdata = msg;
                            }
                        }
                        else 
                        {
                            ViewBag.result = "uploaded file must be in csv, xls or xlsx format";
                            ViewBag.status = "danger";
                            return View();
                        }
                        
                    }
                }
                catch (Exception ex)
                {

                }
            }
            ViewBag.result = "successfully sent";
            ViewBag.status = "success";
            return View();
        }
    }
}