using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TQ.Authentication.Core.Dto.Roles;

namespace TQ.Authentication.Core.Dto.Groups
{
    [ExcludeFromCodeCoverage]
    public class GetRolesDto
    {
        /// <summary>
        /// Gets or sets the list of roles
        /// </summary>
        public List<GetRoleDto> Roles { get; set; }
    }
}