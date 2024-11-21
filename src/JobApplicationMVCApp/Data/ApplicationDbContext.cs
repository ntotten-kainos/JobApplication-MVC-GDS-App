using JobApplicationMVCApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationMVCApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    static readonly string connectionString = "Server=localhost; User ID=root; Password=pass; Database=blog";


    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(MySqlConnectionString, ServerVersion.AutoDetect(MySqlConnectionString));
    }
    
    
}