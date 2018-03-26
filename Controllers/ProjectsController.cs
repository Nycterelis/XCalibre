using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XCalibre.Models;
using XCalibre.Models.Helpers;

namespace XCalibre.Controllers
{
    [RequireHttps]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        [Authorize]
        public ActionResult Index()
        {
            List<ProjectViewModel> vm = new List<ProjectViewModel>();
            foreach(var proj in db.Projects.Where(c => c.Closed == false).ToList())
            {
                ProjectViewModel viewmodel = new ProjectViewModel();
                viewmodel.Project = proj;
                viewmodel.ProjectManager = db.Users.Find(proj.PmId);
                vm.Add(viewmodel);
            }

            
            
            return View(vm);
        }

        // GET: All Projects
        [Authorize]
        public ActionResult AllProjects()
        {
            List<ProjectViewModel> vm = new List<ProjectViewModel>();
            var projects = db.Projects.Where(c => c.Closed == true).ToList().OrderBy(n => n.Name);
            var openProjects = db.Projects.Where(c => c.Closed == false).ToList().OrderBy(n => n.Name);
            var allProjects = openProjects.Union(projects);
            //foreach (var proj in db.Projects.ToList().OrderBy(c => c.Closed).OrderBy(n => n.Name))
            foreach (var proj in allProjects)
            {
                ProjectViewModel viewmodel = new ProjectViewModel();
                viewmodel.Project = proj;
                viewmodel.ProjectManager = db.Users.Find(proj.PmId);
                vm.Add(viewmodel);
            }



            return View(vm);
        }

        // GET: Closed Projects
        [Authorize]
        public ActionResult ClosedProjects()
        {
            List<ProjectViewModel> vm = new List<ProjectViewModel>();
            foreach (var proj in db.Projects.Where(c => c.Closed == true).ToList())
            {
                ProjectViewModel viewmodel = new ProjectViewModel();
                viewmodel.Project = proj;
                viewmodel.ProjectManager = db.Users.Find(proj.PmId);
                vm.Add(viewmodel);
            }



            return View(vm);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,User,Projects, SelectedUsers")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            //db.Projects.Remove(project);
            project.Closed = true;
            var proj = project.Tickets;
            foreach (var tick in proj)
            {
                tick.Closed = true;
            }
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
