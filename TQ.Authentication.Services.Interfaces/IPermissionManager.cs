using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.Data.Entities;

namespace TQ.Authentication.Services.Interfaces
{
    /// <summary>
    /// Defines a Permission Manager service interface for managing permissions.
    /// </summary>
    public interface IPermissionManager
    {
        /// <summary>
        /// Gets the user permissions.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>an anonymous <see cref="Task{object}"/> of formatted user permissions</returns>
        Task<object> GetUserPermissionsAsync(Guid userId);

        /// <summary>
        /// Gets the permission with the specified Id
        /// </summary>
        /// <param name="permissionId">the permission id</param>
        /// <returns>a <see cref="Task"/ that contains the result of the async operation></returns>
        Task<Permission> GetByIdAsync(Guid permissionId);

        /// <summary>
        /// Gets the permissions associated with the specified role.
        /// </summary>
        /// <param name="role">the role</param>
        /// <returns>a <see cref="Task"/ that contains the result of the async operation></returns>
        Task<List<Permission>> GetRolePermissions(Role role);

        /// <summary>
        /// Gets the distinct permissions associated with the specified roles.
        /// </summary>
        /// <param name="roles">the roles</param>
        /// <returns>a <see cref="Task{List{Permission}}"/></returns>
        Task<List<Permission>> GetDistinctPermissionsForRoles(List<Role> roles);
    }
}