using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Defines a Permissions Repository interface.
    /// </summary>
    /// <typeparam name="Permission">The type of the permission.</typeparam>
    /// <seealso cref="IRepository{Permission, Guid}" />
    public interface IPermissionsRepository<Permission> : IRepository<Permission, Guid>
    {
        /// <summary>
        /// Alls the permissions belong to group identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="permissions">The permissions.</param>
        /// <returns>a <see cref="Task{bool}"/></returns>
        Task<bool> AllPermissionsBelongToGroupIdAsync(Guid groupId, IEnumerable<Guid> permissions);

        /// <summary>
        /// Gets the permissions child permissions
        /// </summary>
        /// <param name="permissionId">the permission id</param>
        /// <returns>a <see cref="Task"/ that contains the result of the sync operation></returns>
        Task<IEnumerable<Permission>> GetChildPermissionsAsync(Guid permissionId);
    }
}