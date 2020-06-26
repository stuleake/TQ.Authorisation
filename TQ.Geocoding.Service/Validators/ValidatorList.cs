using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Service.Interface.Validators;

namespace TQ.Geocoding.Service.Validators
{
    public class ValidatorList : List<IValidator>, IValidatorList
    {
        public bool IsValid
        {
            get
            {
                return this.Validate();
            }
        }

        public bool Validate()
        {
            // run validators in sequence given and bail on first failure
            return !this.Any(validator => !validator.IsValid);
        }

        public IEnumerable<string> ErrorMessages
        {
            get
            {
                return this.Where(validator => !string.IsNullOrWhiteSpace(validator.ErrorMessage))
                            .Select(validator => validator.ErrorMessage);
            }
        }
    }
}
