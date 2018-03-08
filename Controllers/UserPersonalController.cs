using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCalibre.Models;

namespace XCalibre.Controllers
{
    public class UserPersonalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: My Projects
        public ActionResult MyProjects(string userId)
        {
            
            
            var userProjects = db.Users.Find(userId).Projects.Where(p => p.PmId == userId);

            return View(userProjects.ToList());
        }

        // GET: My Assigned Tickets
        public ActionResult MyAssignedTickets()
        {
            var userId = User.Identity.GetUserId();
            var userTickets = db.Tickets.Where(u => u.AssignedToUserId == userId).Include(t => t.AssignedToUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(userTickets.ToList());
        }

        //GET: My Submitted Tickets
        public ActionResult MySubmittedTIckets()
        {
            var userId = User.Identity.GetUserId();
            var userTickets = db.Tickets.Where(u => u.OwnerUserId == userId);
            return View(userTickets.ToList());
        }
            
        // GET: My Comments
        public ActionResult MyComments()
        {
            var userId = User.Identity.GetUserId();
            var userComments = db.TicketComments.Where(u => u.UserId == userId);

            return View(userComments.ToList());
        }
    }

}