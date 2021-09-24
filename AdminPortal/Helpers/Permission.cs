using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPortal.Models;
using AdminPortal.Codebase;

namespace AdminPortal.Helpers
{
    public static class Permission
    {
        public static bool HasPermission(string controller, string action) 
        {           
            bool flag = false;
            List<AuthorizeModel> listdata = new List<AuthorizeModel>();
            try
            {
                CommonProcessing.SetPermissions("", HttpContext.Current.Session["UserId"].ToString(), out listdata);
                if (listdata.Count() < 0)
                {
                    flag = false;
                }

                if (action == "")
                {
                    foreach (var i in listdata)
                    {
                        if (i.Controller == controller && i.AllowView == true)
                        {
                            flag = true;
                        }
                    }
                }
                else
                {
                    foreach (var i in listdata)
                    {
                        if (i.Controller == controller && i.Action == action && i.AllowView == true)
                        {
                            flag = true;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                HttpContext.Current.Response.Redirect("/Account/Login");
            }
            
            return flag;
        }
    }
}