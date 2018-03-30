using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XCalibre.Models
{
    public class AdminProjectViewModel
    {
        public MultiSelectList User { get; set; }
        public SelectList ProjectManager { get; set; }
        public Project Projects { get; set; }
        public string[] SelectedUsers { get; set; } //Using this one for the multi select
        public string SelectedUser { get; set; } //Using this for the Select
        public int Id { get; set; }
        public string Name { get; set; }
        public string TempUrl { get; set; }


    }

    public class AdminUserProjectsModel
    {
        public Project Project { get; set; }
        public ICollection<string> User { get; set; }

    }
}