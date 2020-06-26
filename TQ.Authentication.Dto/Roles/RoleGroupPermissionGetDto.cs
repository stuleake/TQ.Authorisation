using System.Collections.Generic;
using TQ.Authentication.Dto.Groups;
using TQ.Authentication.Dto.Permissions;

namespace TQ.Authentication.Dto.Roles
{
    public class RoleGroupPermissionGetDto
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