using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using XCalibre.Models.Helpers;

namespace XCalibre.Models.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string UserId, int ProjectId)
        {
            try
            {
                var prj = db.Projects.Find(ProjectId);
                var usr = db.Users.Find(UserId);
                if (prj.Users.Contains(usr))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        //public Exception AddUserToProject(string UserId, int ProjectId)
        //{
        //    try
        //    {

        //        var prj = db.Projects.Find(ProjectId);
        //        var usr = db.Users.Find(UserId);
        //        if (!IsUserOnProject(UserId, ProjectId) == true)
        //        {

        //            prj.Users.Add(usr);
        //            db.SaveChanges();
        //        }
        //        return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        return ex;
        //    }
        //}

        public Exception AddUserToProject(string UserId, int ProjectId)
        {
            try
            {
                var prj = db.Projects.Find(ProjectId);
                var usr = db.Users.Find(UserId);
                prj.Users.Add(usr);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception AddPmToProject(string UserId, int ProjectId)
        {
            try
            {
                var prj = db.Projects.Find(ProjectId);
                var pm = UserId;
                prj.PmId = pm;
                db.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {

                return ex;
            }
        }

        public Exception RemoveUserToProject(string UserId, int ProjectId)
        {
            try
            {
                var prj = db.Projects.Find(ProjectId);
                var usr = db.Users.Find(UserId);
                if (IsUserOnProject(UserId, ProjectId) == true)
                {
                    prj.Users.Remove(usr);
                    db.SaveChanges();
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ICollection<Project> ListProjectsForUser(string userId)
        {
            return db.Users.Find(userId).Projects.ToList();
        }

        public ICollection<ApplicationUser> ListUsersOnProject(int ProjectId)
        {
            return db.Projects.Find(ProjectId).Users.ToList();
        }

        //Showing All users not assigned to a *SPECIFIC* project
        public ICollection<ApplicationUser> ListUsersNotOnProject(int ProjectId)
        {
            var usersOnProject = db.Projects.Find(ProjectId).Users;
            return db.Users.Except(usersOnProject).ToList();
        }

        //Showing All users not assigned to *ANY* project
        public ICollection<ApplicationUser> ListUsersNotOnProjects()
        {
            List<ApplicationUser> usersNotOnProjects = new List<ApplicationUser>();
            List<ApplicationUser> users = db.Users.ToList();
            foreach (var u in users)
            {
                if (u.Projects == null)
                {
                    usersNotOnProjects.Add(u);
                }
            }
            return usersNotOnProjects;
        }

        
    }
}