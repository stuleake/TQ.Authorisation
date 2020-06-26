using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Requests.Paging;
using TQ.Authentication.Data.Entities;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Defines a Roles Repository interface.
    /// </summary>
    /// <typeparam name="Role">The type of the OLE.</typeparam>
    /// <seealso cref="IRepository{Role, Guid}" />
    public interface IRolesRepository<Role> : IRepository<Role, Guid>
    {
        /// <summary>
        /// Checks if Role name exists.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>a <see cref="Task{bool}"/></returns>
        Task<bool> RoleNameExistsAsync(Guid roleId, string roleName);

        /// <summary>
        /// Checks if Role name is unique per groupId.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>a <see cref="Task{bool}"/></returns>
        Task<bool> IsRoleNameUniquePerGroupIdAsync(Guid groupId, string roleName);

        /// <summary>
        /// Groups the and role combination exists asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>a <see cref="Task{bool}"/></returns>
        Task<bool> GroupAndRoleCombinationExistsAsync(Guid groupId, Guid roleId);

        /// <summary>
        /// Updates the role asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task UpdateRoleAsync(Role entity);

        /// <summary>
        /// Returns the roles associated with a group
        /// </summary>
        /// <param name="groupId">the group id</param>
        /// <returns>a <see cref="Task{IEnumerable<typeparamref name="Role"/>}"/></returns>
        Task<IEnumerable<Role>> GetRolesForGroupAsync(Guid groupId);

        /// <summary>
        /// Returns the role with the specified id
        /// </summary>
        /// <param name="roleId">the role identifier</param>
        /// <returns>a <see cref="Task"/ that contains the result of the async operation></returns>
        Task<Role> GetByIdAsync(Guid roleId);

        /// <summary>
        /// Gets the role and role permissions that match the specified roleId
        /// </summary>
        /// <param name="roleId">the role identifier</param>
        /// <returns>a <see cref="Task"/ that contains the result of the async operation></returns>
        Task<Role> GetByIdIncludeRolePermissionsAync(Guid roleId);

        /// <summary>
        /// Gets the group guid from the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>a <see cref="Task{Guid}"/></returns>
        Task<Guid> GetGroupIdByRoleAsync(Guid roleId);

        /// <summary>
        /// Gets the role and role permissions that match the specified roleIds
        /// </summary>
        /// <param name="roleIds">the role identifiers</param>
        /// <returns>a <see cref="Task{IEnumerable<typeparamref name="Role"/>}"/></returns>
        Task<IEnumerable<Role>> GetByIdsIncludeRolePermissionsAsync(List<Guid> roleIds);

        /// <summary>
        /// Gets the roles associated with a user and their groups asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="groupsForUser">The groups for the user.</param>
        /// <returns>a <see cref="Task"/> that contains <see cref="IEnumerable{Role}"/></returns>
        Task<IEnumerable<Role>> GetRolesByUserIdAndGroupsAsync(Guid userId, List<Group> groupsForUser);

        /// <summary>
        /// Returns the roles associated with a group
        /// </summary>
        /// <param name="groupId">the group id</param>
        /// <param name="request">the request that contains the paging parameters</param>
        /// <returns>a <see cref="{PagedList<typeparamref name="GetRoleDto"/>}"/></returns>
        PagedList<GetRoleDto> GetPagedRolesForGroupAsync(Guid groupId, GetPagedRequest request);
    }
}