using System;
using System.Collections.Generic;
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
                    jobs = jobs.Where(j => j.Location != null && (j.Location.LocationName.ToLower() == location || j.Location.LocationCity.ToLower() == location || j.Location.LocationPostCode.ToLower() == location));
                }

                if (department != "all")
                {
                    jobs = jobs.Where(j => j.Department != null && j.Department.DepartmentName.ToLower() == department); 
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
            
        // // GET: Jobs/Details/5
        // public async Task<IActionResult> Details(int? id)
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

        // GET: Jobs/Create
        public IActionResult Create()
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
        public async Task<IActionResult> ManageJobs()
        {
            var applicationDbContext = _context.JobPostings.Include(j => j.Department).Include(j => j.Location);
            return View(await applicationDbContext.ToListAsync());
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