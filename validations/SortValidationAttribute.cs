using System.ComponentModel.DataAnnotations;

namespace NotesApi.Validations
{
    public class SortValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var sort = value as string;
            if (sort != null && (sort.ToLower() == "title" || sort.ToLower() == "createdat" ))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("The sort parameter must be 'title' or 'createdAt'.");
        }
    }
}