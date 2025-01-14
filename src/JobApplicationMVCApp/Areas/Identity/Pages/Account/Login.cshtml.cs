using Microsoft.AspNetCore.Mvc.RazorPages; // For PageModel
using Microsoft.AspNetCore.Identity;      // For IdentityUser, SignInManager, UserManager
using System.ComponentModel.DataAnnotations; // For Required, DataType, and EmailAddress attributes
using Microsoft.AspNetCore.Mvc;           // For IActionResult
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } // Add this property
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                Input.Email, 
                Input.Password, 
                Input.RememberMe, // Use the RememberMe property here
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return LocalRedirect("~/Admin/AdminHomepage");
                }
                else if (await _userManager.IsInRoleAsync(user, "Recruiter"))
                {
                    return LocalRedirect("~/Recruiter/RecruiterHomepage");
                }
                else
                {
                    return LocalRedirect("~/Home/UserHomepage");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return Page();
    }
}