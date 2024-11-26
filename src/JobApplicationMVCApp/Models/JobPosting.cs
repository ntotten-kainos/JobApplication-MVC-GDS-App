using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobApplicationMVCApp.ValidationAttributes;

namespace JobApplicationMVCApp.Models
{
    public class JobPosting
    {
        [Key]
        [Required(ErrorMessage = "Job posting ID is required.")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int JobPostingId { get; set; }

        [Required(ErrorMessage = "Job Title is required.")]
        [StringLength(100, ErrorMessage = "Job title cannot be longer than 100 characters.")]
        public string JobTitle { get; set; } = null!;
        
        [Required(ErrorMessage = "Job Description is required.")]
        [StringLength(1000, ErrorMessage = "Job description cannot be longer than 1000 characters.")]
        public string JobDescription { get; set; } = null!;

        [Required(ErrorMessage = "Job Requirements are required.")]
        [StringLength(1000, ErrorMessage = "Job requirements cannot be longer than 1000 characters.")]
        public string JobRequirements { get; set; } = null!;
        
        [Required(ErrorMessage = "Job Location is required.")]
        [ForeignKey("Location")]
        public int JobLocationId { get; set; }
        
        public Location Location { get; set; } = null!;
        
        [Required(ErrorMessage = "Department ID is required.")]
        [ForeignKey("Department")]
        public int JobDepartmentId { get; set; }

        public Department Department { get; set; } = null!;
        
   
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public double? Salary { get; set; }
        
        [Required(ErrorMessage = "Closing Date is required.")]
        [FutureDate(ErrorMessage = "Closing Date must be in the future.")]
        public DateTime ClosingDate { get; set; }
        
        public enum JobType
        {
            FullTime,
            PartTime,
            Contract,
            Internship
        }
        public JobType Type { get; set; } = JobType.FullTime;
        
        public enum JobStatus
        {
            Open,
            Closed,
            Draft
        }
        public JobStatus Status { get; set; } = JobStatus.Draft;
        
        [Required(ErrorMessage = "Date Posted is required.")]
        public DateTime DatePosted { get; set; }
        
    }
}