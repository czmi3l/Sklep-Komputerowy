using System.Data.Entity.Validation;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebUI.Infrastructure;

namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebUI.Infrastructure.AppIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "WebUI.Infrastructure.AppIdentityDbContext";
        }

        protected override void Seed(WebUI.Infrastructure.AppIdentityDbContext context)
        {
            try
            {
                AppUserManager userManager = new AppUserManager(new UserStore<AppUser>(context));
                AppRoleManager roleManager = new AppRoleManager(new RoleStore<AppRole>(context));

                string admRole = "Administrators";
                string admName = "Admin";
                string admPass = "MySecret";
                string admEmail = "admin@gmail.com";

                string[] roles = {admRole, "Users"};
                foreach (var role in roles)
                {
                    if (!roleManager.RoleExists(role))
                    {
                        roleManager.Create(new AppRole(role));
                    }
                }

                AppUser user = userManager.FindByName(admName);
                if (user == null)
                {
                    userManager.Create(new AppUser {UserName = admName, Email = admEmail, FirstName = "Admin", LastName = "Admin", Street = "Admin 1", City = "Admin", ZipCode = "Admin"}, admPass);
                    user = userManager.FindByName(admName);
                }

                if (!userManager.IsInRole(user.Id, admRole))
                {
                    userManager.AddToRole(user.Id, admRole);
                }

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
