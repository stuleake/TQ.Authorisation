using System;

namespace TQ.Authentication.Core.Dto.Roles
{
    /// <summary>
    /// Represents a Get Role DTO
    /// </summary>
    public class GetRoleDto
    {
        /// <summary>
        /// Gets or sets the role Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the role description
        /// </summary>
        public string Description { get; set; }
    }
}