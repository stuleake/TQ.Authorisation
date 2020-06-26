using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Data;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data
{
    public class PagedListTests
    {
        [Fact]
        public void ToPagedListSuccess()
        {
            // Arrange
            const int ExpectedPageSize = 2;
            const int ExpectedSourceCount = 2;

            var roleId = Guid.NewGuid();
            var roles = new List<Role>
            {
                new Role { Id = roleId },
                new Role { Id = roleId },
                new Role { Id = roleId },
                new Role { Id = roleId },
                new Role { Id = Guid.NewGuid() },
            };

            var source = roles.Where(role => role.Id == roleId);

            // Act
            var result = PagedList<Role>.ToPagedList(source.AsQueryable(), 1, ExpectedPageSize);

            // Assert
            result.Count.Should().Be(ExpectedSourceCount);
            result.PageSize.Should().Be(ExpectedPageSize);
            result.NextPageNumber.Should().Be(ExpectedPageSize + 1);
        }
    }
}