using System;
using System.Collections.Generic;

namespace TQ.Authentication.Core.Requests.Roles
{
    /// <summary>
    /// Represents a BaseRoleRequest
    /// </summary>
    public class BaseRoleRequest
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public IEnumerable<Guid> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        public Guid GroupId { get; set; }
    }
}