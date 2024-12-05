using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobApplicationMVCApp.Data;
using JobApplicationMVCApp.Models;

namespace JobApplicationMVCApp.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jobs
       public async Task<IActionResult> Index(string location, string department, string status, string type, string sort)
{
    // Set default values to "All" if parameters are null
    location = string.IsNullOrEmpty(location) ? "All" : location;
    department = string.IsNullOrEmpty(department) ? "All" : department;
    status = string.IsNullOrEmpty(status) ? "All" : status;
    type = string.IsNullOrEmpty(type) ? "All" : type;

    // Normalize filter inputs for comparison
    location = location.ToLower();
    department = department.ToLower();
    status = status.ToLower();
    type = type.ToLower();

    // Start with all job postings, including related tables
    var jobs = _context.JobPostings
        .Include(j => j.Location)
        .Include(j => j.Department)
        .AsQueryable();

    // Apply filters if not "All"
    if (location != "all")
    {
        jobs = jobs.Where(j => j.Location != null &&
                               (j.Location.LocationName.ToLower() == location ||
                                j.Location.LocationCity.ToLower() == location ||
                                j.Location.LocationPostCode.ToLower() == location));
    }

    if (department != "all")
    {
        jobs = jobs.Where(j => j.Department != null &&
                               j.Department.DepartmentName.ToLower() == department);
    }

    if (status != "all")
    {
        if (Enum.TryParse<JobPosting.JobStatus>(status, true, out var parsedStatus))
        {
            jobs = jobs.Where(j => j.Status == parsedStatus);
        }
    }

    if (type != "all")
    {
        var typeMappings = new Dictionary<string, JobPosting.JobType>(StringComparer.OrdinalIgnoreCase)
        {
            { "full-time", JobPosting.JobType.FullTime },
            { "part-time", JobPosting.JobType.PartTime },
            { "contract", JobPosting.JobType.Contract },
            { "internship", JobPosting.JobType.Internship }
        };

        if (typeMappings.TryGetValue(type, out var parsedType))
        {
            jobs = jobs.Where(j => j.Type == parsedType);
        }
    }

    // Apply sorting
    jobs = sort switch
    {
        "date-asc" => jobs.OrderBy(j => j.ClosingDate),
        "date-desc" => jobs.OrderByDescending(j => j.ClosingDate),
        "salary-asc" => jobs.OrderBy(j => j.Salary),
        "salary-desc" => jobs.OrderByDescending(j => j.Salary),
        _ => jobs
    };

    // Pass selected values to the view
    ViewData["SelectedLocation"] = location;
    ViewData["SelectedDepartment"] = department;
    ViewData["SelectedStatus"] = status;
    ViewData["SelectedType"] = type;
    ViewData["SelectedSort"] = sort;

    return View(await jobs.ToListAsync());
}




        
        // GET: Jobs/Create
        public IActionResult Create()
        {
            ViewData["JobDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["JobLocationId"] = new SelectList(_context.Locations, "LocationId", "LocationCity");
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobPostingId,JobTitle,JobDescription,JobRequirements,JobLocationId,JobDepartmentId,Salary,ClosingDate,Type,Status,DatePosted")] JobPosting jobPosting)
        {
            jobPosting.DatePosted = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.JobPostings.Add(jobPosting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["JobDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", jobPosting.JobDepartmentId);
            ViewData["JobLocationId"] = new SelectList(_context.Locations, "LocationId", "LocationCity", jobPosting.JobLocationId);
            return View(jobPosting);
        }

        private bool JobPostingExists(int id)
        {
            return _context.JobPostings.Any(e => e.JobPostingId == id);
        }
    }
}