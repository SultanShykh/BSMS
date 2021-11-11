using AdminPortal.Codebase;
using System;
using System.Collections.Generic;
using System.IO;
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
            return View(MaskProcessing.MaskingRequests());
        }
        public ActionResult MaskingRequest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MaskingRequest(FormCollection collection)
        {
            HttpFileCollectionBase files = Request.Files;
            if (!string.IsNullOrEmpty(collection["masking"]) && !string.IsNullOrEmpty(collection["fullname"]) && !string.IsNullOrEmpty(collection["nic"]) && !string.IsNullOrEmpty(collection["contact"]) && files.Count != 0)
            {
                try
                {
                    HttpPostedFileBase file = files[0];
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);
                    string path = Path.Combine(Server.MapPath("~/Media/"), fileName);

                    if (extension != ".jpeg") 
                    {
                        ViewBag.result = "Please Attach JPEG File Only";
                        ViewBag.status = "danger";
                        return View();
                    }
                    file.SaveAs(path);
                    string message = MaskProcessing.AddMaskingRequest(collection["masking"], Convert.ToInt32(Session["UserId"]),collection["fullname"], collection["nic"], collection["contact"], fileName);
                    ViewBag.result = message;
                    ViewBag.status = "dark";
                }
                catch (Exception ex)
                {
                    ViewBag.result = ex.ToString();
                    ViewBag.status = "danger";
                }
            }
            else 
            {
                ViewBag.result = "Please Fill out the Form";
                @ViewBag.status = "danger";
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetMasks(int Id) 
        {
            var masks = UserProcessing.SelectedMaskings(Id);
            return Json(new { status = true, message = masks}, JsonRequestBehavior.AllowGet);
        }
    }
}