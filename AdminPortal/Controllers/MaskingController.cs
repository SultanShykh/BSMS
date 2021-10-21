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
        public ActionResult AddMaskingRoutes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMaskingRoutes(FormCollection collection)
        {
            try
            {
                var result = MaskProcessing.AddMaskingRoute(Convert.ToString(collection["mask"]), Convert.ToInt32(collection["route_name_mobilink"]), Convert.ToInt32(collection["route_name_ufone"]), Convert.ToInt32(collection["route_name_zong"]), Convert.ToInt32(collection["route_name_telenor"]), collection["route_name_SCOM"]);
                ViewBag.result = result;
                ViewBag.status = "info";

            }
            catch (Exception ex) 
            {
                ViewBag.result = "Error while adding";
                ViewBag.status = "danger";
            }
            return View();
        }
        public ActionResult EditMaskingRoute(string mask = null)
        {
            var routes = MaskProcessing.viewRoutes(mask);
            ViewBag.Mobilink = "";
            ViewBag.Ufone = "";
            ViewBag.Zong = "";
            ViewBag.Telenor = "";
            ViewBag.Scom = "";

            foreach (var v in routes)
            {
                if (v.operator1.ToLower().Contains("warid") || v.operator1.ToLower().Contains("mobilink"))
                {
                    ViewBag.Mobilink = v.route;
                }
                if (v.operator1.ToLower().Contains("ufone"))
                {
                    ViewBag.Ufone = v.route;
                }
                if (v.operator1.ToLower().Contains("zong"))
                {
                    ViewBag.Zong = v.route;
                }
                if (v.operator1.ToLower().Contains("telenor"))
                {
                    ViewBag.Telenor = v.route;
                }
                if (v.operator1.ToLower().Contains("scom"))
                {
                    ViewBag.Scom = v.route.ToString();
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditMaskingRoute(FormCollection collection)
        {
            try
            {
                var result = MaskProcessing.UpdateMaskingRoute(Convert.ToString(collection["masking_hidden"]), Convert.ToInt32(collection["route_name_mobilink"]), Convert.ToInt32(collection["route_name_ufone"]), Convert.ToInt32(collection["route_name_zong"]), Convert.ToInt32(collection["route_name_telenor"]), collection["route_name_SCOM"]);
                ViewBag.result = result;
                ViewBag.status = "info";

            }
            catch (Exception ex)
            {
                ViewBag.result = "Error while adding";
                ViewBag.status = "danger";
            }
            return View();
        }
        public ActionResult MaskingApproval()
        {
            return View();
        }
    }
}