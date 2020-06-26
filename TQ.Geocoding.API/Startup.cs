using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TQ.Geocoding.Data.Context.Addresses;
using TQ.Geocoding.Data.Context.Locations;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Repository.Address;
using TQ.Geocoding.Repository.Location;
using TQ.Geocoding.Service.Audit;
using TQ.Geocoding.Service.Builders;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Converters;
using TQ.Geocoding.Service.Extensions;
using TQ.Geocoding.Service.Helpers;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Builders;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Search;

namespace TQ.Geocoding.API
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment hostingEnvironment;

        public Startup(IHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            this.configuration = builder.Build();

            // Init Serilog configuration
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<ConfigurationSettings>()
               .Bind(configuration.GetSection("AppSettings"))
               .ValidateDataAnnotations();

            services.ConfigureAndValidateSettings<ConfigurationSettings>(configuration);

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Geocoding API", Version = "v1" }));
            services.ConfigureSwaggerGen(options =>
            {

                if (File.Exists(Path.Combine(hostingEnvironment.ContentRootPath, "TQ.Geocoding.API.xml")))
                {
                    Log.Logger.Information("Xml file exists at {Path}", Path.Combine(hostingEnvironment.ContentRootPath, "TQ.Geocoding.API.xml"));
                }
                else
                {
                    Log.Logger.Information("{Path} is not the location of the xml file", Path.Combine(hostingEnvironment.ContentRootPath, "TQ.Geocoding.API.xml"));
                }
            });

            services.AddTransient<IReadOnlyRepository<SearchableAddress>, AddressReadOnlyRepository<SearchableAddress>>();
            services.AddTransient<IReadOnlyRepository<vwSearchableAddress>, AddressReadOnlyRepository<vwSearchableAddress>>();
            services.AddTransient<IReadOnlyRepository<AddressSearchAuditLog>, AddressReadOnlyRepository<AddressSearchAuditLog>>();
            services.AddTransient<IWriteOnlyRepository<AddressSearchAuditLog>, AddressWriteOnlyRepository<AddressSearchAuditLog>>();

            services.AddDbContext<AddressDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AddressBase")));
            
            services.AddTransient<IAddressReadOnlyRepository<SearchableAddress>, AddressReadOnlyRepository<SearchableAddress>>();
            services.AddTransient<IAddressReadOnlyRepository<vwSearchableAddress>, AddressReadOnlyRepository<vwSearchableAddress>>();
            services.AddTransient<IAddressReadOnlyRepository<AddressSearchAuditLog>, AddressReadOnlyRepository<AddressSearchAuditLog>>();
        

            services.AddTransient<IReadOnlyRepository<Location>, LocationReadOnlyRepository<Location>>();
            services.AddTransient<IReadOnlyRepository<PostcodeCoordinates>, LocationReadOnlyRepository<PostcodeCoordinates>>();
            services.AddTransient<IReadOnlyRepository<vwSearchableLocationDetails>, LocationReadOnlyRepository<vwSearchableLocationDetails>>();
            services.AddTransient<IReadOnlyRepository<LocationSearchAuditLog>, LocationReadOnlyRepository<LocationSearchAuditLog>>();
            services.AddDbContext<LocationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("LocationBase")));
            services.AddTransient<ILocationReadOnlyRepository<Location>, LocationReadOnlyRepository<Location>>();
            services.AddTransient<ILocationReadOnlyRepository<PostcodeCoordinates>, LocationReadOnlyRepository<PostcodeCoordinates>>();
            services.AddTransient<ILocationReadOnlyRepository<vwSearchableLocationDetails>, LocationReadOnlyRepository<vwSearchableLocationDetails>>();
            services.AddTransient<ILocationReadOnlyRepository<LocationSearchAuditLog>, LocationReadOnlyRepository<LocationSearchAuditLog>>();

            services.AddTransient<ICoordinateConverter, CoordinateConverter>();
            services.AddTransient<ISearchableAddressConverter, SearchableAddressConverter>();
            services.AddTransient<ISearchableWelshAddressConverter, SearchableWelshAddressConverter>();
            services.AddTransient<IAddressEastingNorthingSearch, AddressEastingNorthingSearch>();
            services.AddTransient<IAddressUprnSearch, AddressUprnSearch>();
            services.AddTransient<IAddressTextSearch, AddressTextSearch>();
            services.AddTransient<IAddressPostcodeSearch, AddressPostcodeSearch>();
            services.AddTransient<IPredicateBuilder, PredicateBuilder>();
            services.AddTransient<IPredicateHelper, PredicateHelper>();
            services.AddTransient<IAddressLongitudeLatitudeSearch, AddressLongitudeLatitudeSearch>();
            services.AddTransient<ICoordinateHelper, CoordinateHelper>();
            services.AddTransient<IAddressCoordinateValidation, AddressCoordinateValidation>();
            services.AddTransient<ILocationPostcodeCoordinateSearch, LocationPostcodeCoordinateSearch>();
            services.AddTransient<ILocationCoordinateSearch, LocationCoordinateSearch>();
            services.AddTransient<IPostcodeCoordinateConverter, PostcodeCoordinateConverter>();
            services.AddTransient<ILocationCoordinateConverter, LocationCoordinateConverter>();

            // Audit logging
            services.AddTransient<IAddressAuditLogConverter, AddressAuditLogConverter>();
            services.AddTransient<IAuditLogger<AddressSearchAuditLog>, AuditLogger<AddressSearchAuditLog>>();
            services.AddTransient<IReadOnlyAddressAuditLogger, ReadOnlyAddressAuditLogger>();
            services.AddTransient<IAddressAuditLoggerHelper, AddressAuditLoggerHelper>();

            services.AddTransient<IWriteOnlyRepository<LocationSearchAuditLog>, LocationWriteOnlyRepository<LocationSearchAuditLog>>();
            services.AddTransient<IAuditLogger<LocationSearchAuditLog>, AuditLogger<LocationSearchAuditLog>>();
            services.AddTransient<IReadOnlyLocationAuditLogger, ReadOnlyLocationAuditLogger>();
            services.AddTransient<ILocationAuditLogConverter, LocationAuditLogConverter>();
            services.AddTransient<ILocationAuditLoggerHelper, LocationAuditLoggerHelper>();
            services.AddTransient<IWriteOnlyRepository<AddressSearchAuditLog>, AddressWriteOnlyRepository<AddressSearchAuditLog>>();
            
            services.AddSingleton(Log.Logger);
            services.AddMvc(option => {
                option.EnableEndpointRouting = false;
                option.Filters.Add(typeof(ApiExceptionFilter));
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}