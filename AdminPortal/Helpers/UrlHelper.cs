using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPortal.Helpers
{
    public static class UrlHelper
    {
        public static string actionName(this HtmlHelper html) 
        {
            var route = html.ViewContext.RouteData;
            var action = (string)route.Values["action"];
            return action;
        }

        public static string controllerName(this HtmlHelper html)
        {
            var route = html.ViewContext.RouteData;
            var controller = (string)route.Values["controller"];
            return controller;
        }
    }
}