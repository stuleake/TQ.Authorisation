using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;

namespace TQ.Geocoding.API
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ApiExceptionFilter : IExceptionFilter
    {
        /// <inheritdoc/>
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            HttpResponse response = context.HttpContext.Response;

            response.StatusCode = (context.Exception is ArgumentNullException || 
                                   context.Exception is ArgumentException || 
                                   context.Exception is ArgumentOutOfRangeException) 
                                ? (int)HttpStatusCode.BadRequest
                                : (int)HttpStatusCode.InternalServerError;

            response.ContentType = "application/json";
            context.Result = new ObjectResult(new ApiErrorResponse { Message = context?.Exception?.Message });
        }
    }
}