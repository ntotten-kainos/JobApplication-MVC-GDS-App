using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using JobApplicationMVCApp.Models;

namespace JobApplicationMVCApp.Data.Seed
{
    public class ApplicationDbContextSeed
    { 
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();

                // Define roles
                var roles = new[] { "Admin", "Recruiter" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create a default admin user
                var adminEmail = "admin@example.com";
                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin == null)
                {
                    admin = new Employee
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmployeeForename = "System",
                        EmployeeSurname = "Admin",
                    };

                    var result = await userManager.CreateAsync(admin, "Admin@1234"); // Default password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
        }
}