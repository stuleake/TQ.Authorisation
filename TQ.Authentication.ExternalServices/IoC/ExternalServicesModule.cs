using Autofac;
using System.Diagnostics.CodeAnalysis;

namespace TQ.Authentication.ExternalServices.IoC
{
    /// <summary>
    /// Represents a External Services Module.
    /// We have all external service bindings in this module.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExternalServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsImplementedInterfaces();
        }
    }
}