using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class ActivityLog
    {
        public int id { get; set; }
        public string username { get; set; }
        public string ip { get; set; }
        public string activity { get; set; }
        public DateTime act_time { get; set; }
    }
}