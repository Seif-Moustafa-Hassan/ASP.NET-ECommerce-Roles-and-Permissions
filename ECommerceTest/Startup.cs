using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ECommerceTest.Models; // Make sure your ApplicationDbContext is referenced

[assembly: OwinStartupAttribute(typeof(ECommerceTest.Startup))]
namespace ECommerceTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Seed roles and assign them to existing users
            //CreateRolesAndAssignToUsers();
        }

        /*
        private void CreateRolesAndAssignToUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // 1️⃣ Create Admin role
                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new IdentityRole("Admin");
                    roleManager.Create(role);
                }

                // 2️⃣ Create User role
                if (!roleManager.RoleExists("User"))
                {
                    var role = new IdentityRole("User");
                    roleManager.Create(role);
                }

                // 3️⃣ Assign roles to existing users
                var adminUser = userManager.FindByEmail("admin@gmail.com");
                if (adminUser != null && !userManager.IsInRole(adminUser.Id, "Admin"))
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }

                var normalUser = userManager.FindByEmail("user@gmail.com");
                if (normalUser != null && !userManager.IsInRole(normalUser.Id, "User"))
                {
                    userManager.AddToRole(normalUser.Id, "User");
                }
            }
        }
        */
    }
}