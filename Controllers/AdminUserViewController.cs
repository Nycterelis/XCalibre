using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using XCalibre.Models;
using XCalibre.Models.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using PagedList.Mvc;


namespace XCalibre.Controllers
{
    [AllowAnonymous]
    public class AdminUserViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //GET: AdminUser

        public ActionResult Index()
        {
            UserRoleHelper helper = new UserRoleHelper();
            var adminModelList = new List<AdminUserViewModel>();


            IList<string> selected = new List<string>();

            foreach (var u in db.Users)
            {
                AdminUserViewModel adminModel = new AdminUserViewModel();
                adminModel.SelectedRoles = helper.ListUserRoles(u.Id).ToArray();
                adminModel.User = u;

                adminModelList.Add(adminModel);
            }
            return View(adminModelList);
        }

        //GET: EditUser
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
        {
            var user = db.Users.Find(id);
            AdminUserViewModel adminModel = new AdminUserViewModel();
            UserRoleHelper helper = new UserRoleHelper();
            var selected = helper.ListUserRoles(id);
            adminModel.Roles = new MultiSelectList(db.Roles, "Name", "Name", selected);
            adminModel.User = new ApplicationUser();
            adminModel.User = user; 
            
            return View(adminModel);

        }

        //POST: EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser([Bind(Include = "User, Roles, SelectedRoles")]AdminUserViewModel model)
        {
            var userId = model.User.Id;
            UserRoleHelper helper = new UserRoleHelper();
            foreach (var rolermv in db.Roles.Select(r => r.Name).ToList())
            {
                
                helper.RemoveUserFromRole(userId, rolermv);
            }
            foreach (var roleadd in model.SelectedRoles)
            {
                helper.AddUserToRole(userId, roleadd);
            }
            return RedirectToAction("Index");
        }

    }
}