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
using TestManagement.API.Services.Xml;
using Xunit;

namespace TestManagement.API.Tests.Service
{
	public partial class TestResultServiceTests
	{
		[Theory]
		[InlineData("PASSED", "All good")]
		[InlineData("FAILED", "Something failed")]
		[InlineData("SKIPPED", "Something skipped")]
		[InlineData("IN_PROGRESS", "Something in progress")]
		[InlineData("NOT EXECUTED", "Something to be executed")]
		[InlineData("EXCLUDED", "Something excluded")]
		public async Task GetAllAsync_MapsDomainToResponse_IncludesStatusAndTestLevel(string statusCode, string message)
		{
			// Arrange
			var dbName = Guid.NewGuid().ToString();
			using var ctx = CreateContext(dbName);

			// Create required TestCase/TestCaseVersion/TestExecutionItem and a TestResult using seeded TestStatus/TestLevel.
			var testCase = new TestCase { Id = 1000, Code = "TC-1000" };
			ctx.TestCases.Add(testCase);

			var testCaseVersion = new TestCaseVersion
			{
				Id = 2000,
				TestCaseId = testCase.Id,
				VersionNumber = 1,
				Name = "v1",
				TestLevelId = 1 // seeded TestLevel (see OnModelCreating)
			};
			ctx.TestCaseVersions.Add(testCaseVersion);

			var executionItem = new TestExecutionItem { Id = 3000 };
			ctx.TestExecutionItems.Add(executionItem);

			// Find seeded status by code (case-sensitive in seed)
			var status = ctx.TestStatuses.First(s => s.Code == statusCode);

			var tr = new TestResult
			{
				Id = 4000,
				TestExecutionItemId = executionItem.Id,
				TestCaseVersionId = testCaseVersion.Id,
				StatusId = status.Id,
				Message = message,
				ExecutedAt = DateTime.UtcNow
			};
			ctx.TestResults.Add(tr);

			await ctx.SaveChangesAsync();

			var svc = new TestResultService(ctx, new DummyXmlConverter(), NullLogger<TestResultService>.Instance);

			// Act
			var results = await svc.GetAllAsync(CancellationToken.None);

			// Assert
			Assert.Single(results);
			var res = results.First();
			Assert.Equal(tr.TestExecutionItemId, res.TestExecutionItemId);
			Assert.Equal(testCaseVersion.Id, res.TestCaseVersionId);
			Assert.Equal(testCase.Id, res.TestCaseId);
			Assert.Equal(testCaseVersion.Name, res.TestCaseVersionName);
			Assert.Equal(testCaseVersion.VersionNumber, res.TestCaseVersionNumber);
			Assert.Equal(testCaseVersion.TestLevelId, res.TestLevelId);
			Assert.Equal(status.Code, res.StatusCode);
			Assert.Equal(status.DisplayName, res.StatusDisplayName);
			Assert.Equal(message, res.Message);
		}
	}
}
