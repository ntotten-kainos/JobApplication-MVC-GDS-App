using System.ComponentModel.DataAnnotations;

namespace JobApplicationMVCApp.Models
{
    public class Address
    {
        [Required]
        [StringLength(171)]
        public string Street { get; set; }

        [Required]
        [StringLength(58)]
        public string City { get; set; }

        [Required]
        [StringLength(7)]
        public string PostCode { get; set; }

        [Required]
        [StringLength(16)]
        public string Country { get; set; }
    }

}