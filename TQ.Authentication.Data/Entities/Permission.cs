using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TQ.Authentication.Data.Entities
{
    /// <summary>
    /// Represents a Permission entity.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Permission
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public Group Group { get; set; }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the role permissions.
        /// </summary>
        /// <value>
        /// The role permissions.
        /// </value>
        public ICollection<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// Gets or sets the parent permission Id
        /// </summary>
        public Guid? ParentPermissionId { get; set; }
    }
}