using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication2.Models;
using WebApplication2.Domain;
using WebApplication2.Repositories.SQLServer;
using System;

[assembly: OwinStartupAttribute(typeof(WebApplication2.Startup))]
namespace WebApplication2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        // In this method we will create default User roles and Admin user for login
        private void createRolesandUsers()
        {
            SQLServerProjectRepository db = new SQLServerProjectRepository();
            ApplicationDbContext _context = new ApplicationDbContext();
            SQLServerEmployeeRepository emprepo = new SQLServerEmployeeRepository();
            SQLServerAspNetUserRepository asprepo = new SQLServerAspNetUserRepository();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));


            // In Startup iam creating first Admin Role and creating a default Admin User 
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website				

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "myemail@dom.com";

                string userPWD = "A$dfghjkl11";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                    var aspdata = asprepo.GetByUsername(user.UserName);
                    emprepo.Save(new Employee { uid = aspdata.uid, Birthdate = DateTime.Parse("1/1/2000"), Joindate = DateTime.Now, FirstName = "Admin", LastName = "User" });
                }
            }

            // creating Creating Manager role 
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role 
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }
        }
    }
}
