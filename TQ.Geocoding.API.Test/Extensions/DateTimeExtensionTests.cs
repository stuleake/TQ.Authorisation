using FluentAssertions;
using System;
using TQ.Geocoding.Service.Extensions;
using Xunit;

namespace TQ.Geocoding.API.Test.Extensions
{
    public class DateTimeExtensionTests
    {
       [Fact]
       public void EndOfDayReturnsEndOfDay()
        {
            // Arrange
            var expectedTimeSpan = new TimeSpan(0, 23, 59, 59, 999);

            // Act
            var actualDate = new DateTime(2020, 04, 22).EndOfDay();

            // Assert
            actualDate.TimeOfDay.Should().Be(expectedTimeSpan);
        }
    }
}