using System.Collections.Generic;
using BusinessLogic.Enums;

namespace BusinessLogic.Models
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public int CurrentPageNumber { get; set; }
        public int NumberOfPages { get; set; }
        public SortOrder SortOrder { get; set; }

        public class Order
        {
            public int Id { get; set; }
            public string ClientName { get; set; }
            public string DeviceBrand { get; set; }
            public string DeviceModel { get; set; }
            public TaskProgressDto CurrentState { get; set; }
            public string ClientPhone { get; set; }
            public string ClientEmail { get; set; }
        }
    }
}
