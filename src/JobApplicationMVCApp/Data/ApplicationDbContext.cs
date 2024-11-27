using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobApplicationMVCApp.Models;

namespace JobApplicationMVCApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure JobPosting enums to use string conversion
            modelBuilder.Entity<JobPosting>()
                .Property(j => j.Type)
                .HasConversion<string>();

            modelBuilder.Entity<JobPosting>()
                .Property(j => j.Status)
                .HasConversion<string>();

            // Configure JobApplication as a keyless entity
            modelBuilder.Entity<JobApplication>()
                .HasNoKey()
                .ToTable("JobApplications"); // Optional: Specify the table name explicitly

            // Configure relationships for JobApplication
            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.JobPosting)
                .WithMany() // No navigation back from JobPosting to JobApplication
                .HasForeignKey(ja => ja.JobPostingId);

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Applicant)
                .WithMany() // No navigation back from Applicant to JobApplication
                .HasForeignKey(ja => ja.ApplicantId);

            base.OnModelCreating(modelBuilder); // Call the base class method
        }
    }
}
