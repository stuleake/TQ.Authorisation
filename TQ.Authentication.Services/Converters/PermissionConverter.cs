using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;

namespace TQ.Authentication.Services.Converters
{
    public class PermissionConverter : IPermissionConverter
    {
        /// <inheritdoc/>
        public List<GetPermissionsDto> GetNestedPermissionsDto(IEnumerable<IPermissionTree> permissionTree)
        {
            if (permissionTree?.Any() != true)
            {
                return new List<GetPermissionsDto>();
            }

            return permissionTree.Select(entity => new GetPermissionsDto
            {
                Id = entity.Data.Id,
                Description = entity.Data.Description,
                Name = entity.Data.Name,
                Children = entity.Children.Any()
                    ? GetNestedPermissionsDto(entity.Children)
                    : new List<GetPermissionsDto>(),
            }).ToList();
        }
    }
}