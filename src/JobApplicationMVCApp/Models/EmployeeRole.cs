using System.ComponentModel.DataAnnotations;
namespace JobApplicationMVCApp.Models
{
    public class EmployeeRole
    {
        [Key]
        [Required]
        public int EmployeeRoleId { get; set; }
        
        [Required]
        public string EmployeeRoleName { get; set; }
    }
}