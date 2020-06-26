namespace TQ.Geocoding.Service.Interface.Helpers
{
    /// <summary>
    /// Interface for the <see cref="IAddressAuditLoggerHelper"/>
    /// </summary>
    public interface IAddressAuditLoggerHelper : IAuditLoggerHelper
    {
        // This interface is intentionally empty so that we can inject multiple implementations of IAuditLoggerHelper using the built in DI
    }
}