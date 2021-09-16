using AdminPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AdminPortal.Codebase
{

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        string val;
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["UserId"] != null)
            {
                val = HttpContext.Current.Session["UserId"].ToString();
                if (!GetUserRights(val)) 
                {
                    HttpContext.Current.Response.Redirect("/Home/Index");
                }
                return GetUserRights(val);
            }
            HttpContext.Current.Response.Redirect("/");
            return false;
        }

        public bool GetUserRights(string userId)
        {
            string controllerName = "Home";
            string actionName = "Index";
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            RouteData rd = RouteTable.Routes.GetRouteData(context);
            bool flag = true;
            var listdata = new List<AuthorizeModel>();

            if (rd != null)
            {
                controllerName = rd.GetRequiredString("controller");
                actionName = rd.GetRequiredString("action");
            }

            CommonProcessing.SetPermissions("", userId.ToString(), out listdata);

            if(listdata.Count() > 0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
           
            foreach (var v in listdata)
            {
                if (v.Controller.ToLower() == controllerName.ToLower() && v.Action.ToLower().Contains(actionName.ToLower()) && v.AllowView == false)
                {
                    return flag = false;
                }
            }
            return flag;
        }
    }
}

   