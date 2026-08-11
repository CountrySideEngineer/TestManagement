using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestManagement.API.Controllers;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Models;
using TestManagement.API.Services;
using Xunit;

namespace TestManagement.API.Tests.Controller
{
    public partial class TestCaseControllerTests
    {
        [Fact]
        public async Task GetByVersionIdAsync_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<ITestCaseService>();
            var mockLogger = new Mock<ILogger<TestCaseController>>();
            var controller = new TestCaseController(mockLogger.Object, mockService.Object);

            mockService.Setup(s => s.GetByVersionIdAsync(999, It.IsAny<CancellationToken>()))
                        .ReturnsAsync((TestCaseVersion?)null);

            // Act
            var result = await controller.GetByVersionIdAsync(999, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
