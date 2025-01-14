using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationMVCApp.Controllers
{
    [Authorize(Roles = "Recruiter")]
    public class RecruiterController : Controller
    {
        public IActionResult RecruiterHomepage()
        {
            return View();
        }
    }
}