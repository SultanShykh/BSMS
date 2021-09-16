using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class GroupMasterModel
    {
        public int id { get; set; }
        public int applicationId { get; set; }
        public string Name { get; set; }
        public string createdBy { get; set; }
        public string isActive { get;set; }
        public int Status { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime CreatedDateTime { get; set; }
     
    }

    public class GroupMenuModel
    {
        public int GroupId { get; set; }
        public int MenuId { get; set; }
        public bool AllowView { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowCreate { get; set; }

    }
}