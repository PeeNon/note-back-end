using System.ComponentModel.DataAnnotations;

namespace NotesApi.Validations
{
    public class OrderValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var order = value as string;
            if (order != null && (order.ToLower() == "asc" || order.ToLower() == "desc"))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("The order parameter must be either 'asc' or 'desc'.");
        }
    }
}