using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Authentication.Services.Interfaces
{
    public interface IUserRolesManager
    {
        /// <summary>
        /// Compares the user roles asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<Guid> CompareUserRolesAsync(Dictionary<Guid, List<Guid>> model);
    }
}