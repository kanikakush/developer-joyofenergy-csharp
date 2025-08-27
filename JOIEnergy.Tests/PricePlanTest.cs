using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Interface;
using JOIEnergy.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JOIEnergy.Tests
{
    public class PricePlanTest
    {
        private readonly Mock<IMeterReadingService> _mockMeterReadingService;
        private readonly Mock<IDataSeeder> _mockDataSeeder;
        private readonly PricePlanService _service;

        public PricePlanTest()
        {
            _mockMeterReadingService = new Mock<IMeterReadingService>();
            _mockDataSeeder = new Mock<IDataSeeder>();
            _service = new PricePlanService(_mockMeterReadingService.Object, _mockDataSeeder.Object);
        }

        [Fact]
        public void GetConsumptionCostOfElectricityReadingsForEachPricePlan_ReturnsEmpty_WhenNoReadings()
        {
            // Arrange
            string smartMeterId = "meter-1";
            _mockMeterReadingService.Setup(m => m.GetReadings(smartMeterId))
                                    .Returns(new List<ElectricityReading>());

            // Act
            var result = _service.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetConsumptionCostOfElectricityReadingsForEachPricePlan_ReturnsCalculatedCosts_WhenReadingsExist()
        {
            // Arrange
            string smartMeterId = "meter-1";
            var now = DateTime.Now;
            var readings = new List<ElectricityReading>
            {
                new ElectricityReading { Time = now.AddHours(-2), Reading = 10m },
                new ElectricityReading { Time = now, Reading = 20m }
            };

            _mockMeterReadingService.Setup(m => m.GetReadings(smartMeterId))
                                    .Returns(readings);

            var pricePlans = new List<PricePlan>
            {
                new PricePlan { PlanName = "Plan-A", UnitRate = 0.5m },
                new PricePlan { PlanName = "Plan-B", UnitRate = 1.0m }
            };

            _mockDataSeeder.Setup(d => d.GetPricePlans())
                           .Returns(pricePlans);

            // Act
            var result = _service.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            // ✅ Manual calculation for validation:
            // Average reading = (10 + 20) / 2 = 15
            // Time elapsed = 2 hours
            // Average per hour = 15 / 2 = 7.5
            // Cost Plan-A = 7.5 * 0.5 = 3.75
            // Cost Plan-B = 7.5 * 1.0 = 7.5

            Assert.Equal(3.75m, result["Plan-A"]);
            Assert.Equal(7.5m, result["Plan-B"]);
        }

        [Fact]
        public void GetConsumptionCostOfElectricityReadingsForEachPricePlan_ThrowsDivideByZero_WhenAllReadingsSameTime()
        {
            // Arrange
            string smartMeterId = "meter-1";
            var now = DateTime.Now;
            var readings = new List<ElectricityReading>
            {
                new ElectricityReading { Time = now, Reading = 10m },
                new ElectricityReading { Time = now, Reading = 20m }
            };

            _mockMeterReadingService.Setup(m => m.GetReadings(smartMeterId))
                                    .Returns(readings);

            var pricePlans = new List<PricePlan>
            {
                new PricePlan { PlanName = "Plan-C", UnitRate = 1.0m }
            };

            _mockDataSeeder.Setup(d => d.GetPricePlans())
                           .Returns(pricePlans);

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() =>
                _service.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId));
        }
    }
}
