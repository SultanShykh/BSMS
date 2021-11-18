using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class Inbox
    {
        public int id { get; set; }
        public string prefix { get; set; }
        public int user_id { get; set; }
        public string sender { get; set; }
        public string reciever { get; set; }
        public int msgdata { get; set; }
        public DateTime receivedtime { get; set; }
        public string _operator { get; set; }
    }
}