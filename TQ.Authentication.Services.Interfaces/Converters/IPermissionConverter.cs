using System.Collections.Generic;
using TQ.Authentication.Core.Dto.Permissions;

namespace TQ.Authentication.Services.Interfaces.Converters
{
    public interface IPermissionConverter
    {
        /// <summary>
        /// Converts the permission tree into a <see cref="GetPermissionsDto"/>
        /// </summary>
        /// <param name="permissionTree">the permission tree to be converted</param>
        /// <returns>a <see cref="GetPermissionsDto"/ converted from the permsisions></returns>
        List<GetPermissionsDto> GetNestedPermissionsDto(IEnumerable<IPermissionTree> permissionTree);
    }
}