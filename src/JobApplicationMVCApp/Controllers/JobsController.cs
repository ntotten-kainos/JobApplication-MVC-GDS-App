using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobApplicationMVCApp.Data;
using JobApplicationMVCApp.Models;
using Microsoft.AspNetCore.Authorization;

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
            [AllowAnonymous]
            public async Task<IActionResult> Index(string location, string department, string status, string type, string sort, int? pageNumber)
            {
                // Populate filter dropdowns
                ViewData["Departments"] = await _context.Departments.Select(d => d.DepartmentName).ToListAsync();
                ViewData["Locations"] = await _context.Locations.Select(l => l.LocationCity).ToListAsync();
                ViewData["Status"] = Enum.GetNames(typeof(JobPosting.JobStatus))
                    .Where(status => status == nameof(JobPosting.JobStatus.Open) || status == nameof(JobPosting.JobStatus.Closed))
                    .ToArray();;
                ViewData["Type"] = Enum.GetNames(typeof(JobPosting.JobType));
                
                // Fetch all jobs with necessary includes
                var jobs = _context.JobPostings
                    .Include(j => j.Location)
                    .Include(j => j.Department)
                    .AsQueryable();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(location) && location != "All")
                {
                    jobs = jobs.Where(j => j.Location.LocationCity.ToLower().Equals(location.ToLower()));
                    ViewData["SelectedLocation"] = location;
                }

                if (!string.IsNullOrWhiteSpace(department)&& department != "All")
                {
                    jobs = jobs.Where(j => j.Department.DepartmentName.ToLower().Equals(department.ToLower()));
                    ViewData["SelectedDepartment"] = department;
                }

                if (!string.IsNullOrWhiteSpace(status) && status != "All")
                {
                    if (Enum.TryParse(status, out JobPosting.JobStatus parsedStatus))
                    {
                        jobs = jobs.Where(j => j.Status == parsedStatus);
                    }
                    ViewData["SelectedStatus"] = status;
                }

                if (!string.IsNullOrWhiteSpace(type) && type != "All")
                {
                    if (Enum.TryParse(type, out JobPosting.JobType parsedType))
                    {
                        jobs = jobs.Where(j => j.Type == parsedType);
                    }

                    ViewData["SelectedType"] = type;
                }
                
                // Apply sorting (optional, e.g., by Closing Date)
                if (!string.IsNullOrWhiteSpace(sort))
                {
                    if (sort.Equals("closingdate", StringComparison.OrdinalIgnoreCase))
                    {
                        jobs = jobs.OrderBy(j => j.ClosingDate);
                    }
                    ViewData["SelectedSort"] = sort;
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
                
                var count = await jobs.CountAsync();
                int pageSize = 4;
                return View(await PaginatedList<JobPosting>.CreateAsync(jobs.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            
            [HttpGet]
            [Authorize(Roles = "Admin, Recruiter, User")]
            public IActionResult Apply(int id)
            {
                // Fetch the JobPosting details for the provided ID with Location included
                var jobPosting = _context.JobPostings
                    .Include(j => j.Location) // Eagerly load related Location data
                    .FirstOrDefault(j => j.JobPostingId == id);

                if (jobPosting == null)
                {
                    return NotFound();
                }

                // Initialize the JobApplication model with the JobPostingId
                var jobApplication = new JobApplication
                {
                    JobPostingId = id,
                    JobPosting = jobPosting
                };

                return View(jobApplication);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles = "Admin, Recruiter, User")]
            public async Task<IActionResult> Apply(JobApplication model, string action)
            {
                if (!ModelState.IsValid)
                {
                    var relatedJobPosting = _context.JobPostings
                        .Include(j => j.Location)
                        .FirstOrDefault(j => j.JobPostingId == model.JobPostingId);

                    if (relatedJobPosting != null)
                    {
                        model.JobPosting = relatedJobPosting;
                    }
                    return View(model);
                }

                try
                {
                    if (string.Equals(action, "save", StringComparison.OrdinalIgnoreCase))
                    {
                        model.Status = JobApplication.ApplicationStatus.Draft;
                        ViewData["SuccessMessage"] = "Your application has been saved as a draft.";
                    }
                    else if (string.Equals(action, "submit", StringComparison.OrdinalIgnoreCase))
                    {
                        model.Status = JobApplication.ApplicationStatus.Submitted;
                        model.DateSubmitted = DateTime.UtcNow;
                        ViewData["SuccessMessage"] = "Your application has been successfully submitted.";
                    }

                    _context.JobApplications.Add(model);
                    await _context.SaveChangesAsync();

                    // Reload the job posting to pass to the view
                    model.JobPosting = _context.JobPostings
                        .Include(j => j.Location)
                        .FirstOrDefault(j => j.JobPostingId == model.JobPostingId);

                    return View(model);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving application: {ex.Message}");
                    ViewData["ErrorMessage"] = "An error occurred while processing your application.";
                    return View(model);
                }
            }

            // GET: Jobs/JobDescription/5
            [Authorize(Roles = "Admin, Recruiter, User")]
            public async Task<IActionResult> JobDescription(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }
        
                var jobPosting = await _context.JobPostings
                    .Include(j => j.Department)
                    .Include(j => j.Location)
                    .FirstOrDefaultAsync(m => m.JobPostingId == id);
                if (jobPosting == null)
                {
                    return NotFound();
                }
        
                return View(jobPosting);
            }

        // GET: Jobs/Create
        [Authorize(Roles = "Admin, Recruiter")]
        public IActionResult Create(string returnUrl)
        {
            ViewData["JobDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["JobLocationId"] = new SelectList(_context.Locations, "LocationId", "LocationCity");

            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Create([Bind("JobPostingId,JobTitle,JobDescription,JobRequirements,JobLocationId,JobDepartmentId,Salary,ClosingDate,Type,Status,DatePosted")] JobPosting jobPosting, string action)
        {
            jobPosting.DatePosted = DateTime.Now;
            jobPosting.Department = _context.Departments.Find(jobPosting.JobDepartmentId);
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        Console.WriteLine($"Field: {error.Key}, Error: {subError.ErrorMessage}");
                    }
                }
            }
            
            if (ModelState.IsValid)
            {
                if (action == "Create")
                {
                    _context.JobPostings.Add(jobPosting);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else if (action == "Draft")
                {
                    // Save to drafts    
                } 
                else if (action == "Delete")
                {
                    // Delete from drafts
                }
            }
            ViewData["JobDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", jobPosting.JobDepartmentId);
            ViewData["JobLocationId"] = new SelectList(_context.Locations, "LocationId", "LocationCity", jobPosting.JobLocationId);
            return View(jobPosting);
        }

        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> ManageJobs(string location, string department, string status, string type, string sort, int? pageNumber)
        {
            // Populate filter dropdowns
            ViewData["Departments"] = await _context.Departments.Select(d => d.DepartmentName).ToListAsync();
            ViewData["Locations"] = await _context.Locations.Select(l => l.LocationCity).ToListAsync();
            ViewData["Status"] = Enum.GetNames(typeof(JobPosting.JobStatus));
            ViewData["Type"] = Enum.GetNames(typeof(JobPosting.JobType));

            // Fetch all jobs with necessary includes
            var jobs = _context.JobPostings
            .Include(j => j.Location)
            .Include(j => j.Department)
            .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(location) && location != "All")
            {
                jobs = jobs.Where(j => j.Location.LocationCity.ToLower().Equals(location.ToLower()));
                ViewData["SelectedLocation"] = location;
            }

            if (!string.IsNullOrWhiteSpace(department)&& department != "All")
            {
                jobs = jobs.Where(j => j.Department.DepartmentName.ToLower().Equals(department.ToLower()));
                ViewData["SelectedDepartment"] = department;
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                if (Enum.TryParse(status, out JobPosting.JobStatus parsedStatus))
                {
                    jobs = jobs.Where(j => j.Status == parsedStatus);
                }
                ViewData["SelectedStatus"] = status;
            }

            if (!string.IsNullOrWhiteSpace(type) && type != "All")
            {
                if (Enum.TryParse(type, out JobPosting.JobType parsedType))
                {
                    jobs = jobs.Where(j => j.Type == parsedType);
                }
                ViewData["SelectedType"] = type;
            }

            // Apply sorting (optional, e.g., by Closing Date)
            if (!string.IsNullOrWhiteSpace(sort))
            {
                if (sort.Equals("closingdate", StringComparison.OrdinalIgnoreCase))
                {
                    jobs = jobs.OrderBy(j => j.ClosingDate);
                }
                ViewData["SelectedSort"] = sort;
            }
            if (!string.IsNullOrWhiteSpace(sort))
            {
                if (sort.Equals("closingdate", StringComparison.OrdinalIgnoreCase))
                {
                    jobs = jobs.OrderBy(j => j.ClosingDate);
                }
                ViewData["SelectedSort"] = sort;
            }
            
            var count = await jobs.CountAsync();
            int pageSize = 4;
            return View(await PaginatedList<JobPosting>.CreateAsync(jobs.AsNoTracking(), pageNumber ?? 1, pageSize));
        }       

        [Authorize(Roles = "Admin, Recruiter, User")]
        public async Task<IActionResult> ViewJob(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            var jobPosting = await _context.JobPostings
                .Include(j => j.Department)
                .Include(j => j.Location)
                .FirstOrDefaultAsync(m => m.JobPostingId == id);
            if (jobPosting == null)
            {
                return NotFound();
            }
        
            return View(jobPosting);
        }

        // // GET: Jobs/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var jobPosting = await _context.JobPostings.FindAsync(id);
        //     if (jobPosting == null)
        //     {
        //         return NotFound();
        //     }
        //     ViewData["JobDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", jobPosting.JobDepartmentId);
        //     ViewData["JobLocationId"] = new SelectList(_context.Locations, "LocationId", "LocationCity", jobPosting.JobLocationId);
        //     return View(jobPosting);
        // }

        // // POST: Jobs/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("JobPostingId,JobTitle,JobDescription,JobRequirements,JobLocationId,JobDepartmentId,Salary,ClosingDate,Type,Status,DatePosted")] JobPosting jobPosting)
        // {
        //     if (id != jobPosting.JobPostingId)
        //     {
        //         return NotFound();
        //     }
        //
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(jobPosting);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!JobPostingExists(jobPosting.JobPostingId))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["JobDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", jobPosting.JobDepartmentId);
        //     ViewData["JobLocationId"] = new SelectList(_context.Locations, "LocationId", "LocationCity", jobPosting.JobLocationId);
        //     return View(jobPosting);
        // }

        // // GET: Jobs/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var jobPosting = await _context.JobPostings
        //         .Include(j => j.Department)
        //         .Include(j => j.Location)
        //         .FirstOrDefaultAsync(m => m.JobPostingId == id);
        //     if (jobPosting == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(jobPosting);
        // }

        // // POST: Jobs/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var jobPosting = await _context.JobPostings.FindAsync(id);
        //     if (jobPosting != null)
        //     {
        //         _context.JobPostings.Remove(jobPosting);
        //     }
        //
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        private bool JobPostingExists(int id)
        {
            return _context.JobPostings.Any(e => e.JobPostingId == id);
        }
    }
}