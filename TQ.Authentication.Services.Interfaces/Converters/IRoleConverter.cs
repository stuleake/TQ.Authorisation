using System.Collections.Generic;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.ExternalServices.GraphAPI.Models;

namespace TQ.Authentication.Services.Interfaces.Converters
{
    /// <summary>
    /// The RoleConverter Interface
    /// </summary>
    public interface IRoleConverter
    {
        /// <summary>
        /// Converts a collection of roles to a collection of RoleGetDtos
        /// </summary>
        /// <param name="roles">the collection of roles to convert</param>
        /// <returns>a <see cref="IEnumerable{RoleGetDto}"/> converted from the roles></returns>
        IEnumerable<GetRoleDto> ToRoleGetDtos(IEnumerable<Role> roles);

        /// <summary>
        /// Returns a <see cref="GetRoleGroupPermissionDto"/>
        /// </summary>
        /// <param name="role">the role to convert</param>
        /// <param name="group">the group to convert</param>
        /// <param name="azureGroup">the azure group to convert</param>
        /// <param name="permissionsDtos">the collection of <see cref="GetPermissionsDto"/>to convert</param>
        /// <returns>a <see cref="GetRoleGroupPermissionDto"/></returns>
        GetRoleGroupPermissionDto ToRoleGroupPermissionGetDto(Role role, Group group, AzureGroup azureGroup, List<GetPermissionsDto> permissionsDtos);
    }
}