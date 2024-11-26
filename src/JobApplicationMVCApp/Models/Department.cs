using System.ComponentModel.DataAnnotations;
namespace JobApplicationMVCApp.Models
{
    public class Department
    {
        [Key]
        [Required]
        public int DepartmentId { get; set; }
        
        [Required]
        public string DepartmentName { get; set; }
    }
}