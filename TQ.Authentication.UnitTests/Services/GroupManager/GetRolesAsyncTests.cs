using FluentAssertions;
using System;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Core.Requests.Paging;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;
using Group = TQ.Authentication.Data.Entities.Group;

namespace TQ.Authentication.UnitTests.Services.GroupManager
{
    public class GetRolesAsyncTests
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
        public async Task GetRolesAsyncThrowsTQAuthenticationErrorWhenGroupNotFound()
        {
            // Arrange
            Mock.Arrange(() => this.groupsRepository.GetAsync(Arg.AnyGuid)).TaskResult(null);
            
            var sut = this.CreateSut();

            // Act
            var exception = await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.GetRolesAsync(Guid.NewGuid(), new GetPagedRequest()));

            // Assert
            exception.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
            Mock.Assert(() => this.groupsRepository.GetAsync(Arg.AnyGuid), Occurs.Once());
        }

        [Fact]
        public async Task GetRolesAsyncReturnsSuccess()
        {
            // Arrange
            var group = new Group { Id = Guid.NewGuid(), IsActive = true, ServiceUrl = "service url" };
            Mock.Arrange(() => this.groupsRepository.GetAsync(Arg.AnyGuid)).TaskResult(group);

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRolesAsync(Guid.NewGuid(), new GetPagedRequest());

            // Assert
            result.Should().NotBeNull();
            Mock.Assert(() => this.groupsRepository.GetAsync(Arg.AnyGuid), Occurs.Once());
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