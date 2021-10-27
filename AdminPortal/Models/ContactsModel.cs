using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class ContactsModel
    {
        [Required]
        public int id { get; set; }
        public int user_id { get; set; }
        public string fullname { get; set; }
        [Required]
        public string numbers { get; set; }
        public string emails { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public DateTime crtime { get; set; }
        public int groupid { get; set; }
        [Display(Name = "Group Name")]
        public string groupname { get; set; }
    }
}