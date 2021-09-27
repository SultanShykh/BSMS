using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class Outbox
    {
        public int id { get; set; }
        public int camp_id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        public string sender { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Receiver")]
        public string receiver { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string msgdata { get; set; }

        [Required]
        public DateTime senttime { get; set; }
        public DateTime deliveredtonetwork { get; set; }
        public DateTime deliveredtohandset { get; set; }
        public bool status { get; set; }
        public int route { get; set; }
        public int cost { get; set; }
        public int smstype { get; set; }
        public int api_id { get; set; }
        public int resent { get; set; }
        public string _operator { get; set;}
        public bool isswallow { get;set; }
        public bool isotpallow { get;set; }
        public string RemoteIP { get;set; }
        public DateTime CurrentDateTime { get; set; }
    }
}