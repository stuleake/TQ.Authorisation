using FluentValidation;
using TQ.Authentication.Core.Requests.Users;

namespace TQ.Authentication.API.Validation.Users
{
    /// <summary>
    /// Fluent validation class for the <see cref="UpdateUserRequest"/>
    /// </summary>
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserRequestValidator"/> class.
        /// </summary>
        public UpdateUserRequestValidator()
        {
            RuleFor(req => req.DisplayName).NotNull().NotEmpty().NotStartOrEndWithWhiteSpace();
            RuleFor(req => req.GivenName).NotNull().NotEmpty().NotStartOrEndWithWhiteSpace();
            RuleFor(req => req.Surname).NotNull().NotEmpty().NotStartOrEndWithWhiteSpace();
        }
    }
}