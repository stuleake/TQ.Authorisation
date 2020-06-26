using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Service.Extensions;

namespace TQ.Geocoding.Service.Validators
{
    public class IsNullOrDefaultValidator<T> : ValidatorBase<T> where T: IComparable
    {
        private readonly List<string> errorMessages = new List<string>();

        public IsNullOrDefaultValidator(T context)
        : base(context)
        {
        }

        public override bool IsValid
        {
            get
            {
                if (Context.IsNullEmptyOrDefault())
                {
                    errorMessages.Add($"Value cannot be null or whitespace");
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