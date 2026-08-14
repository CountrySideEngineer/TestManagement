using Moq;
using TestManagement.API.Services;
using TestManagement.API.Controllers;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestManagement.API.Tests.Controller
{
    public class TestLevelControllerTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsOkWithTestLevels()
        {
            // Arrange
            var mockService = new Moq.Mock<ITestLevelService>();
            var mockLogger = new Moq.Mock<NullLogger<TestManagement.API.Controllers.TestLevelController>>();

            var testLevels = new List<TestManagement.API.Features.TestLevel.Get.GetTestLevelResponse>
            {
                new TestManagement.API.Features.TestLevel.Get.GetTestLevelResponse(),
                new TestManagement.API.Features.TestLevel.Get.GetTestLevelResponse()
            };

            mockService
                .Setup(s => s.GetAllAsync(Moq.It.IsAny<CancellationToken>()))
                .ReturnsAsync((ICollection<TestManagement.API.Features.TestLevel.Get.GetTestLevelResponse>)testLevels);

            var controller = new TestManagement.API.Controllers.TestLevelController(mockLogger.Object, mockService.Object);

            // Act
            var result = await controller.GetAllAsync(CancellationToken.None);

            // Assert
            var okResult = Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
            var returned = Xunit.Assert.IsAssignableFrom<ICollection<TestManagement.API.Features.TestLevel.Get.GetTestLevelResponse>>(okResult.Value);
            Xunit.Assert.Equal(2, returned.Count);

            mockService.Verify(s => s.GetAllAsync(Moq.It.IsAny<CancellationToken>()), Moq.Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenServiceThrows_PropagatesException()
        {
            // Arrange
            var mockService = new Moq.Mock<ITestLevelService>();
            var mockLogger = new Moq.Mock<NullLogger<TestManagement.API.Controllers.TestLevelController>>();

            mockService
                .Setup(s => s.GetAllAsync(Moq.It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Service failure"));

            var controller = new TestManagement.API.Controllers.TestLevelController(mockLogger.Object, mockService.Object);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<System.Exception>(() => controller.GetAllAsync(CancellationToken.None));
        }
    }
}