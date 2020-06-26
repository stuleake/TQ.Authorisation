using Autofac;
using System.Diagnostics.CodeAnalysis;

namespace TQ.Authentication.Services.IoC
{
    /// <summary>
    /// Represents a Services Module.
    /// We have all service bindings in this module.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServicesModule : Module
    {
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
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsImplementedInterfaces();
        }
    }
}