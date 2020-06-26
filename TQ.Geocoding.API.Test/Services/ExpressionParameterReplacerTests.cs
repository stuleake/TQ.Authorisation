using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using TQ.Geocoding.Service;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class ExpressionParameterReplacerTests
    {
        [Fact]
        public void ExpressionParameterReplacerThrowsArgumentExceptionWhenFromParametersIsNull()
        {
            // Arrange, Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => new ExpressionParameterReplacer(null, null));
        }

        [Fact]
        public void ExpressionParameterReplacerThrowsArgumentExceptionWhenToParametersIsNull()
        {
            // Arrange
            var fromParameters = new List<ParameterExpression> { Expression.Parameter(typeof(int)) };
            
            // Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => new ExpressionParameterReplacer(fromParameters , null));
        }

        [Fact]
        public void ExpressionParameterReplacerThrowsArgumentExceptionWhenFromParametersIsEmpty()
        {
            // Arrange, Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => new ExpressionParameterReplacer(new List<ParameterExpression>(), null));
        }

        [Fact]
        public void ExpressionParameterReplacerThrowsArgumentExceptionWhenToParametersIsEmpty()
        {
            // Arrange
            var fromParameters = new List<ParameterExpression> { Expression.Parameter(typeof(int)) };

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => new ExpressionParameterReplacer(fromParameters, new List<ParameterExpression>()));
        }

        [Fact]
        public void ExpressionParameterReplacerThrowsArgumentExceptionWhenToParametersAndFromParametersNotEqual()
        {
            // Arrange
            var fromParameters = new List<ParameterExpression> { Expression.Parameter(typeof(int))};
            var toParameters = new List<ParameterExpression> { Expression.Parameter(typeof(int)), Expression.Parameter(typeof(int)) };

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentException>(() => new ExpressionParameterReplacer(fromParameters, toParameters));
        }

        [Fact]
        public void ExpressionParameterReplacerSuccess()
        {
            // Arrange
            var fromParameters = new List<ParameterExpression> { Expression.Parameter(typeof(int)), Expression.Parameter(typeof(int)) };
            var toParameters = new List<ParameterExpression> { Expression.Parameter(typeof(int)), Expression.Parameter(typeof(int)) };

            // Act
            var result = new ExpressionParameterReplacer(fromParameters, toParameters);

            // Assert
            result.Should().NotBeNull();
        }
    }
}