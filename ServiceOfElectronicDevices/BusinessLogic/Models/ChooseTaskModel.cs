using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ChooseTaskModel
    {
        public int OrderId { get; set; }
        public int TaskId { get; set; }
        public int ComponentId { get; set; }
        public string Token { get; set; }
    }
}
