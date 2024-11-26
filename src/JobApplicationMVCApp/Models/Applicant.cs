using System.ComponentModel.DataAnnotations;
namespace JobApplicationMVCApp.Models
{
    public class Applicant
    {
        [Key]
        [Required]
        public int ApplicantId { get; set; }
        
        [Required]
        public string ApplicantForename { get; set; }
        
        [Required]
        public string ApplicantSurname { get; set; }
        
        [Required]
        public string ApplicantEmail { get; set; }
        
        [Required]
        public string ApplicantPhoneNumber { get; set; }
        
    }
}