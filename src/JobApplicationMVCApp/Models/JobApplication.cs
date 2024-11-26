using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace JobApplicationMVCApp.Models
{
    public class JobApplication
    {
        [Key]
        [Required]
        public int Applicant { get; set; }
        
        [Required]
        [ForeignKey("JobPosting")]
        public string JobPostingId { get; set; }
    }
}