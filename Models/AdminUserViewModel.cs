using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace XCalibre.Models
{
    public class AdminUserViewModel
    {
       

        public ApplicationUser User { get; set; }
        public MultiSelectList Roles { get; set; }
        public string[] SelectedRoles { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        

        
        
       

    }
    public class AdminUserRolesModel
    {
        public ApplicationUser User { get; set; }
        public ICollection<string> Roles { get; set; }

    }
    //public class ProjectManageViewModel
    //{
    //    public List<Ticket> Unassigned { get; set; }
    //    public List<Ticket> Assigned { get; set; }
    //    public List<AdminUserViewModel> Roles { get; set; }
    //    public List<ApplicationUser> Devs { get; set; }
    //    public List<ApplicationUser> Submitters { get; set; }
    //    public List<Project> Projects { get; set; }
    //}
}