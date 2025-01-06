using JobApplicationMVCApp.Models;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationMVCApp.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            // Define our roles and add each if it doesn't already exist. 
            var roles = new[] { "Admin", "Recruiter", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
            // add a default admin for testing
            var adminEmail = "admin@gmail.com";
            var adminPassword = "JobAppAdmin!1234";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser() { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        public static async Task SeedDefaultUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
            // add a default admin for testing
            var userEmail = "default@gmail.com";
            var userPassword = "JobAppDefault!1234";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var defaultUser = new ApplicationUser() { UserName = userEmail, Email = userEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(defaultUser, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "User");
                }
            }
            
        }
    }
}