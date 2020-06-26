using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Core.Dto.Permissions;

namespace TQ.Authentication.Services.Extensions
{
    public static class UserPermissionExtensions
    {
        /// <summary>
        /// Gets an anonymous object of user permissions suitable for embedding in a Bearer Token.
        /// </summary>
        /// <param name="userPermissions">The user permissions to be formatted.</param>
        /// <returns>an anonymous object of user permissions/></returns>
        public static object ToBearerTokenFormat(this IEnumerable<GroupPermissionNamesDto> userPermissions)
        {
            if (userPermissions == null)
            {
                throw new ArgumentNullException(nameof(userPermissions));
            }

            var userPermissionsCustom = new Dictionary<string, ICollection<string>>();

            foreach (var group in userPermissions)
            {
                userPermissionsCustom.Add(group.GroupName, group.PermissionNames.ToList());
            }

            var result = new
            {
                ApplicationGroup = JsonConvert.SerializeObject(userPermissionsCustom)
            };

            return result;
        }
    }
}