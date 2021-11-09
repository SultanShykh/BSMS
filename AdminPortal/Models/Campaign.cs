using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class Campaign
    {
        public int id { get; set; }
        [Required]
        public int user_id { get; set; }
        [Required]
        public string camp_name { get; set; }
        [Required]
        public string sender { get; set; }
        public string receiver { get; set; }
        [Required]
        public string msgdata { get; set; }
        public DateTime camp_time { get; set; }
        public DateTime camp_end_time { get; set; }
        [Required]
        public DateTime add_time { get; set; }
        public int status_camp { get; set; }
        public int camp_smstype { get; set; }
        [Required]
        public int total_sms { get; set; }
        public int sent_sms { get; set; }
    }
}