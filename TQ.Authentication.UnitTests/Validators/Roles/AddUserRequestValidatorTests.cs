using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using TQ.Authentication.API.Validation.Roles;
using Xunit;

namespace TQ.Authentication.UnitTests.Validators.Roles
{
    public class AddUserRequestValidatorTests
    {
        [Fact]
        public void AddUserRequestValidatorSuccess()
        {
            var validator = new AddUserRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.UserIds, null as List<Guid>);
            validator.ShouldHaveValidationErrorFor(x => x.UserIds, new List<Guid>());
            validator.ShouldHaveValidationErrorFor(x => x.UserIds, new List<Guid> { Guid.Empty, Guid.Empty });
        }
    }
}