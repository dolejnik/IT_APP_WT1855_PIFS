using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Models;

namespace ServiceOfElectronicDevices.Models
{
    public class AddTaskViewModel : TaskProgressDto
    {
        public IEnumerable<SelectListItem> ComponentsTypes { get; set; }
        public int ComponentType { get; set; }
        public IEnumerable<PartDto> Components { get; set; }
        public IEnumerable<PartDto> Selected { get; set; }
        public Posted Posted { get; set; }
    }

    public class Posted
    {
        public int[] ComponentsIds { get; set; }
    }
}