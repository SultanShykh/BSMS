using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPortal
{
    public static class HtmlUtility
    {
        public static string IsActive(this HtmlHelper html, string action, string controller) 
        {
            var routeData = html.ViewContext.RouteData;
            var info = "";
            bool isActive;

            if (action == "")
            {
                info = (string)(routeData.Values["controller"]);
                isActive = controller == info;
                return isActive ? "active" : "";
            }
            else
            {
                var routeAction = (string)routeData.Values["action"];
                var routeController = (string)routeData.Values["controller"];
                isActive = controller == routeController && action == routeAction;
                return isActive ? "active" : "";
            }
        }
    }
}