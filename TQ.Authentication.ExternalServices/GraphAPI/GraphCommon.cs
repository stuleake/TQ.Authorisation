using Microsoft.Graph;

namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Represents a Graph Common class.
    /// </summary>
    public class GraphCommon
    {
        // Dependencies
        protected readonly IGraphServiceClient GraphServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphCommon"/> class.
        /// </summary>
        /// <param name="graphServiceClientFactory">The graph service client factory.</param>
        public GraphCommon(IGraphServiceClientFactory graphServiceClientFactory)
        {
            GraphServiceClient = graphServiceClientFactory.Create();
        }
    }
}