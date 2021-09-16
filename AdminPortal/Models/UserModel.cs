using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class UserModel
    {
        //[Required]
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string fullname { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }

        [StringLength(12, ErrorMessage = "Do not enter more than 12 characters", MinimumLength = 10)]
        [Display(Name = "Mobile Number")]
        public string user_mobile { get; set; }

        [Display(Name = "Masking")]
        public string [] masking { get; set; }
        public int userType { get; set; }

        [Display(Name = "Created Date")]
        public DateTime createdDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiry Date")]
        public DateTime user_expiry { get; set; }

        [Display(Name = "Account Type")]
        public string acctype { get; set; }

        [Display(Name = "OTP Account Type")]
        public string otpacctype { get; set; }  
        public bool IsOtpAllow { get; set; }

        [Display(Name = "Status")]
        public bool status { get; set; }

        [Display(Name = "OTP Access")]
        public bool otp_access { get; set; }
        public string GroupName { get;set; }

        public int parentAccountId { get; set; }
        public int GroupId { get; set; }
        public List<GroupMasterModel> GroupMasters { get; set; }
    }
}
