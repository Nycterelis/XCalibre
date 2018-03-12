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
        //Gets projects that a user is assigned to 
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public ActionResult MyProjects()
        {
            var userId = User.Identity.GetUserId();
            var userProjects = db.Users.Find(userId).Projects.Where(p => p.PmId == userId).ToList();
            var assignedProjects = db.Users.Find(userId).Projects.ToList();

            return View(assignedProjects);
        }
        // GET: My Projects
        //Gets projects that a Project Manager is assigned to *Manage*
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult MyManagedProjects()
        {
            var userId = User.Identity.GetUserId();
            var userProjects = db.Users.Find(userId).Projects.Where(p => p.PmId == userId).ToList();

            return View(userProjects);
        }

        // GET: My Assigned Tickets
        [Authorize(Roles = "Developer")]
        public ActionResult MyAssignedTickets(string userId)
        {
            
            var userTickets = db.Tickets.Where(u => u.AssignedToUserId == userId).Include(t => t.AssignedToUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(userTickets.ToList());
        }

        //GET: My Submitted Tickets
        [Authorize(Roles = "Submitter, ProjectManager")]
        public ActionResult MySubmittedTIckets(string userId)
        {
           
            var userTickets = db.Tickets.Where(u => u.OwnerUserId == userId);
           
            return View(userTickets.ToList());
        }
            
        // GET: My Comments
        //public ActionResult MyComments()
        //{
        //    var userId = User.Identity.GetUserId();
        //    var userComments = db.TicketComments.Where(u => u.UserId == userId);

        //    return View(userComments.ToList());
        //}
    }

}