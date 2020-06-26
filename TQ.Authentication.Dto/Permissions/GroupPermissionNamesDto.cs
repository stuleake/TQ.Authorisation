using System.Collections.Generic;

namespace TQ.Authentication.Dto.Permissions
{
    /// <summary>
    /// Represents a Group Permission Names Dto.
    /// </summary>
    public class GroupPermissionNamesDto
    {
        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        /// <value>
        /// The group name.
        /// </value>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the permission names.
        /// </summary>
        /// <value>
        /// An <see cref="IEnumerable{string}"></see> of permission names 
        /// </value>
        public IEnumerable<string> PermissionNames { get; set; }
    }
}