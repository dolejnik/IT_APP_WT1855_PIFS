using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class PartDto
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
    }
}
