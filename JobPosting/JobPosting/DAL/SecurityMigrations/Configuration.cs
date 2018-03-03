namespace JobPosting.DAL.Securitymigrations
{
    using JobPosting.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JobPosting.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\Securitymigrations";
        }

        protected override void Seed(JobPosting.DAL.ApplicationDbContext context)
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

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var roleResult = roleManager.Create(new IdentityRole("Admin"));
            }
            if (!context.Roles.Any(r => r.Name == "Hiring Team"))
            {
                var roleResult = roleManager.Create(new IdentityRole("Hiring Team"));
            }
            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                var roleResult = roleManager.Create(new IdentityRole("Manager"));
            }
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var roleResult = roleManager.Create(new IdentityRole("User"));
            }

            var manager = new UserManager<ApplicationUser>(
                            new UserStore<ApplicationUser>(context));

            var adminuser = new ApplicationUser
            {
                UserName = "admin1@outlook.com",
                Email = "admin1@outlook.com"
            };

            var hiringteamuser = new ApplicationUser
            {
                UserName = "hiringteam1@outlook.com",
                Email = "hiringteam1@outlook.com"
            };

            var manageruser = new ApplicationUser
            {
                UserName = "manager1@outlook.com",
                Email = "manager1@outlook.com"
            };

            if (!context.Users.Any(u => u.UserName == "admin1@outlook.com"))
            {
                manager.Create(adminuser, "password");
                manager.AddToRole(adminuser.Id, "Admin");
            }
            if (!context.Users.Any(u => u.UserName == "hiringteam1@outlook.com"))
            {
                manager.Create(hiringteamuser, "password");
                manager.AddToRole(hiringteamuser.Id, "Hiring Team");
                
            }
            if (!context.Users.Any(u => u.UserName == "manager1@outlook.com"))
            {
                manager.Create(manageruser, "password");
                manager.AddToRole(manageruser.Id, "Manager");
            }
            
        }
    }
}
