namespace TQ.Authentication.Core.Requests.Users
{
    /// <summary>
    /// Represents a request to update a user
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the given name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        public string Surname { get; set; }
    }
}