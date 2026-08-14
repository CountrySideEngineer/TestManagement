using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestManagement.API.Controllers;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Services;
using Xunit;

namespace TestManagement.API.Tests.Controller
{
    public partial class TestCaseControllerTests
    {
        [Fact]
        public async Task GetByIdAsync_WhenExists_ReturnsOkWithItem()
        {
            var mockService = new Mock<ITestCaseService>();
            var mockLogger = new Mock<ILogger<TestCaseController>>();
            var controller = new TestCaseController(mockLogger.Object, mockService.Object);

            // Arrange
            var response = new GetTestCaseResponse { Id = 10, Code = "TC-10" };
            mockService.Setup(s => s.GetByTestCaseIdAsync(10, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(response);

            // Act
            var result = await controller.GetByIdAsync(10, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<GetTestCaseResponse>(ok.Value);
            Assert.Equal(10, value.Id);
            Assert.Equal("TC-10", value.Code);
        }
    }
}
