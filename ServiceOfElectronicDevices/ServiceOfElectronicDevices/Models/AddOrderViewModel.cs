using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Models;

namespace ServiceOfElectronicDevices.Models
{
    public class AddOrderViewModel
    {
        public string UserId { get; set; }
        public int DeviceId { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Users { get; set; }
        public IEnumerable<SelectListItem> Devices { get; set; }

    }
}