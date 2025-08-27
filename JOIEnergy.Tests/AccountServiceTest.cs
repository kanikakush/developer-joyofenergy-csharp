using JOIEnergy.Interface;
using JOIEnergy.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace JOIEnergy.Tests
{
    public class AccountServiceTest
    {
        private readonly Mock<IDataSeeder> _mockDataSeeder;
        private readonly AccountService _accountService;

        public AccountServiceTest()
        {
            _mockDataSeeder = new Mock<IDataSeeder>();
            _accountService = new AccountService(_mockDataSeeder.Object);
        }

        [Fact]
        public void GetPricePlanIdForSmartMeterId_ReturnsPricePlanId_WhenSmartMeterExists()
        {
            // Arrange
            var smartMeterId = "meter-123";
            var expectedPricePlanId = "plan-xyz";
            var data = new Dictionary<string, string> { { smartMeterId, expectedPricePlanId } };

            _mockDataSeeder.Setup(ds => ds.GetSmartMeterAccounts())
                           .Returns(data);

            // Act
            var result = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);

            // Assert
            Assert.Equal(expectedPricePlanId, result);
            _mockDataSeeder.Verify(ds => ds.GetSmartMeterAccounts(), Times.Once);
        }

        [Fact]
        public void GetPricePlanIdForSmartMeterId_ReturnsNull_WhenSmartMeterDoesNotExist()
        {
            // Arrange
            var smartMeterId = "meter-unknown";
            var data = new Dictionary<string, string> { { "meter-123", "plan-abc" } };

            _mockDataSeeder.Setup(ds => ds.GetSmartMeterAccounts())
                           .Returns(data);

            // Act
            var result = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);

            // Assert
            Assert.Null(result);
            _mockDataSeeder.Verify(ds => ds.GetSmartMeterAccounts(), Times.Once);
        }

        [Fact]
        public void GetPricePlanIdForSmartMeterId_ReturnsNull_WhenSeederReturnsEmptyDictionary()
        {
            // Arrange
            _mockDataSeeder.Setup(ds => ds.GetSmartMeterAccounts())
                           .Returns(new Dictionary<string, string>());

            // Act
            var result = _accountService.GetPricePlanIdForSmartMeterId("any-meter");

            // Assert
            Assert.Null(result);
        }

    }
}
