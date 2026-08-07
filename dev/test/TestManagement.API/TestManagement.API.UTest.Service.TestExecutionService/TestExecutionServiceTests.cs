using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TestManagement.API.Data;
using TestManagement.API.Features.TestExecutions;
using TestManagement.API.Models;
using TestManagement.API.Services;
using Environment = TestManagement.API.Models.Environment;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Update;

namespace TestManagement.API.UTest.Service
{
    public partial class TestExecutionServiceTests
    {
        private static TestManagementDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TestManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new TestManagementDbContext(options);
        }
    }
}
