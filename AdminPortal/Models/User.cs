using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class User
    {
        [Key]
        [Display(Name = "User ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string username { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string email { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public int GroupId { get; set; }

        [Display(Name = "Image")]
        public string image { get; set; }
    }

}