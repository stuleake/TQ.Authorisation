using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using Xunit;

namespace TQ.Authentication.UnitTests.Filters.ExceptionFilter
{
    public class OnActionExecuted
    {
        [Fact]
        public void OnActionExecuted_TQAuthorisationExceptionNull_NullHttpContext()
        {
            // Arrange
            var actionExecutedContext = Mock.Create<ActionExecutedContext>();
            actionExecutedContext.Exception = null;

            var filter = new API.Filters.ExceptionFilter();

            // Act
            filter.OnActionExecuted(actionExecutedContext);
            
            // Assert
            Assert.Null(actionExecutedContext.HttpContext);
            Assert.False(actionExecutedContext.ExceptionHandled);
        }

        [Fact]
        public void OnActionExecuted_TQAuthorisationException_Handled()
        {
            // Arrange
            var actionExecutedContext = Mock.Create<ActionExecutedContext>();
            var error = HttpStatusCode.NotFound;
            var messages = $"Test error message: {(nameof(ActionExecutedContext))}";
            actionExecutedContext.Exception = new TQAuthenticationException(error, messages);

            var filter = new API.Filters.ExceptionFilter();

            // Act
            filter.OnActionExecuted(actionExecutedContext);
            var result = (ObjectResult)actionExecutedContext.Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.True(actionExecutedContext.ExceptionHandled);
        }

        [Fact]
        public void OnActionExecuted_Exception_SetsHttpCode400()
        {
            // Arrange
            var actionExecutedContext = Mock.Create<ActionExecutedContext>();
            actionExecutedContext.Exception = new Exception();

            var filter = new API.Filters.ExceptionFilter();

            // Act
            filter.OnActionExecuted(actionExecutedContext);

            var result = (ObjectResult)actionExecutedContext.Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.True(actionExecutedContext.ExceptionHandled);
        }
    }
}