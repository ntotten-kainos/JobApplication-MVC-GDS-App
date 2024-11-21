using JobApplicationMVCApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationMVCApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<JobPost> JobPosts { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
 
    
    
}