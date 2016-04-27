using System.Collections.Generic;

namespace BusinessLogic.Models
{
    public class OrderDetailsViewModel
    {
        public OrderViewModel.Order Order { get; set; }
        public IEnumerable<TaskProgressDto> Tasks { get; set; }
        public string Description { get; set; }
    }
}