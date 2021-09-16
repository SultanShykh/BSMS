using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class Pagination
    {
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
    }
}