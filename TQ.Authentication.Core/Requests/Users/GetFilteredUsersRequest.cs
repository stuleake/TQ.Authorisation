namespace TQ.Authentication.Core.Requests.Users
{
    /// <summary>
    /// Represents the Get filtered users request
    /// </summary>
    public class GetFilteredUsersRequest
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        public int PageSize { get; set; } = 100;

        /// <summary>
        /// Gets or sets the next page token.
        /// </summary>
        public string NextPageToken { get; set; }
    }
}