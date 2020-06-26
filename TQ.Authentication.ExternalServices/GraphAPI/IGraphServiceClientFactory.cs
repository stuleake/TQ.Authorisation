using Microsoft.Graph;

namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Defines a Graph Service Client Factory interface.
    /// </summary>
    public interface IGraphServiceClientFactory
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>a <see cref="IGraphServiceClient"/></returns>
        IGraphServiceClient Create();
    }
}