namespace JobApplicationMVCApp.Models;
using System;
using System.ComponentModel.DataAnnotations;
public class Job {
    [Key]
    [MaxLength(255)] // Maximum length for the ke
    public required string Id { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Job title cannot exceed 100 characters.")]
    public required string Title { get; set; }
    
    [Required]       
    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
    public required string Location { get; set; }
    
    [Required]        
    [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters.")] 
    public required string Department { get; set; } 
    
    [Required]        
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]  
    public required string Description { get; set; }  
    
    [Required]       
    [StringLength(500, ErrorMessage = "Requirements cannot exceed 500 characters.")]     
    public required string Requirements { get; set; }   
    
    [Required]     
    [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]   
    public required string Status { get; set; }    
    
    [Required]       
    [Range(0, double.MaxValue, ErrorMessage = "Salary band must be a positive number.")]   
    public decimal SalaryBand { get; set; }    
    
    [Required]      
    [DataType(DataType.Date)] 
    public DateTime ClosingDate { get; set; }    
    public int ApplicantCount { get; set; }   
    public required string RecruiterComments { get; set; }    }