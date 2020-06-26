using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TQ.Geocoding.Service.Extensions
{
    public static class ValidationExtensions
    {
        public static IEnumerable<string> GetValidationErrors(this object @this)
        {
            var validationContext = new ValidationContext(@this, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(@this, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                yield return validationResult.ErrorMessage;
            }
        }
    }
}
