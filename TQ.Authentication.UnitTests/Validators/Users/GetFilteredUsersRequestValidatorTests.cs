using FluentValidation.TestHelper;
using TQ.Authentication.API.Validation.Users;
using Xunit;

namespace TQ.Authentication.UnitTests.Validators
{
    public class GetFilteredUsersRequestValidatorTests
    {
        [Fact]
        public void GetFilteredUsersRequestValidatorSuccess()
        {
            var validator = new GetFilteredUsersRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.PageSize, -1);
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, "invalid_email");
        }
    }
}