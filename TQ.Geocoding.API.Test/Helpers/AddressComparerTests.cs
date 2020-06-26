using FluentAssertions;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Service.Helpers;
using Xunit;

namespace TQ.Geocoding.API.Test.Helpers
{
    public class AddressComparerTests
    {
        [Fact]
        public void AddressComparerSimpleCompareSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "8 MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "7 MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AddressComparerFullCompareSuccess()
        {
            // Arrange
            FullAddressDto thisAddress = new FullAddressDto()
            {
                SingleLineAddress = "8 MARSTON STREET, OXFORD, OX4 1JU"
            };

            FullAddressDto otherAddress = new FullAddressDto()
            {
                SingleLineAddress = "7 MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AddressComparerSimpleWelshCompareSuccess()
        {
            // Arrange
            SimpleWelshAddressDto thisAddress = new SimpleWelshAddressDto()
            {
                SingleLineAddress = "67 CORPORATION ROAD, CARDIFF, CF11 7AP"
            };

            SimpleWelshAddressDto otherAddress = new SimpleWelshAddressDto()
            {
                SingleLineAddress = "65 CORPORATION ROAD, CARDIFF, CF11 7AP"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AddressComparerFullWelshCompareSuccess()
        {
            // Arrange
            FullWelshAddressDto thisAddress = new FullWelshAddressDto()
            {
                SingleLineAddress = "67 CORPORATION ROAD, CARDIFF, CF11 7AP"
            };

            FullWelshAddressDto otherAddress = new FullWelshAddressDto()
            {
                SingleLineAddress = "65 CORPORATION ROAD, CARDIFF, CF11 7AP"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AddressComparerAFollowsBWithNumberSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "8 MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "7 MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AddressComparerAPrecedesBWithNumberSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "3 MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "4 MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void AddressComparerAEqualsBWithNumberSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "1 MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "1 MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().Be(0);
        }

        [Fact]
        public void AddressComparerAFollowsBWithNumberAndLetterSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "1B MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "1A MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AddressComparerAPrecedesBWithNumberAndLetterSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "1C MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "1D MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void AddressComparerAEqualsBWithNumberAndLetterSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "5E MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "5E MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().Be(0);
        }

        [Fact]
        public void AddressComparerAFollowsBWithLetterSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "FLAT 3, 30 MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "FLAT 2, 30 MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AddressComparerAPrecedesBWithLetterSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "FLAT 2, 30 MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "FLAT 3, 30 MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void AddressComparerAEqualsBWithLetterSuccess()
        {
            // Arrange
            SimpleAddressDto thisAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "FLAT 8, MARSTON STREET, OXFORD, OX4 1JU"
            };

            SimpleAddressDto otherAddress = new SimpleAddressDto()
            {
                SingleLineAddress = "FLAT 8, MARSTON STREET, OXFORD, OX4 1JU"
            };

            var sut = CreateSut();

            // Act
            var actual = sut.Compare(thisAddress, otherAddress);

            // Assert
            actual.Should().Be(0);
        }

        private AddressComparerHelper CreateSut()
        {
            return new AddressComparerHelper();
        }
    }
}