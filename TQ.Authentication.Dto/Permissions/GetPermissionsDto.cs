using System;
using System.Collections.Generic;

namespace TQ.Authentication.Dto.Permissions
{
    public class GetPermissionsDto
    {

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the child permissions
        /// </summary>
        public List<GetPermissionsDto> Children { get; set; }
    }
}