using System.Collections.Generic;

namespace TQ.Authentication.ExternalServices.GraphAPI.Models
{
    /// <summary>
    /// Represents an Azure Paged Users.
    /// </summary>
    public class AzurePagedUsers
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IEnumerable<AzureUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the next page token.
        /// </summary>
        /// <value>
        /// The next page token.
        /// </value>
        public string NextPageToken { get; set; }
    }
}