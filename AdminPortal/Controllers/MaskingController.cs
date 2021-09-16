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
        // GET: Masking
        public ActionResult Index()
        {
            return View();
        }


    }
}