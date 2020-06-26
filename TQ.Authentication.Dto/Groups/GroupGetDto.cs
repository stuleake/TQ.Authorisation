using System;
using System.Collections.Generic;
using TQ.Authentication.Dto.Roles;

namespace TQ.Authentication.Dto.Groups
{
    /// <summary>
    /// Represents a GroupGET DTO.
    /// </summary>
    public class GroupGetDto
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
        public IEnumerable<RoleGetDto> Roles { get; set; }
    }
}