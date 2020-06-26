using FluentValidation.TestHelper;
using System;
using TQ.Authentication.API.Validation.Roles;
using Xunit;

namespace TQ.Authentication.UnitTests.Validators.Roles
{
    public class CreateRoleRequestValidatorTests
    {
        [Fact]
        public void CreateRoleRequestValidatorSuccess()
        {
            var validator = new CreateRoleRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.GroupId, Guid.Empty);

            validator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Name, " ");
            validator.ShouldHaveValidationErrorFor(x => x.Name, " name");
            validator.ShouldHaveValidationErrorFor(x => x.Name, "name ");
            validator.ShouldHaveValidationErrorFor(x => x.Name, ValidatorHelpers.GetMaxLengthString(256));

            validator.ShouldHaveValidationErrorFor(x => x.Description, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Description, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Description, " ");
            validator.ShouldHaveValidationErrorFor(x => x.Description, " description");
            validator.ShouldHaveValidationErrorFor(x => x.Description, "description ");
            validator.ShouldHaveValidationErrorFor(x => x.Name, ValidatorHelpers.GetMaxLengthString(256));
        }
    }
}