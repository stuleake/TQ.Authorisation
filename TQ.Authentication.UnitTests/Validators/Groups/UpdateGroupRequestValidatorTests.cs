using FluentValidation.TestHelper;
using TQ.Authentication.API.Validation.Groups;
using Xunit;

namespace TQ.Authentication.UnitTests.Validators.Groups
{
    public class UpdateGroupRequestValidatorTests
    {
        [Fact]
        public void UpdateGroupRequestValidatorSuccess()
        {
            var validator = new UpdateGroupRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Name, " ");
            validator.ShouldHaveValidationErrorFor(x => x.Name, " name");
            validator.ShouldHaveValidationErrorFor(x => x.Name, "name ");
            validator.ShouldHaveValidationErrorFor(x => x.Name, ValidatorHelpers.GetMaxLengthString(122));

            validator.ShouldHaveValidationErrorFor(x => x.Description, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Description, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Description, " ");
            validator.ShouldHaveValidationErrorFor(x => x.Description, " description");
            validator.ShouldHaveValidationErrorFor(x => x.Description, "description ");
            validator.ShouldHaveValidationErrorFor(x => x.Name, ValidatorHelpers.GetMaxLengthString(1026));

            validator.ShouldHaveValidationErrorFor(x => x.ServiceUrl, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.ServiceUrl, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.ServiceUrl, " ");
            validator.ShouldHaveValidationErrorFor(x => x.ServiceUrl, " serviceUrl");
            validator.ShouldHaveValidationErrorFor(x => x.ServiceUrl, "serviceUrl ");
            validator.ShouldHaveValidationErrorFor(x => x.Name, ValidatorHelpers.GetMaxLengthString(501));
        }
    }
}