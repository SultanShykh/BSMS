using AdminPortal.AccountProcessing;
using AdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPortal.Controllers
{
    public class ResellerPartialsController : Controller
    {
        AccountProcessingController apc = new AccountProcessingController();
        public const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";

        [ChildActionOnly]
        public ActionResult EditForm()
        {
            var info = apc.SP_user_roles();
            ViewBag.role = new SelectList(info, "id", "role");
            return PartialView(PARTIAL_VIEW_FOLDER + "Edit_Form/Edit_Form.cshtml");
        }

        [ChildActionOnly]
        public ActionResult CreateForm()
        {
            var info = apc.SP_user_roles();
            ViewBag.role = new SelectList(info, "id", "role");
            return PartialView(PARTIAL_VIEW_FOLDER + "Create_Form/Create_Form.cshtml");
        }
    }
}