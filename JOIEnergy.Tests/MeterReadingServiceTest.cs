using JOIEnergy.Domain;
using JOIEnergy.Interface;
using JOIEnergy.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private readonly Mock<IDataSeeder> _mockDataSeeder;
        private readonly MeterReadingService _service;

        public MeterReadingServiceTest()
        {
            _mockDataSeeder = new Mock<IDataSeeder>();
            _service = new MeterReadingService(_mockDataSeeder.Object);
        }

        [Fact]
        public void GetReadings_ReturnsReadings_WhenSmartMeterExists()
        {
            // Arrange
            var smartMeterId = "meter-1";
            var expectedReadings = new List<ElectricityReading>
            {
                new ElectricityReading { Time = DateTime.Now, Reading = 10.5M }
            };
            var data = new Dictionary<string, List<ElectricityReading>>
            {
                { smartMeterId, expectedReadings }
            };

            _mockDataSeeder.Setup(ds => ds.GetElectricityReadings())
                           .Returns(data);

            // Act
            var result = _service.GetReadings(smartMeterId);

            // Assert
            Assert.Equal(expectedReadings, result);
        }

        [Fact]
        public void GetReadings_ReturnsEmptyList_WhenSmartMeterDoesNotExist()
        {
            // Arrange
            var smartMeterId = "unknown-meter";
            var data = new Dictionary<string, List<ElectricityReading>>();

            _mockDataSeeder.Setup(ds => ds.GetElectricityReadings())
                           .Returns(data);

            // Act
            var result = _service.GetReadings(smartMeterId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void StoreReadings_AddsNewReadings_WhenSmartMeterExists()
        {
            // Arrange
            var smartMeterId = "meter-1";
            var existingReadings = new List<ElectricityReading>
            {
                new ElectricityReading { Time = DateTime.Now.AddMinutes(-10), Reading = 5.0M }
            };
            var newReadings = new List<ElectricityReading>
            {
                new ElectricityReading { Time = DateTime.Now, Reading = 7.5M }
            };
            var data = new Dictionary<string, List<ElectricityReading>>
            {
                { smartMeterId, existingReadings }
            };

            _mockDataSeeder.Setup(ds => ds.GetElectricityReadings())
                           .Returns(data);

            // Act
            var result = _service.StoreReadings(smartMeterId, newReadings);

            // Assert
            Assert.Contains(newReadings[0], result[smartMeterId]);
            Assert.Equal(2, result[smartMeterId].Count);
        }
        [Fact]
        public void StoreReadings_CreatesNewEntry_WhenSmartMeterDoesNotExist()
        {
            // Arrange
            var smartMeterId = "new-meter";
            var newReadings = new List<ElectricityReading>
            {
                new ElectricityReading { Time = DateTime.Now, Reading = 12.3M }
            };
            var data = new Dictionary<string, List<ElectricityReading>>();

            _mockDataSeeder.Setup(ds => ds.GetElectricityReadings())
                           .Returns(data);

            _mockDataSeeder.Setup(ds => ds.AddReadings(It.IsAny<string>(), It.IsAny<IEnumerable<ElectricityReading>>()))
                           .Callback<string, IEnumerable<ElectricityReading>>((id, readings) =>
                           {
                               data[id] = readings.ToList();
                           });

            // Act
            var result = _service.StoreReadings(smartMeterId, newReadings);

            // Assert
            Assert.True(result.ContainsKey(smartMeterId));
            Assert.Contains(newReadings[0], result[smartMeterId]);
        }
    }
}
