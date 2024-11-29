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

        [Required] public Address Address { get; set; } // Complex type
    }
}