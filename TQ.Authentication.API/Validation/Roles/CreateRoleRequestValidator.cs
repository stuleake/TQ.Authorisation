using FluentValidation;
using TQ.Authentication.Core.Requests.Roles;

namespace TQ.Authentication.API.Validation.Roles
{
    /// <summary>
    /// Fluent validation class for the <see cref="CreateRoleRequest"/>
    /// </summary>
    public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleRequestValidator()
        {
            RuleFor(req => req.GroupId).NotEmpty();
            RuleFor(req => req.Name).NotNull().NotEmpty().MaximumLength(255).NotStartOrEndWithWhiteSpace();
            RuleFor(req => req.Description).NotNull().NotEmpty().MaximumLength(255).NotStartOrEndWithWhiteSpace();
        }
    }
}