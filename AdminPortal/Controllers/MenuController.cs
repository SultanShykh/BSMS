
using AdminPortal.Codebase;
using AdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPortal.Controllers
{
    [AuthorizeUser]
    public class MenuController : Controller
    {
        public ActionResult Index(int Id = 1, FormCollection collection = null)
        {
            List<MenuModel> result = new List<MenuModel>();
            List<Pagination> pagination = new List<Pagination>();

            ViewBag.pageNumber = Id;
            ViewBag.menu_name = collection["menu_name"];
            ViewBag.menu_url = collection["menu_url"];

            try
            {
                MenuProcessing.GetMenu(Id, collection["menu_name"], collection["menu_url"], out result, out pagination);
                
                return View(Tuple.Create(result,pagination));
            }
            catch (Exception ex)
            {
                return View(Tuple.Create(result, pagination));
            }
        }

        [ChildActionOnly]
        public ActionResult _CreateMenu()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateMenu(MenuModel menuModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid) 
                return Json(new { status = false, message = "Fields are empty" });
            try
            {
                bool result;
                MenuProcessing.CreateMenu(menuModel, out result);

                return Json(new { status = result, message = "Successfully Created!!!" });
            }
            catch (Exception ex)
            {
                string msg = "Error, Please try again.";
                return Json(new { status = false, message = msg });
            }
        }

        [HttpPost]
        public ActionResult DeleteMenu(string Id)
        {

            bool result = false;
            string msg = "";
            try
            {
                MenuProcessing.DeleteMenu(Id, out result, out msg);
                return Json(new { status = result, message = msg });

            }
            catch (Exception e)
            {
                return Json(new { status = result, message = msg });
            }
        }

        [HttpPost]
        public ActionResult UpdateMenu(MenuModel menuModel)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Fields are empty" });
            try
            {
                bool result;
                MenuProcessing.UpdateMenu(menuModel, out result);

                return Json(new { status = result, message = "Successfully Updated!!!" });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "Successfully Updated!!!" });
            }
        }
    }
}