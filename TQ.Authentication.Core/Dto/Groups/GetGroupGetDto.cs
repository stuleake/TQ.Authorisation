using System;
using System.Collections.Generic;
using TQ.Authentication.Core.Dto.Roles;

namespace TQ.Authentication.Core.Dto.Groups
{
    /// <summary>
    /// Represents a Get Group DTO.
    /// </summary>
    public class GetGroupGetDto
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the service URL.
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the roles associated with the group
        /// </summary>
        public IEnumerable<GetRoleDto> Roles { get; set; }
    }
}