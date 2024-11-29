using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationMVCApp.Models
{
    public class Applicant
    {
        [Key]
        [Required]
        public int ApplicantId { get; set; }
        
        [Required(ErrorMessage = "Forename is required.")]
        [StringLength(100, ErrorMessage = "Forename cannot be longer than 100 characters.")]
        public string Forename { get; set; }
        
        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(100, ErrorMessage = "Surname cannot be longer than 100 characters.")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }
        
        [Required]
        public Address Address { get; set; } // Complex type
        
        [ForeignKey("User")]
        public string UserId { get; set; } 
        public IdentityUser User { get; set; }
        
    }
}