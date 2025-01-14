using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationMVCApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult AdminHomepage()
        {
            return View();
        }

        public IActionResult ManageRoles()
        {
            return View();
        }
    }
}