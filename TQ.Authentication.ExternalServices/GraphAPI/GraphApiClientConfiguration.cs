using System.Diagnostics.CodeAnalysis;

namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Represents configuration settings for the Graph API Client.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GraphApiClientConfiguration
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tenant.
        /// </summary>
        /// <value>
        /// The name of the tenant.
        /// </value>
        public string TenantName { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the default size of the page.
        /// </summary>
        /// <value>
        /// The default size of the page.
        /// </value>
        public int DefaultPageSize { get; set; }
    }
}