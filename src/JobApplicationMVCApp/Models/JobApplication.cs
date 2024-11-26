using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace JobApplicationMVCApp.Models
{
    public class JobApplication
    {
        
        [Required (ErrorMessage = "Applicant ID is required.")]
        [ForeignKey("ApplicantId")]
        public int ApplicantId { get; set; }
        
        [Required (ErrorMessage = "Job Posting ID is required.")]
        [ForeignKey("JobPostingId")]
        public string JobPostingId { get; set; }
    }
}