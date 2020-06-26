using FluentAssertions;
using System.Collections.Generic;
using TQ.Authentication.API.Helpers;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Data;
using Xunit;

namespace TQ.Authentication.UnitTests.Helpers
{
    public class PaginationHelpersTests
    {
        [Fact]
        public void GetPaginationHeadersSuccess()
        {
            // Arrange
            var requestPath = "https://localhost/api/v1/test";
            var requestQueryString = "?PageSize=1&PageNumber=5";
            var pageNumber = 1;

            var dtos = new List<GetRoleDto> { new GetRoleDto { } };
            PagedList<GetRoleDto> roles = new PagedList<GetRoleDto>(dtos, 1, 1, 1);

            // Act
            var result = PaginationHelpers<GetRoleDto>.GetPaginationHeaders(requestPath, requestQueryString, roles, pageNumber);

            // Assert
            result.Should().NotBeNull();
        }
    }
}