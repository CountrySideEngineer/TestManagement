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
        public async Task CreateIfNotExistsAsync_ReturnsOkWithResponses()
        {
            // Arrange
            var mockService = new Mock<ITestCaseService>();
            var mockLogger = new Mock<ILogger<TestCaseController>>();
            var controller = new TestCaseController(mockLogger.Object, mockService.Object);

            var requests = new List<CreateTestCaseRequest>();
            var responses = new List<CreateTestCaseResponse>();
            mockService.Setup(s => s.CreateIfNotExistsAsync(It.IsAny<ICollection<CreateTestCaseRequest>>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ICollection<CreateTestCaseResponse>)responses);

            // Act
            var result = await controller.CreateIfNotExistsAsync(requests, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<ICollection<CreateTestCaseResponse>>(ok.Value);
            Assert.Empty(value);
        }
    }
}
