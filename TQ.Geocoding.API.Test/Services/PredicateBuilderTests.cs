using System;
using System.Collections.Generic;
using FluentAssertions;
using Telerik.JustMock;
using TQ.Geocoding.Service;
using TQ.Geocoding.Service.Builders;
using TQ.Geocoding.Service.Interface.Builders;
using TQ.Geocoding.Service.Interface.Helpers;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class PredicateBuilderTests
    {
        private readonly IPredicateHelper mockPredicateHelper = Mock.Create<IPredicateHelper>();
        private readonly List<int> numericSearchTerms = new List<int>() { 18, 2, 101, 4 };
        private const string SearchString = "REST BAY COURT";
        private const string NumericSearchString = "18 2 101 4";

        [Fact]
        public void BuildPredicateThrowsArgumentNullException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => sut.BuildPredicate(string.Empty));
        }

        [Fact]
        public void BuildWelshPredicateThrowsArgumentNullException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => sut.BuildWelshPredicate(string.Empty));
        }

        [Fact]
        public void BuildPredicateNumericExpressionSuccess()
        {
            //Arrange 
            var expectedSearchTerms = new SearchTerms()
            {
                NumericSearchTerms = this.numericSearchTerms
            };

            Mock.Arrange(() => mockPredicateHelper.SaveSearchStringAsSearchTerms(NumericSearchString)).Returns(expectedSearchTerms).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = sut.BuildPredicate(NumericSearchString);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void BuildPredicateStringExpressionSuccess()
        {
            //Arrange 
            var expectedSearchTerms = new SearchTerms()
            {
                StringSearchTerms = SearchString,
            };

            Mock.Arrange(() => mockPredicateHelper.SaveSearchStringAsSearchTerms(SearchString)).Returns(expectedSearchTerms).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = sut.BuildPredicate(SearchString);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void BuildPredicateNumericAndStringExpressionSuccess()
        {
            //Arrange 
            var expectedSearchTerms = new SearchTerms()
            {
                StringSearchTerms = SearchString,
                NumericSearchTerms = this.numericSearchTerms
            };

            Mock.Arrange(() => mockPredicateHelper.SaveSearchStringAsSearchTerms(SearchString)).Returns(expectedSearchTerms).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = sut.BuildPredicate(SearchString);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void BuildWelshPredicateNumericExpressionSuccess()
        {
            //Arrange 
            var expectedSearchTerms = new SearchTerms()
            {
                StringSearchTerms = null,
                NumericSearchTerms = this.numericSearchTerms
            };

            Mock.Arrange(() => mockPredicateHelper.SaveSearchStringAsSearchTerms(NumericSearchString)).Returns(expectedSearchTerms).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = sut.BuildWelshPredicate(NumericSearchString);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void BuildWelshPredicateStringExpressionSuccess()
        {
            //Arrange 
            var expectedSearchTerms = new SearchTerms()
            {
                StringSearchTerms = SearchString,
                NumericSearchTerms = null
            };

            Mock.Arrange(() => mockPredicateHelper.SaveSearchStringAsSearchTerms(SearchString)).Returns(expectedSearchTerms).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = sut.BuildWelshPredicate(SearchString);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void BuildWelshPredicateNumericAndStringExpressionSuccess()
        {
            //Arrange 
            var expectedSearchTerms = new SearchTerms()
            {
                StringSearchTerms = SearchString,
                NumericSearchTerms = this.numericSearchTerms
            };

            Mock.Arrange(() => mockPredicateHelper.SaveSearchStringAsSearchTerms(SearchString)).Returns(expectedSearchTerms).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = sut.BuildWelshPredicate(SearchString);

            // Assert
            result.Should().NotBeNull();
        }

        private IPredicateBuilder CreateSut()
        {
            return new PredicateBuilder(this.mockPredicateHelper);
        }
    }
}