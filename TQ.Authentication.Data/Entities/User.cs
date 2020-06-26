using System;
using System.Collections.Generic;

namespace TQ.Authentication.Data.Entities
{
    /// <summary>
    /// Represents a User entity.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        /// <value>
        /// The user roles.
        /// </value>
        public ICollection<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the user groups.
        /// </summary>
        /// <value>
        /// The user groups.
        /// </value>
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}