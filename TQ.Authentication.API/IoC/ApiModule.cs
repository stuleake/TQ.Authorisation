using Autofac;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using TQ.Authentication.ExternalServices.GraphAPI;

namespace TQ.Authentication.API.IoC
{
    /// <summary>
    /// Represents an API Module.
    /// We have all API bindings in this module.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiModule : Module
    {
        // Dependencies
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiModule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ApiModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            var graphApiClientConfiguration = new GraphApiClientConfiguration();

            configuration.Bind(nameof(GraphApiClientConfiguration), graphApiClientConfiguration);

            builder.RegisterInstance(graphApiClientConfiguration).SingleInstance();
        }
    }
}