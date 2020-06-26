using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;

namespace TQ.Authentication.UnitTests.Data
{
    public class InMemoryDbContextFactory
    {
        public readonly Guid group1 = Guid.NewGuid();
        public readonly Guid group2 = Guid.NewGuid();
        public readonly Guid group3 = Guid.NewGuid();
        public readonly Guid group4 = Guid.NewGuid();

        public readonly Guid user1 = Guid.NewGuid();
        public readonly Guid user2 = Guid.NewGuid();
        public readonly Guid user3 = Guid.NewGuid();
        public readonly Guid user4 = Guid.NewGuid();

        public readonly Guid permission1 = Guid.NewGuid();
        public readonly Guid permission2 = Guid.NewGuid();
        public readonly Guid permission3 = Guid.NewGuid();
        public readonly Guid permission4 = Guid.NewGuid();

        public readonly Guid role1 = Guid.NewGuid();
        public readonly Guid role2 = Guid.NewGuid();
        public readonly Guid role3 = Guid.NewGuid();
        public readonly Guid role4 = Guid.NewGuid();

        private readonly ApplicationDbContext context;

        public InMemoryDbContextFactory(bool seedData = true)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;

            context = new ApplicationDbContext(options);

            if (seedData)
            {
                AddGroupsToContext();
                AddUsersToContext();
                AddUserGroupsToContext();
                AddPermissionsToContext();
                AddRolesToContext();
                AddRolePermissionsToContext();
                AddUserRolesToContext();
            }
        }

        public ApplicationDbContext GetApplicationDbContext()
        {
            return context;
        }

        #region Seed data

        private void AddGroupsToContext()
        {
            var groups = new List<Group>
            {
                new Group { Id = group1, ServiceUrl = "https://localhost", IsActive = true },
                new Group { Id = group2, ServiceUrl = "https://localhost", IsActive = true },
                new Group { Id = group3, ServiceUrl = "https://localhost", IsActive = true },
                new Group { Id = group4, ServiceUrl = "https://localhost", IsActive = true }
            };

            context.Groups.AddRange(groups);

            context.SaveChanges();
        }

        private void AddUsersToContext()
        {
            var users = new List<User>
            {
                new User { Id = user1 },
                new User { Id = user2 },
                new User { Id = user3 },
                new User { Id = user4 }
            };

            context.Users.AddRange(users);

            context.SaveChanges();
        }

        private void AddUserGroupsToContext()
        {
            var userGroups = new List<UserGroup>
            {
                new UserGroup { Id = Guid.NewGuid(), GroupId = group1, UserId = user1 },
                new UserGroup { Id = Guid.NewGuid(), GroupId = group2, UserId = user2 },
                new UserGroup { Id = Guid.NewGuid(), GroupId = group3, UserId = user3 },
                new UserGroup { Id = Guid.NewGuid(), GroupId = group4, UserId = user4 }
            };

            context.UserGroups.AddRange(userGroups);

            context.SaveChanges();
        }

        private void AddUserRolesToContext()
        {
            var userRoles = new List<UserRole>
            {
                new UserRole { Id = Guid.NewGuid(), RoleId = role1, UserId = user1 },
                new UserRole { Id = Guid.NewGuid(), RoleId = role2, UserId = user2 },
                new UserRole { Id = Guid.NewGuid(), RoleId = role3, UserId = user3 },
                new UserRole { Id = Guid.NewGuid(), RoleId = role4, UserId = user4 }
            };

            context.UserRoles.AddRange(userRoles);

            context.SaveChanges();
        }

        private void AddPermissionsToContext()
        {
            var permissions = new List<Permission>
            {
                new Permission { Id = permission1, Name = "Perm 1", Description = "Perm 1 Description", GroupId = group1 },
                new Permission { Id = permission2, Name = "Perm 2", Description = "Perm 2 Description", GroupId = group1 },
                new Permission { Id = permission3, Name = "Perm 3", Description = "Perm 3 Description", GroupId = group1 },
                new Permission { Id = permission4, Name = "Perm 4", Description = "Perm 4 Description", GroupId = group1 }
            };

            context.Permissions.AddRange(permissions);

            context.SaveChanges();
        }

        private void AddRolesToContext()
        {
            var roles = new List<Role>
            {
                new Role { Id = role1, Name = "Role 1", Description = "Role 1 Description", GroupId = group1 },
                new Role { Id = role2, Name = "Role 2", Description = "Role 2 Description", GroupId = group1 },
                new Role { Id = role3, Name = "Role 3", Description = "Role 3 Description", GroupId = group1 },
                new Role { Id = role4, Name = "Role 4", Description = "Role 4 Description", GroupId = group1 }
            };

            context.Roles.AddRange(roles);

            context.SaveChanges();
        }

        private void AddRolePermissionsToContext()
        {
            var rolePermissions = new List<RolePermission>
            {
                new RolePermission { Id = Guid.NewGuid(), RoleId = role1, PermissionId = permission1 },
                new RolePermission { Id = Guid.NewGuid(), RoleId = role1, PermissionId = permission2 },
                new RolePermission { Id = Guid.NewGuid(), RoleId = role1, PermissionId = permission3 },
                new RolePermission { Id = Guid.NewGuid(), RoleId = role1, PermissionId = permission4 }
            };

            context.RolePermissions.AddRange(rolePermissions);

            context.SaveChanges();
        }

        #endregion Seed data
    }
}