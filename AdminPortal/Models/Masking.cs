using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class Masking
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string masking { get; set; }

        public DateTime crtime { get; set; }
    }
}