using System;
using System.ComponentModel.DataAnnotations;

namespace JobApplicationMVCApp.ValidationAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("The date is required.");
            }

            if (value is DateTime dateTime)
            {
                if (dateTime > DateTime.Now)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("The date must be in the future.");
        }
    }
}