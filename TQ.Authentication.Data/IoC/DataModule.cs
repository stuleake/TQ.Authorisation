using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;

namespace TQ.Authentication.Data.IoC
{
    /// <summary>
    /// Represents a Data Module.
    /// We have all database-related bindings in this module.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    [ExcludeFromCodeCoverage]
    public class DataModule : Module
    {
        // Dependencies
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataModule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DataModule(IConfiguration configuration)
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
            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(
                    configuration.GetConnectionString("SqlConnection"),
                    builder =>
                    {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                        // Enabling this because I had the following error:
                        // An exception has been raised that is likely due to a transient failure.
                        // Consider enabling transient error resiliency by adding 'EnableRetryOnFailure()' to the 'UseSqlServer' call.
                        // More about that:
                        // https://stackoverflow.com/questions/29840282/error-when-connect-database-continuously
                    });

                return new ApplicationDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();

            // Register repositories
            RegisterRepositories(builder);
        }

        /// <summary>
        /// Registers the repositories.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void RegisterRepositories(ContainerBuilder builder)
        {
            // Default scope when not specified is transient
            builder.RegisterType<GroupsRepository>()
                .As<IGroupsRepository<Group>>();

            builder.RegisterType<UsersRepository>()
                .As<IUsersRepository<User>>();

            builder.RegisterType<UserGroupsRepository>()
                .As<IUserGroupsRepository<UserGroup>>();

            builder.RegisterType<RolesRepository>()
                .As<IRolesRepository<Role>>();

            builder.RegisterType<RolePermissionsRepository>()
                .As<IRolePermissionsRepository<RolePermission>>();

            builder.RegisterType<PermissionsRepository>()
                .As<IPermissionsRepository<Permission>>();

            builder.RegisterType<UserRolesRepository>()
                .As<IUserRolesRepository<UserRole>>();
        }
    }
}