using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCalibre.Models;
using XCalibre.Models.Helpers;

namespace XCalibre.Controllers
{
    public class AdminProjectViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminProjectView
        //public ActionResult Index()
        //{
        //    ProjectHelper helper = new ProjectHelper();
        //    var projectModelList = new List<AdminProjectViewModel>();

        //    IList<string> selected = new List<string>();

        //    foreach (var p in )
        //        return View();
        //}

        //GET EditProjectUser
        public ActionResult EditProjectUser(int id)
        {
            var prj = db.Projects.Find(id);
            AdminProjectViewModel adminProject = new AdminProjectViewModel();
            ProjectHelper helper = new ProjectHelper();
            var selected = db.Users.ToList();
            adminProject.Projects = new Project();
            adminProject.User = new MultiSelectList(db.Users, "FullName", "FullName", selected);
            adminProject.Projects.Id = prj.Id;
            adminProject.Name = prj.Name;

            return View(adminProject);
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         //[Authorize(Roles = "Admin, ProjectManager"]
         public ActionResult EditProjectUser([Bind(Include ="User, Projects, SelectedUsers")]AdminProjectViewModel model)
        {
            var prjId = model.Projects.Id;
            ProjectHelper helper = new ProjectHelper();
            foreach (var userRemove in db.Users.Select(u => u.FullName).ToList())
            {
                helper.RemoveUserToProject(userRemove, prjId);
            }
            foreach (var userAdd in model.SelectedUsers)
            {
                helper.AddUserToProject(userAdd, prjId);
            }
            return RedirectToAction("Index", "Projects");
        }

        //GET: Edit the Project Manager
        public ActionResult EditProjectManager(int id)
        {
            AdminProjectViewModel adminProject = new AdminProjectViewModel();
            
            UserRoleHelper helper = new UserRoleHelper();
            var selected = helper.UsersInRole("ProjectManager").ToList();
            adminProject.Projects = new Project();
            adminProject.ProjectManager = new SelectList(selected, "FullName", "FullName", selected);
            
            return View(adminProject);
        }

        //POST: Edit Project Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult EditProjectManager([Bind(Include = "PmId, Projects, SelectedUser")]AdminProjectViewModel model)
        {
            var prjId = model.Projects.Id;
            var pm = model.Projects.PmId;
            ProjectHelper helper = new ProjectHelper();
            helper.AddPmToProject(model.SelectedUser, prjId);
            return RedirectToAction("Index", "Projects");
        }
    }

}