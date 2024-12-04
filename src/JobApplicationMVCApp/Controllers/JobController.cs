using Microsoft.AspNetCore.Mvc;
using JobApplicationMVCApp.Models; 
using System.Linq;
using System.Collections.Generic;
using JobApplicationMVCApp.Data;

namespace JobApplicationMVCApp.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ViewJobRoles(string location, string department, string status, string type, string sort)
        {
            // Start with all job postings
            var jobs = _context.JobPostings.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(location) && location.ToLower() != "all")
            {
                jobs = jobs.Where(j => j.Location.LocationName == location);
            }
            if (!string.IsNullOrEmpty(department) && department.ToLower() != "all")
            {
                jobs = jobs.Where(j => j.Department.DepartmentName == department);
            }
            if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
            {
                jobs = jobs.Where(j => j.Status.ToString().ToLower() == status.ToLower());
            }
            if (!string.IsNullOrEmpty(type) && type.ToLower() != "all")
            {
                jobs = jobs.Where(j => j.Type.ToString().ToLower() == type.ToLower());
            }

            // Apply sorting
            jobs = sort switch
            {
                "date-asc" => jobs.OrderBy(j => j.ClosingDate),
                "date-desc" => jobs.OrderByDescending(j => j.ClosingDate),
                _ => jobs // No sorting applied if sort is null or invalid
            };

            // Store current selections in ViewData
            ViewData["SelectedLocation"] = string.IsNullOrEmpty(location) ? "all" : location;
            ViewData["SelectedDepartment"] = string.IsNullOrEmpty(department) ? "all" : department;
            ViewData["SelectedStatus"] = string.IsNullOrEmpty(status) ? "all" : status;
            ViewData["SelectedType"] = string.IsNullOrEmpty(type) ? "all" : type;
            ViewData["SortBy"] = string.IsNullOrEmpty(sort) ? "" : sort;
            
            return View(jobs.ToList());
        }
    }
}