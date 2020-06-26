using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace TQ.Authentication.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class TQAuthenticationException : Exception
    {
        /// <summary>
        /// Gets the HttpStatusCode
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="TQAuthenticationException"/> class
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code</param>
        /// <param name="message">The status message</param>
        public TQAuthenticationException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}