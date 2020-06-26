using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TQ.Authentication.Core.Dto.Permissions
{
    [ExcludeFromCodeCoverage]
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