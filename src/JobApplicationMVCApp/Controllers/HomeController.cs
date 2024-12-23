using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JobApplicationMVCApp.Models;
using Microsoft.Extensions.Logging;

namespace JobApplicationMVCApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult AdminHomepage()
    {
        return View();
    }

    public IActionResult ManageRoles()
    {
        return View();
    }

    public IActionResult UserHomepage()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}