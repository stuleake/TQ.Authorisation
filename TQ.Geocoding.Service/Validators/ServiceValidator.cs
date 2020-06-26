using System;
using System.Collections.Generic;
using System.Linq;

namespace TQ.Geocoding.Service.Validators
{
    public static class ServiceValidator
    {
        /// <summary>
        /// Validates the input arguments
        /// <exception cref="ArgumentException">Thrown when any of the arguments to validate are invalid.</exception>
        /// </summary>
        /// <param name="validators">the list of validators</param>
        public static void ValidateArguments(ValidatorList validators)
        {
            if (!validators.IsValid)
            {
                throw new ArgumentException(validators.ErrorMessages.FirstOrDefault());
            }
        }

        /// <summary>
        /// Validates the input arguments
        /// </summary>
        /// <param name="validators">the list of validators</param>
        /// <param name="errorMessages">if validation fails this will contain the error messages</param>
        /// <returns>true if the arguments are valid, false otherwise</returns>
        public static bool TryValidateArguments(ValidatorList validators, out IEnumerable<string> errorMessages)
        {
            errorMessages = Enumerable.Empty<string>();
            bool isValid = validators.IsValid;

            if (!isValid)
            {
                errorMessages = validators.ErrorMessages;
            }

            return isValid;
        }
    }
}