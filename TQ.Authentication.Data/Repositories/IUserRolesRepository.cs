using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Defines a User Roles Repository interface.
    /// </summary>
    /// <typeparam name="UserRole">The type of the group.</typeparam>
    /// <seealso cref="IUserRolesRepository{UserRole, Guid}" />
    public interface IUserRolesRepository<UserRole> : IRepository<UserRole, Guid>
    {
        Task<Guid?> IsUserAlreadyAssignedToRoleAsync(Guid roleId, List<Guid> model);
    }
}