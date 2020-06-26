using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace TQ.Geocoding.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAndValidateSettings<Config>(this IServiceCollection @this, IConfiguration config) where Config : class
        {
            return @this
                    .Configure<Config>(config.GetSection(typeof(Config).Name))
                    .PostConfigure<Config>(settings =>
                    {
                        var configurationErrors = settings.GetValidationErrors().ToArray();
                        if (configurationErrors.Any())
                        {
                            throw new ApplicationException($"Found {configurationErrors.Length} configuration error(s) in {typeof(Config).Name}: {string.Join(",", configurationErrors)}");
                        }
                    });
        }
    }
}
