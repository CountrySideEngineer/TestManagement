using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestManagement.API.Controllers;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Features.TestCases.Update;
using TestManagement.API.Services;
using Xunit;

namespace TestManagement.API.Tests.Controller
{
    public partial class TestCaseControllerTests
    {
        [Fact]
        public async Task UpdateAsync_ReturnsOkWithUpdatedResponse()
        {
            // Arrange
            var mockService = new Mock<ITestCaseService>();
            var mockLogger = new Mock<ILogger<TestCaseController>>();
            var controller = new TestCaseController(mockLogger.Object, mockService.Object);

            var request = new UpdateTestCaseRequest { Code = "C", Name = "New", Description = "Desc" };
            var response = new UpdateTestCaseResponse { Code = "C", Name = "New", Description = "Desc", VersionNumber = 2 };
            mockService.Setup(s => s.UpdateAsync(request, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(response);

            // Act
            var result = await controller.UpdateAsync(1, request, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<UpdateTestCaseResponse>(ok.Value);
            Assert.Equal(2, value.VersionNumber);
            Assert.Equal("C", value.Code);
        }
    }
}
