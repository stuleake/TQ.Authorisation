using System.Collections.Generic;
using TQ.Authentication.Data.Entities;

namespace TQ.Authentication.Services.Interfaces
{
    public interface IPermissionTree
    {
        /// <summary>
        /// Gets or sets the data
        /// </summary>
        Permission Data { get; }

        /// <summary>
        /// Gets or sets the parent
        /// </summary>
        IPermissionTree Parent { get; }

        /// <summary>
        /// Gets or sets the children
        /// </summary>
        ICollection<IPermissionTree> Children { get; }
    }
}