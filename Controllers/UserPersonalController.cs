using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using XCalibre.Models;

namespace XCalibre.Controllers
{
    [RequireHttps]
    public class UserPersonalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: Index
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var assignedProjects = db.Users.Find(userId).Projects.ToList();
            var assignedTickets = db.Tickets.Where(u => u.AssignedToUserId == userId).ToList();
            var submittedTickets = db.Tickets.Where(u => u.OwnerUserId == userId).ToList();
            var managedProjects = db.Projects.Where(u => u.PmId == userId).ToList();
            List<UserPersonalViewModel> vm = new List<UserPersonalViewModel>();
            foreach (var proj in assignedProjects)
            {
                UserPersonalViewModel viewmodelProj = new UserPersonalViewModel()
                {
                    Project = proj

                };
                vm.Add(viewmodelProj);
            }
            foreach (var tick in assignedTickets)
            {
                UserPersonalViewModel viewmodelTick = new UserPersonalViewModel()
                {
                    AssignedTicket = tick

                };
                vm.Add(viewmodelTick);
            }
            foreach (var tick in submittedTickets)
            {
                UserPersonalViewModel viewmodelStick = new UserPersonalViewModel()
                {
                    SubmittedTicket = tick

                };
                vm.Add(viewmodelStick);
            }
            foreach (var proj in managedProjects)
            {
                UserPersonalViewModel viewmodelMProj = new UserPersonalViewModel()
                {
                    ManagedProject = proj

                };
                vm.Add(viewmodelMProj);
            }

            return View(vm);
        }

        // GET: My Projects
        //Gets projects that a user is assigned to 
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public ActionResult MyProjects()
        {
            var userId = User.Identity.GetUserId();
            //var userProjects = db.Users.Find(userId).Projects.Where(p => p.PmId == userId).ToList();
            var assignedProjects = db.Users.Find(userId).Projects.ToList();
            List<ProjectViewModel> vm = new List<ProjectViewModel>();
            foreach (var p in assignedProjects)
            {
                ProjectViewModel viewmodel = new ProjectViewModel()
                {
                    Project = p,
                    ProjectManager = db.Users.Find(p.PmId)
                };
                vm.Add(viewmodel);
            }

            return View(vm);
        }
        // GET: My Projects
        //Gets projects that a Project Manager is assigned to *Manage*
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult MyManagedProjects()
        {
            var userId = User.Identity.GetUserId();
            var userProjects = db.Projects.Where(p => p.PmId == userId).ToList();
            List<ProjectViewModel> vm = new List<ProjectViewModel>();
            foreach (var p in userProjects)
            {
                ProjectViewModel viewmodel = new ProjectViewModel()
                {
                    Project = p,
                    ProjectManager = db.Users.Find(p.PmId)
                };
                vm.Add(viewmodel);
            }

            return View(vm);
        }

        // GET: My Assigned Tickets
        [Authorize(Roles = "Developer")]
        public ActionResult MyAssignedTickets(string userId)
        {
            
            var userTickets = db.Tickets.Where(u => u.AssignedToUserId == userId).Include(t => t.AssignedToUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(userTickets.Where(t => t.Closed == false).ToList());
        }

        //GET: My Submitted Tickets
        [Authorize(Roles = "Submitter, ProjectManager")]
        public ActionResult MySubmittedTIckets(string userId)
        {
           
            var userTickets = db.Tickets.Where(u => u.OwnerUserId == userId);
           
            return View(userTickets.Where(t => t.Closed == false).ToList());
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