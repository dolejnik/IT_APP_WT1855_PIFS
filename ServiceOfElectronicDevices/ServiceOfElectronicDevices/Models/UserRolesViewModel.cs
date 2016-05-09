using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceOfElectronicDevices.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Users { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }

        public IEnumerable<SelectListItem> ComponentsTypes { get; set; } 
    }
}