using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XCalibre.Models;
using XCalibre.Models.Helpers;

namespace XCalibre.Controllers
{
    [RequireHttps]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter, ProjectManager, Admin")]
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Submitter, ProjectManager")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Body,Created,ProjectId,Project,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;
                ticket.TicketStatusId = db.TicketStatuses.First(i => i.Id == 1).Id;
                ticket.TicketPriorityId = db.TicketPriorities.First(i => i.Id == 5).Id;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.AssignedToUserId = db.Users.FirstOrDefault(n => n.FirstName == "Unassigned").Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                //var thisTicket = db.Tickets.Where(i => i.Id == ticket.Id);
                var projectId = ticket.ProjectId;//The magic starts here
                var projectManager = db.Projects.Find(projectId).PmId;


                await UserManager.SendEmailAsync(projectManager, "New Ticket", "A new ticket has been submitted for one of your managed projects. Please login to view details.");
                return RedirectToAction("Index");
            }

            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            // ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            UserRoleHelper helper = new UserRoleHelper();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            ViewBag.AssignedToUserId = new SelectList(helper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Body,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(ticket).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            if (ModelState.IsValid)
            {
                //var model = db.Tickets.Find(ticket.Id);
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                foreach (var prop in typeof(Ticket).GetProperties())
                {
                    if (prop.Name != null && prop.Name.In("Title", "Body", "TicketTypeId", "TicketPriorityId", "TicketStatusId", "AssignedToUserId"))
                    {
                        var oldInt = oldTicket.GetType().GetProperty(prop.Name).GetValue(oldTicket);
                        var newInt = oldTicket.GetType().GetProperty(prop.Name).GetValue(ticket);
                        var oldValue = oldTicket.GetType().GetProperty(prop.Name).GetValue(oldTicket).ToString();
                        var newValue = ticket.GetType().GetProperty(prop.Name).GetValue(ticket).ToString();

                        if (prop.Name == "TicketTypeId")
                        {
                            oldValue = db.TicketTypes.Find(oldInt).Name;
                            newValue = db.TicketTypes.Find(newInt).Name;
                        }
                        if (prop.Name == "TicketStatusId")
                        {
                            oldValue = db.TicketStatuses.Find(oldInt).Name;
                            newValue = db.TicketStatuses.Find(newInt).Name;
                        }
                        if (prop.Name == "TicketPriorityId")
                        {
                            oldValue = db.TicketPriorities.Find(oldInt).Name;
                            newValue = db.TicketPriorities.Find(newInt).Name;
                        }
                        //if (!(ticket.GetType().GetField(prop.Name).GetValue(prop).Equals(model.GetType().GetProperty(prop.Name).GetValue(prop))
                        if (oldValue != newValue)
                        {
                            TicketHistory ticketHistory = new TicketHistory()
                            {
                                TicketId = oldTicket.Id,
                                UserId = User.Identity.GetUserId(),
                                Property = prop.Name,
                                OldValue = oldValue,
                                NewValue = newValue,
                                Changed = DateTime.Now
                            };

                            db.TicketHistories.Add(ticketHistory);
                            //If Block for Emails
                            if (prop.Name == "AssignedToUserId")
                            {
                                await UserManager.SendEmailAsync(oldValue, "Ticket", "You have been removed from" + ticket.Title + ".");
                                await UserManager.SendEmailAsync(newValue, "New Assignment", "You have been assigned to the ticket," + ticket.Title + ". Please login to view details.");
                            };
                            if (prop.Name == "TicketPriorityId")
                            {
                                await UserManager.SendEmailAsync(ticket.AssignedToUserId, "New Ticket Priority", "One of your tickets have changed in priority. Please login to view details");
                            };

                        }
                    }
                }
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { id = ticket.Id });
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }
        //public ActionResult EditUser

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
