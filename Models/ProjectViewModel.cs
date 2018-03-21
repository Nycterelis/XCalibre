using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCalibre.Models
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public ApplicationUser ProjectManager { get; set; }
    }
}