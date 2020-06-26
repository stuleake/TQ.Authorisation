using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TQ.Authentication.API.Filters;
using TQ.Authentication.API.IoC;
using TQ.Authentication.Core.Dto;
using TQ.Authentication.Data.IoC;
using TQ.Authentication.ExternalServices.IoC;
using TQ.Authentication.Services.IoC;

namespace TQ.Authentication.API
{
    /// <summary>
    /// Represents the Startup class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["AzureAd:Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = new List<string>
                        {
                            Configuration["AzureAd:AppIdUri"],
                            Configuration["AzureAd:ClientId"]
                        },
                        ValidIssuer = Configuration["AzureAd:ValidIssuer"],

                        // Without these settings it doesn't validate the lifetime of the token
                        ValidateLifetime = true,
                        LifetimeValidator = ValidateTokenLifetime,
                        ClockSkew = TimeSpan.Zero
                    };
#if DEBUG
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            return Task.FromResult(context.Exception.Message);
                        }
                    };
#endif
                });
            // Configure swagger settings
            const string bearer = "Bearer";
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(bearer, new OpenApiSecurityScheme
                {
                    Description = $"JWT Authorization header using the {bearer} scheme. Example: \"Authorization: {bearer} {{token}}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = bearer
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = bearer
                            },
                            Scheme = "oauth2",
                            Name = bearer,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TQ.Authentication.API", Version = "v1" });
                options.EnableAnnotations();
                options.CustomSchemaIds(type => type.FullName);
            });

            services.AddAuthorization();

            // Add controller support
            services.AddControllers(options =>
            {
                // By default, all controllers require authentication
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
                options.Filters.Add(new AuthorizeFilter(policy));

                options.Filters.Add(new ExceptionFilter());
            })
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Startup>();
                config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                config.ImplicitlyValidateChildProperties = true;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // Customise Validation Error Responses
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .Where(modelStateEntry => modelStateEntry.Errors.Count > 0)
                        .SelectMany(modelStateEntry => modelStateEntry.Errors)
                        .Select(modelError => modelError.ErrorMessage);

                    return new BadRequestObjectResult(new ErrorResponseDto
                    {
                        StatusCode = (HttpStatusCode)StatusCodes.Status400BadRequest,
                        Messages = errors
                    });
                };
            });
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Load Autofac modules
            LoadModules(builder);
        }

        /// <summary>
        /// Loads the modules.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void LoadModules(ContainerBuilder builder)
        {
            // Register API module
            builder.RegisterModule(new ApiModule(Configuration));

            // Register Services module
            builder.RegisterModule(new ServicesModule());

            // Register External Services module
            builder.RegisterModule(new ExternalServicesModule());

            // Register Data module
            builder.RegisterModule(new DataModule(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                options.RoutePrefix = "swagger";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Validates the token lifetime.
        /// </summary>
        /// <param name="notBefore">The not before.</param>
        /// <param name="expires">The expires.</param>
        /// <param name="securityToken">The security token.</param>
        /// <param name="validationParameters">The validation parameters.</param>
        /// <returns>a <see cref="bool"/></returns>
        private bool ValidateTokenLifetime(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            var isValid = false;

            var now = DateTime.Now.ToUniversalTime();

            if (securityToken.ValidFrom <= now && now <= securityToken.ValidTo)
            {
                isValid = true;
            }

            // Return
            return isValid;
        }
    }
}