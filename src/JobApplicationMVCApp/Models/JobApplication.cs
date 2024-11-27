using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace JobApplicationMVCApp.Models
{
    public class JobApplication
    {
        
        [Required (ErrorMessage = "Applicant ID is required.")]
        [ForeignKey("ApplicantId")]
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; } = null!;
        
        [Required (ErrorMessage = "Job Posting ID is required.")]
        [ForeignKey("JobPostingId")]
        public int JobPostingId { get; set; }
        public JobPosting JobPosting { get; set; } = null!;
    }
}