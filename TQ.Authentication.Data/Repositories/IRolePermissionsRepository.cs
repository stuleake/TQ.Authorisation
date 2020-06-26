using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Defines a Role Permissions Repository interface.
    /// </summary>
    /// <seealso cref="IRepository{Entities.RolePermission, Guid}" />
    public interface IRolePermissionsRepository<RolePermission> : IRepository<RolePermission, Guid>
    {
        /// <summary>
        /// Creates the multiple asynchronous.
        /// </summary>
        /// <param name="rolePermissions">The role permissions.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task CreateMultipleAsync(IEnumerable<RolePermission> rolePermissions);

        /// <summary>
        /// Removes the multiple asynchronous.
        /// </summary>
        /// <param name="rolePermissions">The role permissions.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task RemoveMultipleAsync(IEnumerable<RolePermission> rolePermissions);

        /// <summary>
        /// Updates the multiple asynchronous.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="rolePermissions">The role permissions.</param>
        /// <remarks>
        /// It is possible to pass in the empty list of permissions.
        /// That would get all existing permissions unassigned.
        /// </remarks>
        /// <returns>a <see cref="Task"/></returns>
        Task UpdateMultipleAsync(Guid roleId, IEnumerable<RolePermission> rolePermissions);
    }
}