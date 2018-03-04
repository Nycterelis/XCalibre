using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCalibre.Models.Helpers
{
    public class UserRoleHelper
    {
        private UserManager<ApplicationUser> userManager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserinRole(string UserId, string RoleId)
        {
            try
            {
                var result = userManager.IsInRole(UserId, RoleId);
                return result;
            }
            catch
            {
                return false;
            }
        }

        public bool AddUserToRole(string UserId, string Role)
        {
            try
            {
                var result = userManager.AddToRole(UserId, Role);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveUserFromRole(string UserId, string Role)
        {
            try
            {
                var result = userManager.RemoveFromRole(UserId, Role);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public ICollection<ApplicationUser> UsersNotInRole(string Role)
        {
            List<ApplicationUser> roleUsers = new List<ApplicationUser>();
            List<ApplicationUser> users = userManager.Users.ToList();
            foreach (var u in users)
            {
                if (!IsUserinRole(u.Id, Role))
                {
                    roleUsers.Add(u);
                }
            }
            return roleUsers;
        }

        public ICollection<string> ListUserRoles(string UserId)
        {
            return userManager.GetRoles(UserId);
        }

    }
}