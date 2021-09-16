using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class Reseller
    {
        [Key]
        [Display(Name = "Reseller ID")]
        public int id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [MaxLength(15)]
        public string username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string pass { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public string userrole { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(13,ErrorMessage = "Phone number must be 11 to 13 digits long",MinimumLength = 11)]
        public string ph_no { get; set; }

        [Display(Name = "Domain")]
        public string domain { get; set; }

        [Display(Name = "Masking ID")]
        public int maskingID { get; set; }

        [Display(Name = "Image")]
        public string image { get; set; }
    }
}