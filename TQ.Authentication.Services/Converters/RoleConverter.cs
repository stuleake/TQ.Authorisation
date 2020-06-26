using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces.Converters;

namespace TQ.Authentication.Services.Converters
{
    public class RoleConverter : IRoleConverter
    {
        /// <inheritdoc/>
        public IEnumerable<GetRoleDto> ToRoleGetDtos(IEnumerable<Role> roles)
        {
            if (roles?.Any() != true)
            {
                return Enumerable.Empty<GetRoleDto>();
            }

            return roles.Select(role => ToRoleGetDto(role)).ToList();
        }

        /// <inheritdoc/>
        public GetRoleGroupPermissionDto ToRoleGroupPermissionGetDto(Role role, Group group, AzureGroup azureGroup, List<GetPermissionsDto> permissionsDtos)
        {
            return new GetRoleGroupPermissionDto
            {
                Name = role.Name,
                Description = role.Description,
                Group = new GroupDto
                {
                    Id = azureGroup.Id ?? default,
                    CreatedAt = azureGroup.CreatedAt ?? default,
                    ServiceUrl = group.ServiceUrl,
                    IsActive = group.IsActive,
                    Name = azureGroup.Name,
                    Description = azureGroup.Description
                },
                Permissions = permissionsDtos
            };
        }

        private GetRoleDto ToRoleGetDto(Role role)
        {
            return new GetRoleDto
            {
                Name = role.Name,
                Description = role.Description
            };
        }
    }
}