using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobApplicationMVCApp.Models;

namespace JobApplicationMVCApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
	 public DbSet<JobPosting> JobPostings { get; set; }
        	public DbSet<Location> Locations { get; set; }
        	public DbSet<Department> Departments { get; set; }

        	protected override void OnModelCreating(ModelBuilder modelBuilder)
        	{
            	modelBuilder.Entity<JobPosting>()
                	.Property(j => j.Type)
               	 	.HasConversion<string>();

            	modelBuilder.Entity<JobPosting>()
                	.Property(j => j.Status)
                	.HasConversion<string>();

            	base.OnModelCreating(modelBuilder);
        	}
}