using FluentValidation;
using TQ.Authentication.Core.Requests.Paging;

namespace TQ.Authentication.API.Validation.Paging
{
    /// <summary>
    /// Fluent validation class for the <see cref="GetPagedRequest"/>
    /// </summary>
    public class GetPagedRequestValidator : AbstractValidator<GetPagedRequest>
    {
            public GetPagedRequestValidator()
            {
            RuleFor(req => req.PageNumber).GreaterThanOrEqualTo(0);
                RuleFor(req => req.PageSize).GreaterThanOrEqualTo(0);
        }
        
    }
}