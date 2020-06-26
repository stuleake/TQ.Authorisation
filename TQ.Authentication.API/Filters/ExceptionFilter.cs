using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using TQ.Authentication.Core.Dto;
using TQ.Authentication.Core.Exceptions;

[assembly: InternalsVisibleTo("TQ.Authentication.UnitTests")]

namespace TQ.Authentication.API.Filters
{
    /// <summary>
    /// Represents an Exception Filter.
    /// </summary>
    /// <seealso cref="IActionFilter" />
    public class ExceptionFilter : IActionFilter
    {
        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext" />.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // this method is called for all requests so if there's no exception to handle, exit immediately
            if (context.Exception == null)
            {
                return;
            }

            if (context.Exception is TQAuthenticationException tqAuthenticationException)
            {
                context.Result = new ObjectResult(new ErrorResponseDto
                {
                    StatusCode = tqAuthenticationException.HttpStatusCode,
                    Messages = new List<string> { tqAuthenticationException.Message }
                })
                {
                    StatusCode = (int)tqAuthenticationException.HttpStatusCode
                };
                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is Exception exception)
            {
                context.Result = new ObjectResult(new ErrorResponseDto
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Messages = new List<string> { exception.Message }
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                context.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        { }
    }
}