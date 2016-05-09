using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class TaskProgressViewModel
    {
        public TaskProgressDto Task { get; set; }
        public IEnumerable<PartDto> ComponentsList { get; set; }
    }
}
