using System;

namespace TQ.Authentication.Core.Dto.Groups
{
    /// <summary>
    /// Represents a User Group DTO.
    /// </summary>
    public class UserGroupDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether [user already in group].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [user already in group]; otherwise, <c>false</c>.
        /// </value>
        public bool UserAlreadyInGroup { get; set; }

        /// <summary>
        /// Gets or sets the user group identifier.
        /// </summary>
        /// <value>
        /// The user group identifier.
        /// </value>
        public Guid? UserGroupId { get; set; }
    }
}