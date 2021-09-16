using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Models
{

    public class MenuModel
    {
        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }

        [Required]
        public string MenuName { get; set; }

        [Required]
        public string MenuUrl { get; set; }

        [Required]
        public string Controller { get; set; }

        [Required]
        public string Action { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public string IconClass { get; set; }

        public string MenuTree { get; set; }
        public IEnumerable<MenuModel> List { get; set; }
    }
}