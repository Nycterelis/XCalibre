namespace XCalibre.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using XCalibre.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<XCalibre.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(XCalibre.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            //Creating the Admin Role if it isn't here
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }
            //Creating the Developer Role if it isn't here
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                role = new IdentityRole { Name = "Developer" };
                manager.Create(role);
            }
            //Creating the Submitter Role if it isn't here
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                role = new IdentityRole { Name = "Submitter" };
                manager.Create(role);
            }
            //Creating the Project Manager role if it isn't here
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                role = new IdentityRole { Name = "ProjectManager" };
                manager.Create(role);
            }
            //Seeding Myself into Admin, and PlaceHolder users for the other roles
            if (!context.Users.Any(u => u.Email == "toomuchyang@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "toomuchyang@gmail.com",
                    Email = "toomuchyang@gmail.com",
                    FirstName = "Reid",
                    LastName = "Thompson"
                };
                userManager.Create(user, "Abc&123!");
                userManager.AddToRoles(user.Id, new string[] { "Admin" });
            }
            if (!context.Users.Any(u => u.Email == "prjmgr@placeholder.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "prjmgr@placeholder.com",
                    Email = "prjmgr@placeholder.com",
                    FirstName = "ProjectManager",
                    LastName = "PlaceHolder"
                };
                userManager.Create(user, "Abc&123!");
                userManager.AddToRole(user.Id, "ProjectManager");
            }
            if (!context.Users.Any(u => u.Email == "developer@placeholder.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "developer@placeholder.com",
                    Email = "developer@placeholder.com",
                    FirstName = "Developer",
                    LastName = "PlaceHolder"
                };
                userManager.Create(user, "Abc&123!");
                userManager.AddToRole(user.Id, "Developer");
            }
            if (!context.Users.Any(u => u.Email == "submitter@placeholder.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "submitter@placeholder.com",
                    Email = "submitter@placeholder.com",
                    FirstName = "Submitter",
                    LastName = "PlaceHolder"
                };
                userManager.Create(user, "Abc&123!");
                userManager.AddToRole(user.Id, "Submitter");
            }

            //Seeding the Ticket Statuses Below
            if (!context.TicketStatuses.Any(s => s.Name == "New"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "New" });
            }
            if (!context.TicketStatuses.Any(s => s.Name == "Assigned"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Assigned" });
            }
            if (!context.TicketStatuses.Any(s => s.Name == "In Development"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "In Development" });
            }
            if (!context.TicketStatuses.Any(s => s.Name == "Testing"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Testing" });
            }
            if (!context.TicketStatuses.Any(s => s.Name == "Approved"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Approved" });
            }
            if (!context.TicketStatuses.Any(s => s.Name == "Resolved"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Resolved" });
            }

            //Seeding the Ticket Types Below

            if (!context.TicketTypes.Any(t => t.Name == "Production Fix"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Production Fix" });
            }
            if (!context.TicketTypes.Any(t => t.Name == "Projet Task"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Project Task" });
            }
            if (!context.TicketTypes.Any(t => t.Name == "Software Update"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Software Update" });
            }

            //Seeding the Ticket Priorities
            if (!context.TicketPriorities.Any(p => p.Name == "Urgent"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Urgent" });
            }
            if (!context.TicketPriorities.Any(p => p.Name == "High"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "High" });
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Medium"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Medium" });
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Low"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Low" });
            }
        }
    }
}
