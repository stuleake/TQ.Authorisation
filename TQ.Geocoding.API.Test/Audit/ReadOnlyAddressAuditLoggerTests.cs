using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telerik.JustMock;
using TQ.Geocoding.Api.Test;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository.Address;
using TQ.Geocoding.Service.Audit;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Converters;
using Xunit;

namespace TQ.Geocoding.API.Test.Audit
{
    public class ReadOnlyAddressAuditLoggerTests
    {
        private readonly IAddressReadOnlyRepository<AddressSearchAuditLog> mockRepo = Mock.Create<IAddressReadOnlyRepository<AddressSearchAuditLog>>();
        private readonly IAddressAuditLogConverter mockConverter = Mock.Create<IAddressAuditLogConverter>();
        private readonly IOptions<ConfigurationSettings> mockConfigurationSettings = Options.Create(TestHelper.GetConfigurationSettings());

        private readonly DateTime logDate = DateTime.Now;
        private readonly Guid logId = Guid.NewGuid();
        private readonly string addressSearch = "address search";
        private readonly int resultsCount = 100;

        private const int ItemCount = 3;

        private int maxRowCount;
        private IEnumerable<AddressSearchAuditLog> expectedModels;
        private IEnumerable<AddressSearchAuditLogDto> expectedDtos;

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
        public async Task GetAllByDateRangeAsyncThrowsArgumentNullExceptionWhenStartDateIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetAllByDateRangeAsync (default, default));
        }

        [Fact]
        public async Task GetAllByDateRangeAsyncThrowsArgumentNullExceptionWhenEndDateIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetAllByDateRangeAsync(DateTime.Now, default));
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

            Mock.Arrange(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<AddressSearchAuditLog, bool>>>(), Arg.AnyInt)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToAddressSearchAuditLogDtos(Arg.IsAny<IEnumerable<AddressSearchAuditLog>>())).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetAllByDateRangeAsync(DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            Mock.Assert(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<AddressSearchAuditLog, bool>>>(), Arg.AnyInt), Occurs.Once());
            Mock.Assert(() => mockConverter.ToAddressSearchAuditLogDtos(Arg.IsAny<IEnumerable<AddressSearchAuditLog>>()), Occurs.Once());

            actual.Count().Should().Be(ItemCount);
            this.AssertAddressSearchAuditLogItemsAreEqual(expectedModels.ToList(), actual.ToList());
        }

        [Fact]
        public async Task GetAllByResultCountRangeAsyncSuccess()
        {
            // Arrange
            ArrangeExpected();

            Mock.Arrange(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<AddressSearchAuditLog, bool>>>(), Arg.AnyInt)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToAddressSearchAuditLogDtos(Arg.IsAny<IEnumerable<AddressSearchAuditLog>>())).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetAllByResultCountRangeAsync(0, 100);

            // Assert
            Mock.Assert(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<AddressSearchAuditLog, bool>>>(), Arg.AnyInt), Occurs.Once());
            Mock.Assert(() => mockConverter.ToAddressSearchAuditLogDtos(Arg.IsAny<IEnumerable<AddressSearchAuditLog>>()), Occurs.Once());

            actual.Count().Should().Be(ItemCount);
            this.AssertAddressSearchAuditLogItemsAreEqual(expectedModels.ToList(), actual.ToList());
        }

        [Fact]
        public async Task GetAllBySearchTextAsyncSuccess()
        {
            // Arrange
            ArrangeExpected();

            Mock.Arrange(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<AddressSearchAuditLog, bool>>>(), Arg.AnyInt)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToAddressSearchAuditLogDtos(Arg.IsAny<IEnumerable<AddressSearchAuditLog>>())).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetAllBySearchTextAsync(addressSearch);

            // Assert
            Mock.Assert(() => mockRepo.ListAsync(Arg.IsAny<Expression<Func<AddressSearchAuditLog, bool>>>(), Arg.AnyInt), Occurs.Once());
            Mock.Assert(() => mockConverter.ToAddressSearchAuditLogDtos(Arg.IsAny<IEnumerable<AddressSearchAuditLog>>()), Occurs.Once());

            actual.Count().Should().Be(ItemCount);
            this.AssertAddressSearchAuditLogItemsAreEqual(expectedModels.ToList(), actual.ToList());
        }

        [Fact]
        public async Task GetAllAsyncSuccess()
        {
            // Arrange
            ArrangeExpected();

            Mock.Arrange(() => mockRepo.ListAsync(maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToAddressSearchAuditLogDtos(Arg.IsAny<IEnumerable<AddressSearchAuditLog>>())).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetAllAsync();

            // Assert
            Mock.Assert(() => mockRepo.ListAsync(maxRowCount), Occurs.Once());
            Mock.Assert(() => mockConverter.ToAddressSearchAuditLogDtos(Arg.IsAny<IEnumerable<AddressSearchAuditLog>>()), Occurs.Once());

            actual.Count().Should().Be(ItemCount);
            this.AssertAddressSearchAuditLogItemsAreEqual(expectedModels.ToList(), actual.ToList());
        }

        private void AssertAddressSearchAuditLogItemsAreEqual(List<AddressSearchAuditLog> entities, List<AddressSearchAuditLogDto> dtos)
        {
            for (var index = 0; index < entities.Count; index++)
            {
                this.AssertAddressSearchAuditLog(entities[index], dtos[index]);
            }
        }

        private void AssertAddressSearchAuditLog(AddressSearchAuditLog entity, AddressSearchAuditLogDto dto)
        {
            dto.Should().BeEquivalentTo(new AddressSearchAuditLogDto
            {
                AddressSearch = entity.AddressSearch,
                Id = entity.Id,
                LogDate = entity.LogDate,
                ResultCount = entity.ResultCount
            });
        }

        private IEnumerable<AddressSearchAuditLog> GetAddressSearchAuditLogList(int count)
        {
            var auditLogItems = new List<AddressSearchAuditLog>();

            for (var index = 1; index <= count; index++)
            {
                var auditLogItem = new AddressSearchAuditLog
                {
                    AddressSearch = addressSearch,
                    Id = logId,
                    LogDate = logDate.AddDays(index),
                    ResultCount = resultsCount,
                };

                auditLogItems.Add(auditLogItem);
            }

            return auditLogItems;
        }

        private IEnumerable<AddressSearchAuditLogDto> GetAddressSearchAuditLogDtoList(int count)
        {
            var dtos = new List<AddressSearchAuditLogDto>();

            for (var index = 1; index <= count; index++)
            {
                var dto = new AddressSearchAuditLogDto
                {
                    AddressSearch = addressSearch,
                    Id = logId,
                    LogDate = logDate.AddDays(index),
                    ResultCount = resultsCount
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        private void ArrangeExpected()
        {
            maxRowCount = this.mockConfigurationSettings.Value.MaxRowCount;
            ArrangeExpectedModels();
            ArrangeExpectedDtos();
        }

       private void ArrangeExpectedModels()
        {
            expectedModels = GetAddressSearchAuditLogList(ItemCount);
        }

        private void ArrangeExpectedDtos()
        {
            expectedDtos = GetAddressSearchAuditLogDtoList(ItemCount);
        }

        private IReadOnlyAddressAuditLogger CreateSut()
        {
            return new ReadOnlyAddressAuditLogger(this.mockRepo, this.mockConverter, this.mockConfigurationSettings);
        }
    }
}