using System;
using System.Collections.Generic;
using FluentAssertions;
using TQ.Geocoding.Service;
using TQ.Geocoding.Service.Helpers;
using TQ.Geocoding.Service.Interface.Helpers;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class PredicateHelperTests
    {  
        [Fact]
        public void SaveSearchStringAsSearchTermsAccumulatesNumericSearchTerms()
        {
            //Arrange 
            var searchString = "18 2 101 4";

            var expectedSearchTerms = new SearchTerms()
            {
                StringSearchTerms = null,
                NumericSearchTerms = new List<int>() { 18, 2, 101, 4 }
            };

            var sut = CreateSut();

            // Act
            var result = sut.SaveSearchStringAsSearchTerms(searchString);

            // Assert
            result.Should().BeOfType(typeof(SearchTerms));
            result.Should().BeEquivalentTo(expectedSearchTerms);
        }

        [Fact]
        public void SaveSearchStringAsSearchTermsAccumulatesStringSearchTerms()
        {
            //Arrange 
            var searchString = "LIME AVE LILLINGTON LEAM";

            var expectedSearchTerms = new SearchTerms()
            {
                StringSearchTerms = "\"LIME*\" AND \"AVE*\" AND \"LILLINGTON*\" AND \"LEAM*\"",
                NumericSearchTerms = new List<int>() 
            };

            var sut = CreateSut();

            // Act
            var result = sut.SaveSearchStringAsSearchTerms(searchString);

            // Assert
            result.Should().BeOfType(typeof(SearchTerms));
            result.Should().BeEquivalentTo(expectedSearchTerms);
        }

        [Fact]
        public void SaveSearchStringAsSearchTermsAccumulatesMixedSearchTerms()
        {
            //Arrange 
            var searchString = "FLAT 2 28 ACACIA AVENUE B92";

            var expectedSearchTerms = new SearchTerms()
            {
                StringSearchTerms = "\"FLAT*\" AND \"ACACIA*\" AND \"AVENUE*\" AND \"B92*\"",
                NumericSearchTerms = new List<int>() { 2, 28 }
            };

            var sut = CreateSut();

            // Act
            var result = sut.SaveSearchStringAsSearchTerms(searchString);

            // Assert
            result.Should().BeOfType(typeof(SearchTerms));
            result.Should().BeEquivalentTo(expectedSearchTerms);
        }

        [Fact]
        public void SaveSearchStringAsSearchTermsThrowsArgumentNullExceptionWhenInputIsDefault()
        {
            // Arrange
            const string ExpectedErrorMessage = "Value cannot be null. (Parameter 'searchString is null or empty')";
            var searchString = string.Empty;
            var sut = CreateSut();

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => sut.SaveSearchStringAsSearchTerms(searchString));
            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        [Fact]
        public void SaveSearchStringAsSearchTermsReturnsSearchTermsWhenInputIsNull()
        {
            // Arrange
            var expectedStringSearchTerms = "\"search*\" AND \"string*\" AND \"with*\" AND \"spaces*\"";
            var searchString = "search string  with  spaces";
            var sut = this.CreateSut();

            // Act
            var result = sut.SaveSearchStringAsSearchTerms(searchString);

            // Assert
            result.StringSearchTerms.Should().Be(expectedStringSearchTerms);
        }

        [Fact]
        public void SaveSearchStringAsSearchTermsThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var searchString = "100100515538";
            var expectedErrorMessage = $"Specified argument was out of the range of valid values. (Parameter 'word : {searchString} is not valid')";
            var sut = CreateSut();

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.SaveSearchStringAsSearchTerms(searchString));
            ex.Message.Should().Be(expectedErrorMessage);
        }

        private IPredicateHelper CreateSut()
        {
            return new PredicateHelper();
        }
    }
}