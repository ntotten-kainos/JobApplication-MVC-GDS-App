using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplicationMVCApp.Models
{
    public class Department
    {
        [Key]
        [Required(ErrorMessage = "Department ID is required.")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int DepartmentId { get; set; }
        
        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(100, ErrorMessage = "Department Name cannot be longer than 100 characters.")]
        public string DepartmentName { get; set; }
    }
}