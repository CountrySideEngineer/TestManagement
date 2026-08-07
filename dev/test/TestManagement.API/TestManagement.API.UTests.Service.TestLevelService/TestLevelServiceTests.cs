using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestManagement.API.Data;
using TestManagement.API.Models;
using TestManagement.API.Services;
using Xunit;

namespace TestManagement.API.UTests.Service
{
    public class TestLevelServiceTests
    {
        private static TestManagementDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TestManagementDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new TestManagementDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_WithOneRecord_ReturnsMappedResponses()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);
            context.TestLevels.AddRange(
                new TestLevel { Id = 1, Name = "Level A", Description = "Desc A", Code = "Code A", DisplayName = "Display A" }
            );
            await context.SaveChangesAsync();

            var service = new TestLevelService(context, new NullLogger<TestLevelService>());

            // Act
            var result = await service.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(result, r => r.Id == 1 && r.Name == "Level A" && r.Description == "Desc A");
        }

        [Fact]
        public async Task GetAllAsync_WithTwoRecords_ReturnsMappedResponses()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);
            context.TestLevels.AddRange(
                new TestLevel { Id = 1, Name = "Level A", Description = "Desc A", Code = "Code A", DisplayName = "Display A" },
                new TestLevel { Id = 2, Name = "Level B", Description = "Desc B", Code = "Code B", DisplayName = "Display B" }
            );
            await context.SaveChangesAsync();

            var service = new TestLevelService(context, new NullLogger<TestLevelService>());

            // Act
            var result = await service.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Id == 1 && r.Name == "Level A" && r.Description == "Desc A");
            Assert.Contains(result, r => r.Id == 2 && r.Name == "Level B" && r.Description == "Desc B");
        }

        [Fact]
        public async Task GetAllAsync_WithNoRecords_ReturnsEmptyCollection()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateDbContext(dbName);

            var service = new TestLevelService(context, new NullLogger<TestLevelService>());

            // Act
            var result = await service.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_CancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateDbContext(dbName);
            context.TestLevels.Add(
                new TestLevel { Id = 1, Name = "Level A", Description = "Desc A", Code = "Code A", DisplayName = "Display A" }
                );
            await context.SaveChangesAsync();

            var service = new TestLevelService(context, new NullLogger<TestLevelService>());

            using var cts = new CancellationTokenSource();
            cts.Cancel(); // cancellation requested before call

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => service.GetAllAsync(cts.Token));
        }
    }
}