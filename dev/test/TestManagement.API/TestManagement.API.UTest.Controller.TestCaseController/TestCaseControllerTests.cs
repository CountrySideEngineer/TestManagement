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
        private readonly Mock<ITestCaseService> _mockService;
        private readonly Mock<ILogger<TestCaseController>> _mockLogger;
        private readonly TestCaseController _controller;

        public TestCaseControllerTests()
        {
            _mockService = new Mock<ITestCaseService>();
            _mockLogger = new Mock<ILogger<TestCaseController>>();
            _controller = new TestCaseController(_mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkWithCollection()
        {
            // Arrange
            var list = new List<GetTestCaseResponse>
            {
                new GetTestCaseResponse { Code = "C1", Id = 1 },
                new GetTestCaseResponse { Code = "C2", Id = 2 }
            };
            _mockService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ICollection<GetTestCaseResponse>)list);

            // Act
            var result = await _controller.GetAllAsync(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<ICollection<GetTestCaseResponse>>(ok.Value);
            Assert.Equal(2, value.Count);
        }
    }
}
