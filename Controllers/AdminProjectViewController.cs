using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCalibre.Models;
using XCalibre.Models.Helpers;

namespace XCalibre.Controllers
{
    [RequireHttps]
    public class AdminProjectViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET EditProjectUser
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult EditProjectUser(int id, string initialUrl)
        {
            ViewBag.SaveUrl = initialUrl;
            var prj = db.Projects.Find(id);
            AdminProjectViewModel adminProject = new AdminProjectViewModel();
            ProjectHelper helper = new ProjectHelper();
            UserRoleHelper roleHelper = new UserRoleHelper();
            var selected = roleHelper.UsersInRole("Developer").ToList();
            //var selected = db.Users.ToList();
            adminProject.Projects = new Project();
            adminProject.User = new MultiSelectList(selected, "Id", "FullName", selected);
            adminProject.Projects.Id = prj.Id;
            adminProject.Name = prj.Name;

            return View(adminProject);
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         [Authorize(Roles = "Admin, ProjectManager")]
         public ActionResult EditProjectUser([Bind(Include ="Id, Name, User, Projects, SelectedUsers, TempUrl")]AdminProjectViewModel model)
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
            //return Redirect(Request.UrlReferrer.ToString());
            //return RedirectToRoute(model.TempUrl);
            return RedirectToAction("Index", "Projects");
        }

        //GET: Edit the Project Manager
        [Authorize(Roles = "Admin")]
        public ActionResult EditProjectManager(int id)
        {
            AdminProjectViewModel adminProject = new AdminProjectViewModel();
            var prj = db.Projects.Find(id);
            UserRoleHelper helper = new UserRoleHelper();
            var selected = helper.UsersInRole("ProjectManager").ToList();
            adminProject.Projects = new Project();
            adminProject.ProjectManager = new SelectList(selected, "Id", "FullName", selected);
            adminProject.Projects.Id = prj.Id;
            adminProject.Name = prj.Name;

            return View(adminProject);
        }

        //POST: Edit Project Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditProjectManager([Bind(Include = "PmId, Projects, SelectedUser, Id, Name")]AdminProjectViewModel model)
        {
            var prjId = model.Projects.Id;
            var pm = model.Projects.PmId;
            ProjectHelper helper = new ProjectHelper();
            helper.AddPmToProject(model.SelectedUser, prjId);
            return RedirectToAction("Index", "Projects");
        }
    }

}