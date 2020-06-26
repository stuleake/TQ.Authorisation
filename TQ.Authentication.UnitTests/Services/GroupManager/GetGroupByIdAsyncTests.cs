using FluentAssertions;
using System;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.GroupManager
{
    public class GetGroupByIdAsyncTests
    {
        private readonly IGraphApiClient graphApiClient = Mock.Create<IGraphApiClient>();
        private readonly IGroupsRepository<Group> groupsRepository = Mock.Create<IGroupsRepository<Group>>();
        private readonly IUserGroupsRepository<UserGroup> userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
        private readonly IRolesRepository<Role> rolesRepository = Mock.Create<IRolesRepository<Role>>();
        private readonly IRoleConverter roleConverter = Mock.Create<IRoleConverter>();
        private readonly IAzureGroupManager azureGroupManager = Mock.Create<IAzureGroupManager>();
        private readonly IPermissionManager permissionManager = Mock.Create<IPermissionManager>();
        private readonly IPermissionConverter permissionConverter = Mock.Create<IPermissionConverter>();

        [Fact]
        public async Task GetGroupByIdAsyncThrowsGroupManagerException()
        {
            // Arrange
            Mock.Arrange(() => groupsRepository.GetAsync(Arg.AnyGuid)).TaskResult(null);
            var sut = this.CreateSut();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.GetGroupByIdAsync(Guid.NewGuid()));
            exception.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
            Mock.Assert(() => groupsRepository.GetAsync(Arg.AnyGuid), Occurs.Once());
        }

        [Fact]
        public async Task GetGroupByIdAsyncReturnsGroup()
        {
            // Arrange
            Mock.Arrange(() => groupsRepository.GetAsync(Arg.AnyGuid)).TaskResult(new Group { Id = Guid.NewGuid() });
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetGroupByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            Mock.Assert(() => groupsRepository.GetAsync(Arg.IsAny<Guid>()), Occurs.Once());
        }

        private Authentication.Services.GroupManager CreateSut()
        {
            return new Authentication.Services.GroupManager(graphApiClient,
                                                            groupsRepository,
                                                            userGroupsRepository,
                                                            rolesRepository,
                                                            roleConverter,
                                                            azureGroupManager,
                                                            permissionManager,
                                                            permissionConverter);
        }
    }
}