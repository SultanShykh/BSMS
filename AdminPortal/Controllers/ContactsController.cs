using AdminPortal.Codebase;
using AdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPortal.Controllers
{
    public class ContactsController : Controller
    {
        public ActionResult ManageContacts(int Id=1)
        {
            ContactsProcessing.GetContacts(Id, out List<ContactsModel> contacts, out int totalPages);
            ViewBag.pageNumber = Id;
            ViewBag.totalPages = totalPages;
            return View(contacts);
        }
        public ActionResult ManageGroups(int Id=1)
        {
            ContactsProcessing.GetGroups(Id, out List<dynamic> contacts, out int totalPages);
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
                ContactsProcessing.DeleteGroup(Id);
            }
            catch (Exception ex) 
            {
                return Json(new { status = false, message = ex.Message.ToString() });
            }
            return Json(new { status = true, message = "Successfully Deleted" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteContact(int Id)
        {
            try
            {
                ContactsProcessing.DeleteContact(Id);
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
    }
}