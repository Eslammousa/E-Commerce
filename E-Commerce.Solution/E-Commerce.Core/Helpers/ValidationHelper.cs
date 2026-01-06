using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Core.Helpers
{
    public static class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj); // this way how can validate the model

            List<ValidationResult> validationResults = new List<ValidationResult>(); // store the List of validation error 

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }

        }
    }
}
