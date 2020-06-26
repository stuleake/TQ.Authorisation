using FluentValidation.TestHelper;
using TQ.Authentication.API.Validation.Paging;
using Xunit;

namespace TQ.Authentication.UnitTests.Validators.Paging
{
    public class GetPagedRequestValidatorTests
    {
        [Fact]
        public void GetPagedRequestValidatorSuccess()
        {
            var validator = new GetPagedRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.PageSize, -1);
            validator.ShouldHaveValidationErrorFor(x => x.PageNumber, -1);

            validator.ShouldNotHaveValidationErrorFor(x => x.PageSize, 0);
            validator.ShouldNotHaveValidationErrorFor(x => x.PageNumber, 0);

            validator.ShouldNotHaveValidationErrorFor(x => x.PageSize, 1);
            validator.ShouldNotHaveValidationErrorFor(x => x.PageNumber, 1);
        }
    }
}