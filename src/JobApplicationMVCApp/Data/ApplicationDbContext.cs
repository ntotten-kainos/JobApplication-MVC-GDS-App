using JobApplicationMVCApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationMVCApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
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
        // Configure JobPosting enums to use string conversion
        modelBuilder.Entity<JobPosting>()
            .Property(j => j.Type)
            .HasConversion<string>();

        modelBuilder.Entity<JobPosting>()
            .Property(j => j.Status)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder); // Call the base class method
    }
    
    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Department> Departments { get; set; }
}