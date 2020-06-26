using System;
using System.Collections.Generic;

namespace TQ.Geocoding.Service.Interface.Validators
{
    public interface IValidatorList
    {
        bool IsValid { get; }
        bool Validate();
        IEnumerable<string> ErrorMessages { get; }
    }
}
