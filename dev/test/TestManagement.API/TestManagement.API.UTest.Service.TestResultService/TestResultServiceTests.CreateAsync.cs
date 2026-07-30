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
		[InlineData("passed")]
		[InlineData("FAILED")]
		public async Task CreateAsync_SingleRequest_CreatesEntityAndReturnsResponse(string requestStatus)
		{
			// Arrange
			var dbName = Guid.NewGuid().ToString();
			using var ctx = CreateContext(dbName);

			// Create TestCase/TestCaseVersion and execution item
			var testCase = new TestCase { Id = 1100, Code = "TC-1100" };
			ctx.TestCases.Add(testCase);

			var testCaseVersion = new TestCaseVersion
			{
				Id = 2100,
				TestCaseId = testCase.Id,
				VersionNumber = 7,
				Name = "v7",
				TestLevelId = 1
			};
			ctx.TestCaseVersions.Add(testCaseVersion);

			var executionItem = new TestExecutionItem { Id = 3100 };
			ctx.TestExecutionItems.Add(executionItem);

			await ctx.SaveChangesAsync();

			var svc = new TestResultService(ctx, NullLogger<TestResultService>.Instance);

			var req = new CreateTestResultRequest
			{
				TestExecutionItemId = executionItem.Id,
				TestCaseId = testCase.Id,
				TestCaseVersionNumber = testCaseVersion.VersionNumber,
				TestLevelId = testCaseVersion.TestLevelId,
				ExecutedAt = DateTime.UtcNow,
				Message = "create single",
				TestResultStatus = requestStatus
			};

			// Act
			var resp = await svc.CreateAsync(req, CancellationToken.None);

			// Assert - response should reflect the request and an entity should be persisted
			Assert.Equal(req.TestExecutionItemId, resp.TestExecutionItemId);
			Assert.Equal(req.TestCaseId, resp.TestCaseId);
			Assert.Equal(req.TestCaseVersionNumber, resp.TestCaseVersionNumber);
			Assert.Equal(req.TestLevelId, resp.TestLevelId);
			Assert.Equal(req.Message, resp.Message);
			Assert.Equal(req.TestResultStatus, resp.TestResultStatus);

			var persisted = await ctx.TestResults.FirstOrDefaultAsync(tr => tr.Id == resp.ResultId);
			Assert.NotNull(persisted);
			// StatusId should map to a seeded TestStatus (lookup by case-insensitive code)
			var expectedStatus = ctx.TestStatuses.FirstOrDefault(s => s.Code.Equals(req.TestResultStatus, StringComparison.OrdinalIgnoreCase));
			Assert.NotNull(expectedStatus);
			Assert.Equal(expectedStatus.Id, persisted.StatusId);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(2)]
		public async Task CreateAsync_BulkRequests_HandlesEmptyAndMultiple(int count)
		{
			// Arrange
			var dbName = Guid.NewGuid().ToString();
			using var ctx = CreateContext(dbName);

			// Setup minimal test case/version and execution items used by requests
			var testCase = new TestCase { Id = 1200, Code = "TC-1200" };
			ctx.TestCases.Add(testCase);

			var testCaseVersion = new TestCaseVersion
			{
				Id = 2200,
				TestCaseId = testCase.Id,
				VersionNumber = 3,
				Name = "v3",
				TestLevelId = 1
			};
			ctx.TestCaseVersions.Add(testCaseVersion);

			var execItem = new TestExecutionItem { Id = 3200 };
			ctx.TestExecutionItems.Add(execItem);

			await ctx.SaveChangesAsync();

			var svc = new TestResultService(ctx, NullLogger<TestResultService>.Instance);

			var requests = new List<CreateTestResultRequest>();
			for (int i = 0; i < count; i++)
			{
				requests.Add(new CreateTestResultRequest
				{
					TestExecutionItemId = execItem.Id,
					TestCaseId = testCase.Id,
					TestCaseVersionNumber = testCaseVersion.VersionNumber,
					TestLevelId = testCaseVersion.TestLevelId,
					ExecutedAt = DateTime.UtcNow,
					Message = $"bulk-{i}",
					TestResultStatus = "PASSED"
				});
			}

			// Act
			var responses = await svc.CreateAsync(requests, CancellationToken.None);

			// Assert
			Assert.Equal(count, responses.Count);
			if (count > 0)
			{
				// verify persisted count
				var persisted = await ctx.TestResults.ToListAsync();
				Assert.Equal(count, persisted.Count);
			}
		}
	}
}
