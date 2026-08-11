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
    public class TestCaseControllerTests
    {
        public TestCaseControllerTests()
        {
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkWithCollection()
        {
            var mockService = new Mock<ITestCaseService>();
            var mockLogger = new Mock<ILogger<TestCaseController>>();
            var controller = new TestCaseController(mockLogger.Object, mockService.Object);

            // Arrange
            var list = new List<GetTestCaseResponse>
            {
                new GetTestCaseResponse { Code = "C1", Id = 1 },
                new GetTestCaseResponse { Code = "C2", Id = 2 }
            };
            mockService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ICollection<GetTestCaseResponse>)list);

            // Act
            var result = await controller.GetAllAsync(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<ICollection<GetTestCaseResponse>>(ok.Value);
            Assert.Equal(2, value.Count);
        }
    }
}
