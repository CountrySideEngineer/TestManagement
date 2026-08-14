using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestManagement.API.Controllers;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Services;
using Xunit;

namespace TestManagement.API.Tests.Controller
{
    public partial class TestCaseControllerTests
    {
        [Fact]
        public async Task CreateAsync_ReturnsCreatedAtAction_WithIdRouteValue()
        {
            // Arrange
            var mockService = new Mock<ITestCaseService>();
            var mockLogger = new Mock<ILogger<TestCaseController>>();
            var controller = new TestCaseController(mockLogger.Object, mockService.Object);
            var request = new CreateTestCaseRequest { Code = "X", Name = "N", Description = "D", TestLevelId = 1 };
            var response = new CreateTestCaseResponse { Id = 123, Code = "X", Name = "N", VersionNumber = 1 };
            mockService.Setup(s => s.CreateAsync(request, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(response);

            // Act
            var result = await controller.CreateAsync(request, CancellationToken.None);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(TestCaseController.GetByIdAsync), created.ActionName);
            Assert.NotNull(created.RouteValues);
            Assert.True(created.RouteValues.ContainsKey("id"));
            Assert.Equal((long)123, created.RouteValues["id"]);
            var value = Assert.IsType<CreateTestCaseResponse>(created.Value);
            Assert.Equal(123, value.Id);
        }
    }
}
