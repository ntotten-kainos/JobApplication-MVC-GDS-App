using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplicationMVCApp.Models
{
    public class Location
    {
        [Key]
        [Required(ErrorMessage = "Location ID is required.")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int LocationId { get; set; }
        
        [Required(ErrorMessage = "Location name is required.")]
        [StringLength(100, ErrorMessage = "Location Name cannot be longer than 100 characters.")]
        public string LocationName { get; set; }
        
        [Required(ErrorMessage = "Street address is required.")]
        [StringLength(171, ErrorMessage = "Location Street Address cannot be longer than 171 characters.")]
        public string LocationStreetAddress { get; set; }
        
        [Required(ErrorMessage = "City is required.")]
        [StringLength(58, ErrorMessage = "Location City cannot be longer than 58 characters.")]
        public string LocationCity { get; set; }
        
        [Required(ErrorMessage = "Post Code is required.")]
        [StringLength(7, ErrorMessage = "Post Code cannot be longer than 7 characters.")]
        public string LocationPostCode { get; set; }
        
        [Required(ErrorMessage = "Country is required.")]
        [StringLength(16, ErrorMessage = "Location Country cannot be longer than 16 characters.")]
        public string LocationCountry { get; set; }
    }
}