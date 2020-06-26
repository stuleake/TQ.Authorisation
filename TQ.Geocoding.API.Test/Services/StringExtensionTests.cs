using FluentAssertions;
using TQ.Geocoding.Service.Extensions;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("EC1A 1BB")]
        [InlineData("EC1A1BB")]
        [InlineData("W1A 0AX")]
        [InlineData("W1A0AX")]
        [InlineData("M1 1AE")]
        [InlineData("M11AE")]
        [InlineData("B33 8TH")]
        [InlineData("B338TH")]
        [InlineData("CR2 6XH")]
        [InlineData("CR26XH")]
        [InlineData("DN55 1PT")]
        [InlineData("DN551PT")]
        public void IsPostCodeReturnsTrueWhenInputIsPostCode(string postCode)
        {
            // Arrange
            var sut = postCode;

            // Act
            var result = sut.IsPostCode();

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("EC1A1B")]
        [InlineData("W1A_0AX")]
        [InlineData("_M1 1AE")]
        [InlineData("Hello World")]
        [InlineData("CR2 6XH!")]
        [InlineData("DN55 *PT")]
        public void IsPostCodeReturnsFalseWhenInputIsNotPostCode(string postCode)
        {
            // Arrange
            var sut = postCode;

            // Act
            var result = sut.IsPostCode();

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("EC1A1BB", "EC1A 1BB")]
        [InlineData("CR26XH", "CR2 6XH")]
        [InlineData("DN554PT", "DN55 4PT")]
        public void ToPostCodeReturnsFormattedPostCode(string postcodeToFormat, string formattedPostCode)
        {
            // Arrange
            var sut = postcodeToFormat;

            // Act
            var result = sut.ToPostCode();

            // Assert
            result.Should().Be(formattedPostCode);
        }

        [Theory]
        [InlineData("EC1A 1BB")]
        [InlineData("EC1A")]
        [InlineData("EC1A1BB1")]
        public void ToPostCodeReturnsNonFormattedPostCode(string postcodeToFormat)
        {
            // Arrange
            var sut = postcodeToFormat;

            // Act
            var result = sut.ToPostCode();

            // Assert
            result.Should().Be(postcodeToFormat);
        }

        [Theory]
        [InlineData("word")]
        [InlineData("word 1")]
        [InlineData("another string")]
        [InlineData("and another string")]
        public void IsNumberSuccessWhenInputIsString(string word)
        {
            // Arrange
            var sut = word;

            // Act
            var result = sut.IsNumber();

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("123")]
        [InlineData("123456")]
        [InlineData("1234567")]
        public void IsNumberSuccessWhenInputIsNumber(string word)
        {
            // Arrange
            var sut = word;

            // Act
            var result = sut.IsNumber();

            // Assert
            result.Should().BeTrue();
        }
    }
}