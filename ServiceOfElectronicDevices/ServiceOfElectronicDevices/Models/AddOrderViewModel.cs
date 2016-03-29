using System.Collections.Generic;
using System.Web.Mvc;

namespace ServiceOfElectronicDevices.Models
{
    public class AddOrderViewModel
    {
        public string UserId { get; set; }
        public int DeviceId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
        public IEnumerable<SelectListItem> Devices { get; set; }
    }
}