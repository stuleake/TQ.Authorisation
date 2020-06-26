using FluentValidation;
using TQ.Authentication.Core.Requests.Users;

namespace TQ.Authentication.API.Validation.Users
{
    /// <summary>
    /// Fluent validation class for the <see cref="GetFilteredUsersRequest"/>
    /// </summary>
    public class GetFilteredUsersRequestValidator : AbstractValidator<GetFilteredUsersRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilteredUsersRequestValidator"/> class.
        /// </summary>
        public GetFilteredUsersRequestValidator()
        {
            RuleFor(req => req.PageSize).GreaterThan(0);
            RuleFor(req => req.EmailAddress).EmailAddress();
        }
    }
}