namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Defines a Graph API Client interface.
    /// Performs various operations in the Azure AD (B2C) service.
    /// </summary>
    public interface IGraphApiClient
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        /// <returns>a <see cref="IUserGraph"/></returns>
        public IUserGraph Users { get; }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        /// <returns>a <see cref="IGroupGraph"/></returns>
        public IGroupGraph Groups { get; }
    }
}