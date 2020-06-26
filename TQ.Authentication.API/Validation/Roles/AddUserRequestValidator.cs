using FluentValidation;
using TQ.Authentication.Core.Requests.Roles;

namespace TQ.Authentication.API.Validation.Roles
{
    /// <summary>
    /// Fluent validation class for the <see cref="AddUsersRequest"/>
    /// </summary>
    public class AddUserRequestValidator : AbstractValidator<AddUsersRequest>
    {
        public AddUserRequestValidator()
        {
            RuleFor(req => req.UserIds).NotNull().NotEmpty();
            RuleForEach(req => req.UserIds).NotEmpty();
        }
    }
}