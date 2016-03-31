using System.Collections.Generic;

namespace BusinessLogic.Models
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }

        public class Order
        {
            public string ClientName { get; set; }
            public string DeviceBrand { get; set; }
            public string DeviceModel { get; set; }
        }
    }
}
