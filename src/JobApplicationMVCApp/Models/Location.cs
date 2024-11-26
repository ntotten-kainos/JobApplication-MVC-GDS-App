using System.ComponentModel.DataAnnotations;
namespace JobApplicationMVCApp.Models
{
    public class Location
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int LocationId { get; set; }
        
        [Required]
        [StringLength(100), ErrorMessage = "Location Name cannot be longer than 100 characters.")]
        public string LocationName { get; set; }
        
        [Required]
        [StringLength(171), ErrorMessage = "Location Street Address cannot be longer than 171 characters.")]
        public string LocationStreetAddress { get; set; }
        
        [Required]
        [StringLength(58), ErrorMessage = "Location Name cannot be longer than 58 characters.")]
        public string LocationCity { get; set; }
        
        [Required]
        [StringLength(58), ErrorMessage = "Location Name cannot be longer than 58 characters.")]
        public string LocationPostCode { get; set; }
        
        [Required]
        public string LocationCountry { get; set; }
    }
}