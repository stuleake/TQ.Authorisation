namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// This is a wrapper service class.
    /// Provides access to other management services.
    /// </summary>
    public class GraphApiClient : IGraphApiClient
    {
        /// <inheritdoc />
        public IGroupGraph Groups { get; }

        /// <inheritdoc />
        public IUserGraph Users { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphApiClient"/> class.
        /// </summary>
        /// <param name="groupGraph">The group graph.</param>
        /// <param name="userGraph">The user graph.</param>
        public GraphApiClient(IGroupGraph groupGraph, IUserGraph userGraph)
        {
            Groups = groupGraph;
            Users = userGraph;
        }
    }
}