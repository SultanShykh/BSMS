using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class SMSSummaryModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public int user_id { get; set; }
        public DateTime summary_date { get; set; }
        public string smstype { get; set; }
        public string sender { get; set; }
        public int operater_id { get; set; }
        public int smscount { get; set; }
        public int smscost { get; set; }
    }
}