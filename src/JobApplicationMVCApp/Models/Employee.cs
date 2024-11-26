using System.ComponentModel.DataAnnotations;
namespace JobApplicationMVCApp.Models
{
    public class Employee
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }
        
        [Required]
        public string EmployeeForename { get; set; }
        
        [Required]
        public string EmployeeSurname { get; set; }
        
    }
}