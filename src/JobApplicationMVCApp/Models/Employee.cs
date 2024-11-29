using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationMVCApp.Models
{
    public class Employee : IdentityUser
    {
        [Required]
        [StringLength(100, ErrorMessage = "Forename cannot be longer than 100 characters.")]
        public string EmployeeForename { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Surname cannot be longer than 100 characters.")]
        public string EmployeeSurname { get; set; }
    }
}