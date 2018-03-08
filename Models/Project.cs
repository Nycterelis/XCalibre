using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCalibre.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PmId { get; set; }


        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}