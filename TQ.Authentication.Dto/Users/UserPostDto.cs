using System.Diagnostics.CodeAnalysis;

namespace TQ.Authentication.Dto.Users
{
    /// <summary>
    /// Represents a User POST DTO.
    /// </summary>
    /// <seealso cref="TQ.Authentication.Dto.Users.UserDto" />
    [ExcludeFromCodeCoverage]
    public class UserPostDto : UserDto
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }
    }
}