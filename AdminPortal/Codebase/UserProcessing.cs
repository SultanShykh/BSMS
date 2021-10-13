using AdminPortal.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace AdminPortal.Codebase
{
    public class UserProcessing
    {
        static dynamic AppDB = Database.OpenNamedConnection("MainDB");
        
        public static void GetUsers(int pageNum, string username, string email, string user_mobile, out List<UserModel> userModel, out List<Pagination> pagination, int userId) 
        {
            userModel = new List<UserModel>();

            dynamic Records = AppDB.COR_USP_GetUserWithGroups(currentPage: pageNum, username: username,email: email, mobile_number: user_mobile, userId: userId);

            if (Records.FirstOrDefault() != null)
            {
                userModel = Records.ToList<UserModel>();
            }
            Records.NextResult();
            pagination = Records.ToList<Pagination>();
        }
        public static void CheckUser(string username, out bool result)
        {
            result = false;

            dynamic Records = AppDB.COR_USP_CheckUser( Username: username);
            if (Records.FirstOrDefault() != null && Records.FirstOrDefault().Message == "NOT EXIST")
            {
                result = true;
            }
            
        }

        public static void CreateUser(UserModel model, out bool result, out string msg)
        {
            result = false;
            msg = null;

            var masking = "";
            foreach (var maskings in model.masking)
            {
                masking += maskings + ",";
            }

            dynamic Records = AppDB.COR_USP_CreateUser(fullname: model.fullname,Username: model.username, Password: model.password, Email: model.email,ExpiryDateTime: model.user_expiry, userGroupId: model.GroupId,status: Convert.ToInt32(model.status), mobile: model.user_mobile,masking: masking, parentaccid: model.parentAccountId, otp_access: Convert.ToInt32(model.otp_access) );
            if (Records.FirstOrDefault() != null && Records.FirstOrDefault().Message == "User Created")
            {
                result = true;
            }
            else if (Records.FirstOrDefault() != null && Records.FirstOrDefault().Message == "Username Already Taken")
            {
                result = true;
            }
            msg = Records.FirstOrDefault().Message;


        }
        
        public static void UpdateUser(UserModel model, out bool result)
        {
            result = false;
            var masking = "";
            foreach (var maskings in model.masking) 
            {
                masking += maskings + ",";
            }
            dynamic Records = AppDB.COR_USP_UpdateUser(userId:model.Id, fullname: model.fullname, Email: model.email, mobile_number: model.user_mobile,masking: masking,groupId: model.GroupId, password: model.password, otp_access: model.otp_access, expiry: model.user_expiry, status: model.status );

            if (Records.FirstOrDefault() != null && Records.FirstOrDefault().Message == "User Updated")
            {
                result = true;
            }
        }

        public static void SelectedUserMaskings(int id, out string selectedMaskings,out List<Masking> masking) 
        {
            selectedMaskings = "";
            var Records = AppDB.COR_USP_SelectedUserMaskings(id);

            if (Records.FirstOrDefault() != null)
            {
                foreach (var rec in Records) 
                {
                    selectedMaskings += rec.Id + ",";
                }
            }

            Records.NextResult();

            masking = Records.ToList<Masking>();
            selectedMaskings = selectedMaskings.TrimEnd(',');
        }

        public static List<Masking> SelectedMaskings()
        {
            List<Masking> masking = AppDB.COR_USP_SelectedUserMaskings(Convert.ToInt32(HttpContext.Current.Session["UserId"])).ToList<Masking>();
            return masking;
        }
    }
}