using Microsoft.AspNetCore.Mvc.Filters;
using Telerik.JustMock;
using Xunit;

namespace TQ.Authentication.UnitTests.Filters.ExceptionFilter
{
    public class OnActionExecuting
    {
        [Fact]
        public void OnActionExecuting_Test()
        {
            // Arrange
            var actionExecutingContext = Mock.Create<ActionExecutingContext>();

            var filter = new API.Filters.ExceptionFilter();

            // Act
            filter.OnActionExecuting(actionExecutingContext);
        }
    }
}