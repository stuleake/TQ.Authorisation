using FluentAssertions;
using System;
using TQ.Geocoding.Service.Extensions;
using Xunit;

namespace TQ.Geocoding.API.Test.Extensions
{
    public class TypeExtensionTests
    {
        [Fact]
        public void IsNullEmptyOrDefaultReturnsTrueWhenIsNull()
        {
            // Arrange
            string itemToValidate = null;

            // Act
            var result = itemToValidate.IsNullEmptyOrDefault();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsNullEmptyOrDefaultReturnsTrueWhenIsEmpty()
        {
            // Arrange
            string itemToValidate = string.Empty;

            // Act
            var result = itemToValidate.IsNullEmptyOrDefault();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsNullEmptyOrDefaultReturnsTrueWhenIsWhitespace()
        {
            // Arrange
            string itemToValidate = " ";

            // Act
            var result = itemToValidate.IsNullEmptyOrDefault();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsNullEmptyOrDefaultReturnsTrueWhenIsDefault()
        {
            // Arrange
            Guid itemToValidate = Guid.Empty;

            // Act
            var result = itemToValidate.IsNullEmptyOrDefault();

            // Assert
            result.Should().BeTrue();
        }
    }
}