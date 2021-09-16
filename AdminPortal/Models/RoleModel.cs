using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowView { get; set; }

        public bool AllowDelete { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }

    }
}