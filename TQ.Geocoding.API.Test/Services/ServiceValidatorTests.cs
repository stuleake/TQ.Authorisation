using FluentAssertions;
using System;
using System.Linq;
using TQ.Geocoding.Service.Validators;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class ServiceValidatorTests
    {
        [Fact]
        public void ValidateArgumentsThrowsArgumentException()
        {
            // Arrange
            var context = string.Empty;
            
            // Act and Assert
            Assert.Throws<ArgumentException>(() => ServiceValidator.ValidateArguments(new ValidatorList { new IsNullOrDefaultValidator<string>(context) }));
        }

        [Fact]
        public void ValidateArgumentsSuccess()
        {
            // Arrange
            var context = "this is valid";
            var validatorList = new ValidatorList
            {
                new IsNullOrDefaultValidator<string>(context)
            };

            // Act and Assert
            ServiceValidator.ValidateArguments(validatorList);
        }

        [Fact]
        public void TryValidateArgumentsReturnsTrue()
        {
            // Arrange
            var context = "this is valid";
            
            // Act
            var result = ServiceValidator.TryValidateArguments(new ValidatorList { new IsNullOrDefaultValidator<string>(context) }, out _);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TryValidateArgumentsReturnsFalse()
        {
            // Arrange
            var context = string.Empty;
            
            // Act
            var result = ServiceValidator.TryValidateArguments(new ValidatorList { new IsNullOrDefaultValidator<string>(context) }, out _);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void TryValidateArgumentsHasExceptionMessagesWhenFalse()
        {
            // Arrange
            var context = string.Empty;
            
            // Act
            ServiceValidator.TryValidateArguments(new ValidatorList { new IsNullOrDefaultValidator<string>(context) }, out var errors);

            // Assert
            errors.Should().NotBeEmpty();
            errors.Count().Should().Be(1);
        }
    }
}