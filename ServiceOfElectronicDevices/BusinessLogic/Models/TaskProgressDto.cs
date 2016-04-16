using System;

namespace BusinessLogic.Models
{
    public class TaskProgressDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public double Price { get; set; }
    }
}
