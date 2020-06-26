using System.Collections.Generic;
using System.Net;

namespace TQ.Authentication.Dto
{
    /// <summary>
    /// Represents an Error Response DTO.
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        /// <value>
        /// The HTTP status code.
        /// </value>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IEnumerable<string> Messages { get; set; }
    }
}