using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Service.Extensions;

namespace TQ.Geocoding.Service.Validators
{
    public class IsPostcodeValidator : ValidatorBase<string>
    {
        private readonly List<string> errorMessages = new List<string>();

        public IsPostcodeValidator(string context)
        : base(context)
        {
        }

        public override bool IsValid
        {
            get
            {
                if (!Context.IsPostCode())
                {
                    errorMessages.Add($"{Context} is not a valid postcode");
                }
                
                return !errorMessages.Any();
            }
        }

        public override string ErrorMessage
        {
            get
            {
                return errorMessages.Any() ? string.Join(",", errorMessages) : string.Empty;
            }
        }
    }
}