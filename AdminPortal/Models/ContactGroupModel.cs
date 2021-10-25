using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class ContactGroupModel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        [Display(Name = "Group Name")]
        public string groupname { get; set; }
        public string description { get; set; }
        public DateTime crtime { get; set; }
    }
}