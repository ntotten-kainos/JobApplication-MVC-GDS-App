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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JobPostings.Include(j => j.Department).Include(j => j.Location);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult ManageJobs()
        {
            return View();
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