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
        [ChildActionOnly]
        public ActionResult _AddContact()
        {
            return PartialView(new ContactsModel());
        }
        [HttpPost]
        public ActionResult _AddContact(ContactsModel contactsModel)
        {
            ModelState.Remove("id");
            return Json(new { status = true, message = "Successfully Added"});
        }
        [ChildActionOnly]
        public ActionResult _UpdateContact()
        {
            return PartialView(new ContactsModel());
        }
        [HttpPost]
        public ActionResult _UpdateContact(ContactsModel contactsModel)
        {
            return Json(new { status = true, message = "" });
        }
    }
}