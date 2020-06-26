using System.Collections.Generic;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Core.Dto.Permissions;

namespace TQ.Authentication.Core.Dto.Roles
{
    /// <summary>
    /// Represents a Get Role Group Permission DTO.
    /// </summary>
    public class GetRoleGroupPermissionDto
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Group
        /// </summary>
        public GroupDto Group { get; set; }

        /// <summary>
        /// Gets or sets the permissions
        /// </summary>
        public IEnumerable<GetPermissionsDto> Permissions { get; set; }
    }
}