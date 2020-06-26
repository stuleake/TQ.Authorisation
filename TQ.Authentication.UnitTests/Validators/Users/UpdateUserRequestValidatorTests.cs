using FluentValidation.TestHelper;
using TQ.Authentication.API.Validation.Users;
using Xunit;

namespace TQ.Authentication.UnitTests.Validators
{
    public class UpdateUserRequestValidatorTests
    {
        [Fact]
        public void UpdateUserRequestValidatorSuccess()
        {
            var validator = new UpdateUserRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.GivenName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.GivenName, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.GivenName, " ");
            validator.ShouldHaveValidationErrorFor(x => x.GivenName, "Given name ");
            validator.ShouldHaveValidationErrorFor(x => x.GivenName, " Given name");

            validator.ShouldHaveValidationErrorFor(x => x.Surname, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Surname, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Surname, " ");
            validator.ShouldHaveValidationErrorFor(x => x.Surname, "Given name ");
            validator.ShouldHaveValidationErrorFor(x => x.Surname, " Given name");
        }
    }
}