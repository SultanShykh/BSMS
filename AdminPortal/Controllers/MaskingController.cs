using AdminPortal.Codebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPortal.Controllers
{
    public class MaskingController : Controller
    {
        CommonProcessing c = new CommonProcessing();
        public ActionResult MaskingRoute()
        {
            return View();
        }
        public ActionResult AddMaskingRoutes()
        {
            return View();
        }
        public ActionResult EditMaskingRoute()
        {
            return View();
        }
        public ActionResult MaskingApproval()
        {
            return View();
        }
    }
}