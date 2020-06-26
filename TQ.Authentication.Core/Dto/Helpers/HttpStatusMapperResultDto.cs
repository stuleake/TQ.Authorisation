using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace TQ.Authentication.Core.Dto.Helpers
{
    /// <summary>
    /// Represents a Http Status Mapper Result DTO.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HttpStatusMapperResultDto
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        /// <value>
        /// The HTTP status code.
        /// </value>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
    }
}