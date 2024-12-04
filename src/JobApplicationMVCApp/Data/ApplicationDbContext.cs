using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var name = Environment.GetEnvironmentVariable("DB_NAME");
            var username = Environment.GetEnvironmentVariable("DB_USERNAME");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306"; // Default MySQL port

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Database configuration environment variables are missing.");
            }

            var mySqlConnString = $"Server={host}; Port={port}; User ID={username}; Password={password}; Database={name};";

            optionsBuilder.UseMySql(mySqlConnString, ServerVersion.AutoDetect(mySqlConnString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobPosting>()
                .Property(j => j.Type)
                .HasConversion<string>();

            modelBuilder.Entity<JobPosting>()
                .Property(j => j.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Applicant>()
                .OwnsOne(a => a.Address);
            modelBuilder.Entity<Location>()
                .OwnsOne(a => a.Address);

            modelBuilder.Entity<Applicant>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasBaseType<IdentityUser>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Employee>("Employee");


            modelBuilder.Entity<JobApplication>()
                .HasNoKey()
                .ToTable("JobApplications");


            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.JobPosting)
                .WithMany()
                .HasForeignKey(ja => ja.JobPostingId);

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Applicant)
                .WithMany()
                .HasForeignKey(ja => ja.ApplicantId);

            base.OnModelCreating(modelBuilder);
        }
    }
}