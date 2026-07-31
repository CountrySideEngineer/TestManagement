using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using TestManagement.API.Data;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Features.TestCases.Update;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Tests.Service
{
	public partial class TestCaseServiceTests
	{
		[Fact]
		public async Task GetAllLatestVersionAsync_ReturnsOnlyTheLatestVersionPerTestCase()
		{
			await using var context = CreateContext();
			await SeedDataAsync(context);
			var service = CreateService(context);

			var result = await service.GetAllLatestVersionAsync(CancellationToken.None);

			Assert.Equal(2, result.Count);
			Assert.All(result, response => Assert.Single(response.Versions));
			Assert.Contains(result, response => response.Code == "TC-001" && response.Versions.Single().VersionNumber == 2);
			Assert.Contains(result, response => response.Code == "TC-002" && response.Versions.Single().VersionNumber == 1);
		}
	}
}
