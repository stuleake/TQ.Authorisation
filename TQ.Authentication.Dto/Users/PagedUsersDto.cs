using System.Collections.Generic;

namespace TQ.Authentication.Dto.Users
{
    /// <summary>
    /// Represents a Paged Users DTO.
    /// </summary>
    public class PagedUsersDto
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IEnumerable<UserDto> Users { get; set; }

        /// <summary>
        /// Gets or sets the next page token.
        /// </summary>
        /// <value>
        /// The next page token.
        /// </value>
        public string NextPageToken { get; set; }
    }
}