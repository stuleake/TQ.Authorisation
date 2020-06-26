using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Services.Interfaces;

namespace TQ.Authentication.Services.Extensions
{
    public static class PermissionTreeExtensions
    {
        /// <summary> Converts given collection to tree. </summary>
        /// <param name="permissions">The collection items.</param>
        /// <param name="parentSelector">Expression to select parent.</param>
        public static IPermissionTree ToPermissionTree(this IList<Permission> permissions, Func<Permission, Permission, bool> parentSelector)
        {
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }

            var lookup = permissions.ToLookup(item => permissions.FirstOrDefault(parent => parentSelector(parent, item)), child => child);

            return PermissionTree.FromLookup(lookup);
        }
    }
}