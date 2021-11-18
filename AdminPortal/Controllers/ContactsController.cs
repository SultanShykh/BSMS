using AdminPortal.Codebase;
using AdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using AdminPortal.Helpers;
using ClosedXML.Excel;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace AdminPortal.Controllers
{
    public class ContactsController : Controller
    {
        public ActionResult ManageContacts(int Id = 1, string fullname = null, string email = null)
        {
            ViewBag.pageNumber = Id;
            ViewBag.fullname = fullname;
            ViewBag.email = email;

            fullname = fullname == "" ? null : fullname;
            email = email == "" ? null : email;
            ContactsProcessing.GetContacts(Id, fullname, email, Convert.ToInt32(Session["UserId"]), out List<ContactsModel> contacts, out int totalPages);
            ViewBag.pageNumber = Id;
            ViewBag.totalPages = totalPages;
            return View(contacts);
        }
        public ActionResult ManageGroups(int Id = 1)
        {
            ContactsProcessing.GetGroups(Id, Convert.ToInt32(Session["UserId"]), out List<dynamic> contacts, out int totalPages);
            ViewBag.pageNumber = Id;
            ViewBag.totalPages = totalPages;
            return View(contacts);
        }
        [ChildActionOnly]
        public ActionResult _AddContact()
        {
            return PartialView(new ContactsModel());
        }
        [HttpPost]
        public ActionResult _AddContact(ContactsModel contactsModel)
        {
            ModelState.Remove("id");
            if (!ModelState.IsValid) return Json(new { status = false, message = "Fields are empty" });

            contactsModel.user_id = Convert.ToInt32(Session["UserId"]);
            contactsModel.fullname = contactsModel.fullname == "" ? null : contactsModel.fullname;
            contactsModel.emails = contactsModel.emails == "" ? null : contactsModel.emails;
            contactsModel.option1 = contactsModel.option1 == "" ? null : contactsModel.option1;
            contactsModel.option2 = contactsModel.option2 == "" ? null : contactsModel.option2;
            contactsModel.option3 = contactsModel.option3 == "" ? null : contactsModel.option3;
            contactsModel.groupname = contactsModel.groupname == "" ? null : contactsModel.groupname;
            try
            {
                string result = ContactsProcessing.COR_Contacts_AddContact(contactsModel);
                return Json(new { status = true, message = result });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message.ToString() });
            }
        }
        [ChildActionOnly]
        public ActionResult _UpdateContact()
        {
            return PartialView(new ContactsModel());
        }
        [HttpPost]
        public ActionResult _UpdateContact(ContactsModel contactsModel)
        {
            if (!ModelState.IsValid) return Json(new { status = false, message = "Fields are empty" });

            contactsModel.fullname = contactsModel.fullname == "" ? null : contactsModel.fullname;
            contactsModel.emails = contactsModel.emails == "" ? null : contactsModel.emails;
            contactsModel.option1 = contactsModel.option1 == "" ? null : contactsModel.option1;
            contactsModel.option2 = contactsModel.option2 == "" ? null : contactsModel.option2;
            contactsModel.option3 = contactsModel.option3 == "" ? null : contactsModel.option3;
            contactsModel.groupname = contactsModel.groupname == "" ? null : contactsModel.groupname;

            try
            {
                string result = ContactsProcessing.COR_Contacts_UpdateContact(contactsModel);
                return Json(new { status = true, message = result });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message.ToString() });
            }
        }
        [ChildActionOnly]
        public ActionResult _AddGroup()
        {
            return PartialView(new ContactGroupModel());
        }
        [HttpPost]
        public ActionResult _AddGroup(ContactGroupModel groupModel)
        {
            ModelState.Remove("id");
            if (!ModelState.IsValid) return Json(new { status = false, message = "Fields are empty" });

            groupModel.user_id = Convert.ToInt32(Session["UserId"]);
            try
            {
                ContactsProcessing.COR_Contacts_AddGroup(groupModel, out string result, out string status);
                return Json(new { status = status, message = result });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message.ToString() });
            }
        }
        [ChildActionOnly]
        public ActionResult _UpdateGroup()
        {
            return PartialView(new ContactGroupModel());
        }
        [HttpPost]
        public ActionResult _UpdateGroup(ContactGroupModel group)
        {
            if (!ModelState.IsValid) return Json(new { status = false, message = "Fields are empty" });

            group.description = group.description == "" ? null : group.description;
            try
            {
                ContactsProcessing.COR_Contacts_UpdateGroup(group, out string result, out string status);
                return Json(new { status = status, message = result });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message.ToString() });
            }
        }
        [HttpGet]
        public JsonResult DeleteGroup(int Id)
        {
            try
            {
                ContactsProcessing.DeleteGroup(Id, Convert.ToInt32(Session["UserId"]));
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message.ToString() });
            }
            return Json(new { status = true, message = "Successfully Deleted" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteContact(int Id, int groupId)
        {
            try
            {
                ContactsProcessing.DeleteContact(Id, Convert.ToInt32(Session["UserId"]), groupId);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message.ToString() });
            }
            return Json(new { status = true, message = "Successfully Deleted" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _AssignMultipleGroup()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult _AssignMultipleGroup(ContactsModel contactsModel)
        {
            ModelState.Remove("numbers");
            if (!ModelState.IsValid) return Json(new { status = false, message = "Fields are empty" });
            try
            {
                contactsModel.user_id = Convert.ToInt32(Session["UserId"]);
                ContactsProcessing.AssignMultipleGroup(contactsModel, out string result, out string status);
                return Json(new { status = status, message = result });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message.ToString() });
            }
        }
        [ChildActionOnly]
        public ActionResult _ViewContacts()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult _ViewContacts(FormCollection collection)
        {
            try
            {
                var list = ContactsProcessing.ViewContacts(Convert.ToInt32(Session["UserId"]), Convert.ToInt32(collection["id"]));
                return Json(new { status = true, message = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            {
                return Json(new { status = true, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [ChildActionOnly]
        public ActionResult _AssignContacts()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult _ViewAssignContacts(FormCollection collection)
        {
            try
            {
                var list = ContactsProcessing.ViewAssignContacts(Convert.ToInt32(Session["UserId"]), Convert.ToInt32(collection["id"]), Convert.ToInt32(collection["contactid"]));
                return Json(new { status = true, message = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = true, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _AssignContacts(List<ContactsModel> model) 
        {
            try
            {
                foreach (var v in model)
                {
                    ContactsProcessing._AssignContacts(v.id, v.groupid, Convert.ToInt32(Session["UserId"]));
                }
            }
            catch (Exception ex) 
            {
                return Json(new { status = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status=true, message="Assigned Successfully"}, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Download(string fullname, string email)
        {
            ViewBag.fullname = fullname;
            ViewBag.email = email;

            fullname = fullname == "" ? null : fullname;
            email = email == "" ? null : email;
            
            try
            {
                var list = ContactsProcessing.DownloadContacts(Convert.ToInt32(Session["UserId"]), fullname, email);

                DataTable dt = new DataTable();
                ContactsExtension.makeData(out dt, null);
                string fileName = "contacts.xlsx";

                foreach (var v in list)
                {
                    DataRow dr = dt.NewRow();

                    dr["id"] = v.id;
                    dr["user_id"] = v.user_id;
                    dr["fullname"] = v.fullname;
                    dr["numbers"] = v.numbers;
                    dr["emails"] = v.emails;
                    dr["groupname"] = v.groupname;
                    dr["option1"] = v.option1;
                    dr["option2"] = v.option2;
                    dr["option3"] = v.option3;
                    dr["crtime"] = v.crtime;
                    dt.Rows.Add(dr);
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("id,user id,fullname,numbers,emails,groupname,option1,option2,option3,crtime,\r\n");
                foreach (var v in list)
                {
                    sb.Append(v.id + "," + v.user_id + "," + v.fullname + "," + v.numbers + "," + v.emails + "," + v.groupname + "," + v.option1 + "," + v.option2 + "," + v.option2 + "," + v.crtime + ",\r\n");
                }
                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "contacts.csv");

                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    wb.Worksheets.Add(dt);
                //    using (MemoryStream stream = new MemoryStream())
                //    {
                //        wb.SaveAs(stream);
                //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                //    }
                //}
            }
            catch (Exception ex) 
            {
                return RedirectToAction("ManageContacts", "Contacts");
            }
        }
        [HttpPost]
        public ActionResult ImportContacts() 
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];

                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);
                    string path = Path.Combine(Server.MapPath("~/UploadFiles/"), fileName);

                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        file.SaveAs(path);

                        if (ContactsExtension.getContactsFromExcel(path, Convert.ToInt32(Session["UserId"]), out DataTable dt))
                        {
                            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString))
                            {
                                using (SqlBulkCopy sqlbulk = new SqlBulkCopy(con))
                                {
                                    sqlbulk.DestinationTableName = "contacts";
                                    sqlbulk.ColumnMappings.Add("user_id", "user_id");
                                    sqlbulk.ColumnMappings.Add("emails", "emails");
                                    sqlbulk.ColumnMappings.Add("fullname", "fullname");
                                    sqlbulk.ColumnMappings.Add("numbers", "numbers");
                                    sqlbulk.ColumnMappings.Add("option1", "option1");
                                    sqlbulk.ColumnMappings.Add("option2", "option2");
                                    sqlbulk.ColumnMappings.Add("option3", "option3");
                                    sqlbulk.ColumnMappings.Add("crtime", "crtime");

                                    con.Open();
                                    sqlbulk.WriteToServer(dt);
                                    sqlbulk.BatchSize = 5000;
                                    sqlbulk.Close();
                                    con.Close();

                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                }
                            }
                        }
                        else 
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            TempData["result"] = "Invalid or Curropt File";
                            TempData["status"] = "danger";
                            return RedirectToAction("ManageContacts", "Contacts");
                        }
                    }
                    else if (extension == ".csv") 
                    {
                        file.SaveAs(path);

                        if (ContactsExtension.getContactsFromCSV(path, Convert.ToInt32(Session["UserId"]), out DataTable dt))
                        {
                            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString))
                            {
                                using (SqlBulkCopy sqlbulk = new SqlBulkCopy(con))
                                {
                                    sqlbulk.DestinationTableName = "contacts";
                                    sqlbulk.ColumnMappings.Add("user_id", "user_id");
                                    sqlbulk.ColumnMappings.Add("emails", "emails");
                                    sqlbulk.ColumnMappings.Add("fullname", "fullname");
                                    sqlbulk.ColumnMappings.Add("numbers", "numbers");
                                    sqlbulk.ColumnMappings.Add("option1", "option1");
                                    sqlbulk.ColumnMappings.Add("option2", "option2");
                                    sqlbulk.ColumnMappings.Add("option3", "option3");
                                    sqlbulk.ColumnMappings.Add("crtime", "crtime");

                                    con.Open();
                                    sqlbulk.WriteToServer(dt);
                                    sqlbulk.BatchSize = 5000;
                                    sqlbulk.Close();
                                    con.Close();

                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            TempData["result"] = "Invalid or Curropt File";
                            TempData["status"] = "danger";
                            return RedirectToAction("ManageContacts", "Contacts");
                        }
                    }
                    else
                    {
                        TempData["result"] = "Please Upload csv, xls or xlsx file only";
                        TempData["status"] = "danger";
                        return RedirectToAction("ManageContacts", "Contacts");
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["result"] = ex.Message.ToString();
                TempData["status"] = "danger";
                return RedirectToAction("ManageContacts", "Contacts");
            }
            TempData["result"] = "Successfully Uploaded";
            TempData["status"] = "info";
            return RedirectToAction("ManageContacts", "Contacts");
        }
        [HttpPost]
        public JsonResult GetUserContacts() 
        {
            try
            {
                var list = ContactsProcessing.GetUserContacts(Convert.ToInt32(Session["UserId"]));
                return Json(new { status = true, message = list });
            }
            catch (Exception ex) 
            {
                return Json(new { status = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
