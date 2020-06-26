using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.JustMock;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Helpers;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Helpers;
using Xunit;

namespace TQ.Geocoding.API.Test.Helpers
{
    public class LocationAuditLoggerHelperTests
    {
        private readonly ILogger<LocationAuditLoggerHelper> mockLogger = Mock.Create<ILogger<LocationAuditLoggerHelper>>();
        private readonly IAuditLogger<LocationSearchAuditLog> mockAuditLogger = Mock.Create<IAuditLogger<LocationSearchAuditLog>>();

        [Fact]
        public async void LogAsyncThrowsArgumentNullExceptionWhenSearchParametersIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.LogAsync(null, null));
        }

        [Fact]
        public async void LogAsyncThrowsArgumentNullExceptionWhenSearchParametersAreEmpty()
        {
            // Arrange
            var searchParameters = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.LogAsync(searchParameters, null));
        }

        [Fact]
        public async void LogAsyncLogsException()
        {
            // Arrange
            var postcode = "test postcode search";
            var searchParameters = new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(postcode), postcode } });

            Mock.Arrange(() => mockAuditLogger.LogAsync(Arg.IsAny<LocationSearchAuditLog>())).Throws(new Exception($"Address audit log error.  Search parameters: {string.Join(";", searchParameters.Select(sp => sp.Key + "=" + sp.Value.ToString()))}"));

            var sut = this.CreateSut();

            // Act
            await sut.LogAsync(searchParameters, 0);

            // Assert
            Mock.Assert(() => mockAuditLogger.LogAsync(Arg.IsAny<LocationSearchAuditLog>()), Occurs.Once());
        }

        [Fact]
        public async void LogAsyncAuccess()
        {
            // Arrange
            var postcode = "test postcode search";
            var searchParameters = new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(postcode), postcode } });

            Mock.Arrange(() => mockAuditLogger.LogAsync(Arg.IsAny<LocationSearchAuditLog>()));

            var sut = this.CreateSut();

            // Act
            await sut.LogAsync(searchParameters, 0);

            // Assert
            Mock.Assert(() => mockAuditLogger.LogAsync(Arg.IsAny<LocationSearchAuditLog>()), Occurs.Once());
        }

        private IAuditLoggerHelper CreateSut()
        {
            return new LocationAuditLoggerHelper(this.mockLogger, this.mockAuditLogger);
        }
    }
}