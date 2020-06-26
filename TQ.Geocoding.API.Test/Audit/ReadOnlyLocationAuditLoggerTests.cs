using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Geocoding.Api.Test;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository.Location;
using TQ.Geocoding.Service.Audit;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Converters;
using Xunit;

namespace TQ.Geocoding.API.Test.Audit
{
    public class ReadOnlyLocationAuditLoggerTests
    {
        private readonly ILocationReadOnlyRepository<LocationSearchAuditLog> mockRepo = Mock.Create<ILocationReadOnlyRepository<LocationSearchAuditLog>>();
        private readonly ILocationAuditLogConverter mockConverter = Mock.Create<ILocationAuditLogConverter>();
        private readonly IOptions<ConfigurationSettings> mockConfigurationSettings = Options.Create(TestHelper.GetConfigurationSettings());

        private readonly DateTime logDate = DateTime.Now;
        private readonly Guid logId = Guid.NewGuid();
        private readonly string locationSearch = "location search";
        private readonly int resultsCount = 100;

        private const int ItemCount = 2;

        private int maxRowCount;
        private IEnumerable<LocationSearchAuditLog> expectedModels;
        private IEnumerable<LocationSearchAuditLogDto> expectedDtos;

        [Fact]
        public async Task GetAllByResultCountRangeAsyncThrowsArgumentExceptionWhenStartCountIsNegative()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetAllByResultCountRangeAsync(-1, 1));
        }

        [Fact]
        public async Task GetAllByResultCountRangeAsyncThrowsArgumentExceptionWhenEndCountIsNegative()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetAllByResultCountRangeAsync(1, -1));
        }

        [Fact]
        public async Task GetAllByResultCountRangeAsyncThrowsArgumentExceptionWhenEndCountIsLessThanStartCount()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetAllByResultCountRangeAsync(100, 10));
        }

        [Fact]
        public async Task GetAllBySearchTextAsyncThrowsArgumentNullExceptionWhenSearchTermIsNullOrWhitespace()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetAllBySearchTextAsync(string.Empty));
        }

        [Fact]
        public async Task GetAllByDateRangeAsyncThrowsArgumentExceptionWhenStartDateIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetAllByDateRangeAsync(default, default));
        }

        [Fact]
        public async Task GetAllByDateRangeAsyncThrowsArgumentExceptionWhenEndDateIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetAllByDateRangeAsync(DateTime.Now, default));
        }

        [Fact]
        public async Task GetAllByDateRangeAsyncThrowsArgumentExceptionWhenEndDateIsBeforeStartDate()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetAllByDateRangeAsync(DateTime.Now, DateTime.Now.AddDays(-1)));
        }

        [Fact]
        public async Task GetAllByDateRangeAsyncSuccess()
        {
            // Arrange
            ArrangeExpected();

            Mock.Arrange(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<LocationSearchAuditLog, bool>>>(), maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToLocationSearchAuditLogDtos(Arg.IsAny<IEnumerable<LocationSearchAuditLog>>())).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetAllByDateRangeAsync(DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            Mock.Assert(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<LocationSearchAuditLog, bool>>>(), maxRowCount), Occurs.Once());
            Mock.Assert(() => mockConverter.ToLocationSearchAuditLogDtos(Arg.IsAny<IEnumerable<LocationSearchAuditLog>>()), Occurs.Once());

            actual.Count().Should().Be(ItemCount);
            this.AssertLocationSearchAuditLogItemsAreEqual(expectedModels.ToList(), actual.ToList());
        }

        [Fact]
        public async Task GetAllByResultCountRangeAsyncSuccess()
        {
            // Arrange
            ArrangeExpected();

            Mock.Arrange(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<LocationSearchAuditLog, bool>>>(), maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToLocationSearchAuditLogDtos(Arg.IsAny<IEnumerable<LocationSearchAuditLog>>())).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetAllByResultCountRangeAsync(0, 100);

            // Assert
            Mock.Assert(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<LocationSearchAuditLog, bool>>>(), maxRowCount), Occurs.Once());
            Mock.Assert(() => mockConverter.ToLocationSearchAuditLogDtos(Arg.IsAny<IEnumerable<LocationSearchAuditLog>>()), Occurs.Once());

            actual.Count().Should().Be(ItemCount);
            this.AssertLocationSearchAuditLogItemsAreEqual(expectedModels.ToList(), actual.ToList());
        }

        [Fact]
        public async Task GetAllBySearchTextAsyncSuccess()
        {
            // Arrange
            ArrangeExpected();

            Mock.Arrange(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<LocationSearchAuditLog, bool>>>(), maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToLocationSearchAuditLogDtos(Arg.IsAny<IEnumerable<LocationSearchAuditLog>>())).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetAllBySearchTextAsync(locationSearch);

            // Assert
            Mock.Assert(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<LocationSearchAuditLog, bool>>>(), maxRowCount), Occurs.Once());
            Mock.Assert(() => mockConverter.ToLocationSearchAuditLogDtos(Arg.IsAny<IEnumerable<LocationSearchAuditLog>>()), Occurs.Once());

            actual.Count().Should().Be(ItemCount);
            this.AssertLocationSearchAuditLogItemsAreEqual(expectedModels.ToList(), actual.ToList());
        }

        [Fact]
        public async Task GetAllAsyncSuccess()
        {
            // Arrange
            ArrangeExpected();

            Mock.Arrange(() => mockRepo.ListAsync(maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToLocationSearchAuditLogDtos(Arg.IsAny<IEnumerable<LocationSearchAuditLog>>())).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetAllAsync();

            // Assert
            Mock.Assert(() => mockRepo.ListAsync(maxRowCount), Occurs.Once());
            Mock.Assert(() => mockConverter.ToLocationSearchAuditLogDtos(Arg.IsAny<IEnumerable<LocationSearchAuditLog>>()), Occurs.Once());

            actual.Count().Should().Be(ItemCount);
            this.AssertLocationSearchAuditLogItemsAreEqual(expectedModels.ToList(), actual.ToList());
        }

        private IEnumerable<LocationSearchAuditLog> GetLocationSearchAuditLogList(int count)
        {
            var auditLogItems = new List<LocationSearchAuditLog>();

            for (var index = 1; index <= count; index++)
            {
                var auditLogItem = new LocationSearchAuditLog
                {
                    LocationSearch = locationSearch,
                    Id = logId,
                    LogDate =logDate.AddDays(index),
                    ResultCount = resultsCount,
                };

                auditLogItems.Add(auditLogItem);
            }

            return auditLogItems;
        }

        private IEnumerable<LocationSearchAuditLogDto> GetLocationSearchAuditLogDtoList(int count)
        {
            var dtos = new List<LocationSearchAuditLogDto>();

            for (var index = 1; index <= count; index++)
            {
                var dto = new LocationSearchAuditLogDto
                {
                    LocationSearch = locationSearch,
                    Id = logId,
                    LogDate = logDate.AddDays(index),
                    ResultCount = resultsCount
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        private void AssertLocationSearchAuditLogItemsAreEqual(List<LocationSearchAuditLog> entities, List<LocationSearchAuditLogDto> dtos)
        {
            for (var index = 0; index < entities.Count; index++)
            {
                this.AssertLocationSearchAuditLog(entities[index], dtos[index]);
            }
        }

        private void AssertLocationSearchAuditLog(LocationSearchAuditLog entity, LocationSearchAuditLogDto dto)
        {
            dto.Should().BeEquivalentTo(new LocationSearchAuditLogDto
            {
                LocationSearch = entity.LocationSearch,
                Id = entity.Id,
                LogDate = entity.LogDate,
                ResultCount = entity.ResultCount
            });
        }

        private void ArrangeExpected()
        {
            maxRowCount = this.mockConfigurationSettings.Value.MaxRowCount;
            ArrangeExpectedModels();
            ArrangeExpectedDtos();
        }

        private void ArrangeExpectedModels()
        {
            expectedModels = GetLocationSearchAuditLogList(ItemCount);
        }

        private void ArrangeExpectedDtos()
        {
            expectedDtos = GetLocationSearchAuditLogDtoList(ItemCount);
        }

        private IReadOnlyLocationAuditLogger CreateSut()
        {
            return new ReadOnlyLocationAuditLogger(this.mockRepo, this.mockConverter, this.mockConfigurationSettings);
        }
    }
}