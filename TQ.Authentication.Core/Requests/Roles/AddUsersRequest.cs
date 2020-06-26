using System;
using System.Collections.Generic;

namespace TQ.Authentication.Core.Requests.Roles
{
    /// <summary>
    /// Represents a request to add users to a role
    /// </summary>
    public class AddUsersRequest
    {
        /// <summary>
        /// Gets or sets the user ids
        /// </summary>
        public List<Guid> UserIds { get; set; }
    }
}