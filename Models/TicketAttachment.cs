using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCalibre.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserId { get; set; }
        public string FilreUrl { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}