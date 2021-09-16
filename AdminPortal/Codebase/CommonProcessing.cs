using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple.Data;
using AdminPortal.Models;
using System.Dynamic;
using System.Text;
using System.Net;

namespace AdminPortal.Codebase
{
    public class CommonProcessing
    {
        static dynamic AppDB = Database.OpenNamedConnection("MainDB");
        
        public static void GetUserByGroupId(out string selectedUsers, out List<UserModel> users, int? groupId = null)
        {
            users = new List<UserModel>();
            selectedUsers = "";

            dynamic Records = AppDB.COR_USP_GetUsersByGroupId(groupId: groupId);

            if (Records.FirstOrDefault() != null)
            {
                users = Records.ToList<UserModel>();
            }

            Records.NextResult();
            if (Records.FirstOrDefault() != null)
            {
                foreach (var rec in Records)
                {
                    selectedUsers += rec.Id+ ",";
                }
            }
            selectedUsers = selectedUsers.TrimEnd(',');
        }
        
        public static void GetAllGroups(out List<GroupMasterModel> groups)
        {
            groups = AppDB.COR_USP_GetAllGroups() ?? new List<GroupMasterModel>();
        }

        
        public static void GetGroupWithPermissionsTree(out List<AuthorizeModel> groups)
        {
            groups = AppDB.COR_USP_GetAllGroupPermissions() ?? new List<AuthorizeModel>();
        }

        public static void SetPermissions(dynamic Records, string userid, out List<AuthorizeModel> groups)
        {
            groups = AppDB.COR_WEB_UserPermissions(Id: userid) ?? new List<AuthorizeModel>();
        }
        public static void GetGroupByIdWithPermissionsTree(out List<dynamic> groups, string GroupId = null)
        {
            groups = new List<dynamic>();

            dynamic Result = AppDB.COR_USP_GetGroupPermissionsByGroupId(groupId: GroupId);
            var Records = Result.FirstOrDefault();

            if (Records != null)
            {
                foreach (var rec in Result)
                {
                    groups.Add(new GroupMasterModel()
                    {
                        id = rec.Id,
                        Name = rec.Name,
                        Status = rec.Status
                    });
                }
            }
            
            Result.NextResult();
            if(Result.FirstOrDefault() != null)
            {
                foreach (var rec in Result)
                {
                    groups.Add(new
                    {
                        FormMasterId = rec.FormMasterId,
                        FormMasterName = rec.FormMasterName,
                        MenuName = rec.MenuName,
                        MenuId = rec.MenuId,
                        MenuParentId = rec.MenuParentId,
                        MenuUrl = rec.MenuUrl,
                        Controller = rec.Controller,
                        Action = rec.Action,
                        PermissionId = rec.PermissionId,
                        AllowCreate = rec.AllowCreate,
                        AllowDelete = rec.AllowDelete,
                        AllowEdit = rec.AllowEdit,
                        AllowView = rec.AllowView,
                    });
                }
            }
        }

        public dynamic GetMaskings() 
        {
            List<Masking> m = AppDB.GetMaskings() ?? new List<Masking>();
            return m;
        }
    }
}