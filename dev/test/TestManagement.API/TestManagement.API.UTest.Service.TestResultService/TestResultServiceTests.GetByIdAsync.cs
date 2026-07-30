using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using TestManagement.API.Data;
using TestManagement.API.Features.TestResult.Create;
using TestManagement.API.Features.TestResult.Get;
using TestManagement.API.Models;
using TestManagement.API.Services;
using Xunit;

namespace TestManagement.API.Tests.Service
{
    public partial class TestResultServiceTests
    {
        [Theory]
        [InlineData(1000, 1)]
        [InlineData(1000, 1000)]
        public async Task GetByIdAsync_RegisteredInTableAtTopOrTail(int totalCount, int id)
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            for (int index = 1; index <= totalCount; index++)
            {
                string code = $"TC-{index}";
                var testCase = new TestCase { Id = index, Code = code };
                ctx.TestCases.Add(testCase);
                var testCaseVersion = new TestCaseVersion
                {
                    Id = 2000 + index,
                    TestCaseId = testCase.Id,
                    VersionNumber = 1,
                    Name = "v1",
                    TestLevelId = 1 // seeded TestLevel (see OnModelCreating)
                };
                ctx.TestCaseVersions.Add(testCaseVersion);
                var executionItem = new TestExecutionItem { Id = 3000 + index };
                ctx.TestExecutionItems.Add(executionItem);
                // Find seeded status by code (case-sensitive in seed)
                var status = ctx.TestStatuses.First(s => s.Code == "PASSED");
                var tr = new TestResult
                {
                    Id = index,
                    TestExecutionItemId = executionItem.Id,
                    TestCaseVersionId = testCaseVersion.Id,
                    StatusId = status.Id,
                    Message = "Test message",
                    ExecutedAt = DateTime.UtcNow
                };
                ctx.TestResults.Add(tr);
            }
            // Create required TestCase/TestCaseVersion/TestExecutionItem and a TestResult using seeded TestStatus/TestLevel.
            await ctx.SaveChangesAsync();

            var svc = new TestResultService(ctx, NullLogger<TestResultService>.Instance);
            // Act
            var resultResponse = await svc.GetByIdAsync(id, CancellationToken.None);
            // Assert
            Assert.NotNull(resultResponse);
            Assert.Equal(id, resultResponse.Id);
        }

        [Theory]
        [InlineData(1000, 0)]     // min id - 1 (1 - 1)
        [InlineData(1000, 1001)]  // max id + 1 (1000 + 1)
        public async Task GetByIdAsync_NotFound_ThrowsInvalidOperationException(int totalCount, int id)
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            for (int index = 1; index <= totalCount; index++)
            {
                string code = $"TC-{index}";
                var testCase = new TestCase { Id = index, Code = code };
                ctx.TestCases.Add(testCase);
                var testCaseVersion = new TestCaseVersion
                {
                    Id = 2000 + index,
                    TestCaseId = testCase.Id,
                    VersionNumber = 1,
                    Name = "v1",
                    TestLevelId = 1 // seeded TestLevel (see OnModelCreating)
                };
                ctx.TestCaseVersions.Add(testCaseVersion);
                var executionItem = new TestExecutionItem { Id = 3000 + index };
                ctx.TestExecutionItems.Add(executionItem);
                // Find seeded status by code (case-sensitive in seed)
                var status = ctx.TestStatuses.First(s => s.Code == "PASSED");
                var tr = new TestResult
                {
                    Id = index,
                    TestExecutionItemId = executionItem.Id,
                    TestCaseVersionId = testCaseVersion.Id,
                    StatusId = status.Id,
                    Message = "Test message",
                    ExecutedAt = DateTime.UtcNow
                };
                ctx.TestResults.Add(tr);
            }
            await ctx.SaveChangesAsync();

            var svc = new TestResultService(ctx, NullLogger<TestResultService>.Instance);

            // Act & Assert: GetByIdAsync uses FirstAsync -> not found => InvalidOperationException
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await svc.GetByIdAsync(id, CancellationToken.None));
        }
    }
}
