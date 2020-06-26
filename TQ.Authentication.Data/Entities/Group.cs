using System;
using System.Collections.Generic;

namespace TQ.Authentication.Data.Entities
{
    /// <summary>
    /// Represents a Group entity.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the service URL.
        /// </summary>
        /// <value>
        /// The service URL.
        /// </value>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the user groups.
        /// </summary>
        /// <value>
        /// The user groups.
        /// </value>
        public ICollection<UserGroup> UserGroups { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public ICollection<Permission> Permissions { get; set; }
    }
}