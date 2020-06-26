using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Diagnostics.CodeAnalysis;

namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Represents a Graph Service Client Factory.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GraphServiceClientFactory : IGraphServiceClientFactory
    {
        // Dependencies
        private readonly GraphApiClientConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphServiceClientFactory"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public GraphServiceClientFactory(GraphApiClientConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public IGraphServiceClient Create()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException($"{configuration} cannot be null.");
            }

            GraphServiceClient graphClient;

            try
            {
                // Initiate client application
                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(configuration.ClientId)
                    .WithTenantId(configuration.TenantId)
                    .WithClientSecret(configuration.ClientSecret)
                    .Build();

                // Create the auth provider
                var authProvider = new ClientCredentialProvider(confidentialClientApplication);

                // Create Graph Service Client
                graphClient = new GraphServiceClient(authProvider);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not create a Graph Service Client: {ex.Message}");
            }

            // Return
            return graphClient;
        }
    }
}