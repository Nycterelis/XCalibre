using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCalibre.Models
{
    public class UserPersonalViewModel
    {
        public ApplicationUser User { get; set; }
        public Project Project { get; set; }
        public Ticket Ticket { get; set; }
        public TicketComment Comment { get; set; }

    }
}