using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TestManagement.API.Data;
using TestManagement.API.Features.Environment.Create;
using TestManagement.API.Features.Environment.Update;
using TestManagement.API.Models;
using TestManagement.API.Services;
using Xunit;

namespace TestManagement.API.Tests.Service
{
    public class EnvironmentServiceTests
    {
        private static TestManagementDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TestManagementDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new TestManagementDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEnvironmentsWithVersions()
        {
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            var env1 = new Models.Environment { Id = 1, Name = "Env-A" };
            env1.AddVersion("Windows", ".NET 8");
            env1.AddVersion("Windows", ".NET 8.1");

            var env2 = new Models.Environment { Id = 2, Name = "Env-B" };
            env2.AddVersion("Linux", ".NET 7");

            ctx.Environments.Add(env1);
            ctx.Environments.Add(env2);
            await ctx.SaveChangesAsync();

            var svc = new EnvironmentService(ctx, NullLogger<EnvironmentService>.Instance);

            var results = await svc.GetAllAsync(CancellationToken.None);

            Assert.Equal(2, results.Count);
            var a = results.Single(r => r.Name == "Env-A");
            Assert.Equal(2, a.Versions.Count());
            var b = results.Single(r => r.Name == "Env-B");
            Assert.Single(b.Versions);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMatchingEnvironment()
        {
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            var env = new Models.Environment { Id = 100, Name = "ByIdEnv" };
            env.AddVersion("OS1", "RT1");
            ctx.Environments.Add(env);
            await ctx.SaveChangesAsync();

            var svc = new EnvironmentService(ctx, NullLogger<EnvironmentService>.Instance);

            var results = await svc.GetByIdAsync((int)env.Id, CancellationToken.None);
            Assert.Single(results);
            Assert.Equal("ByIdEnv", results.First().Name);
            Assert.Single(results.First().Versions);
        }

        [Fact]
        public async Task GetByNameAsync_ReturnsMatchingEnvironment()
        {
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            var env = new Models.Environment { Id = 200, Name = "ByNameEnv" };
            env.AddVersion("OSX", "RTX");
            ctx.Environments.Add(env);
            await ctx.SaveChangesAsync();

            var svc = new EnvironmentService(ctx, NullLogger<EnvironmentService>.Instance);

            var results = await svc.GetByNameAsync("ByNameEnv", CancellationToken.None);
            Assert.Single(results);
            Assert.Equal("ByNameEnv", results.First().Name);
        }

        [Fact]
        public async Task CreateAsync_CreatesEnvironment_WhenNameNotExists()
        {
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            var svc = new EnvironmentService(ctx, NullLogger<EnvironmentService>.Instance);

            var req = new CreateEnvironmentRequest
            {
                Name = "NewEnv",
                Os = "Win",
                RunTime = ".NET 8"
            };

            var resp = await svc.CreateAsync(req, CancellationToken.None);

            Assert.Equal(req.Name, resp.Name);
            Assert.NotEqual(0, resp.Id);

            var persisted = ctx.Environments.Include(e => e.Versions).FirstOrDefault(e => e.Name == req.Name);
            Assert.NotNull(persisted);
            Assert.Single(persisted.Versions);
        }

        [Fact]
        public async Task CreateAsync_Throws_WhenNameExists()
        {
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            var existing = new Models.Environment { Id = 500, Name = "ExistsEnv" };
            existing.AddVersion("X", "Y");
            ctx.Environments.Add(existing);
            await ctx.SaveChangesAsync();

            var svc = new EnvironmentService(ctx, NullLogger<EnvironmentService>.Instance);

            var req = new CreateEnvironmentRequest
            {
                Name = "ExistsEnv",
                Os = "X",
                RunTime = "Y"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await svc.CreateAsync(req, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateAsync_AddsNewVersion_WhenDifferent()
        {
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            var env = new Models.Environment { Id = 600, Name = "UpdEnv" };
            env.AddVersion("A", "1");
            ctx.Environments.Add(env);
            await ctx.SaveChangesAsync();

            var svc = new EnvironmentService(ctx, NullLogger<EnvironmentService>.Instance);

            var req = new UpdateEnvironmentRequest
            {
                Name = "UpdEnv",
                Os = "B",
                RunTime = "2"
            };

            var resp = await svc.UpdateAsync(req, CancellationToken.None);

            Assert.Equal(req.Name, resp.Name);
            Assert.Equal(req.Os, resp.Os);
            Assert.Equal(req.RunTime, resp.RunTime);
            Assert.Equal(2, resp.VersionNumber);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenNoChange()
        {
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            var env = new Models.Environment { Id = 700, Name = "NoChangeEnv" };
            env.AddVersion("SameOS", "SameRT");
            ctx.Environments.Add(env);
            await ctx.SaveChangesAsync();

            var svc = new EnvironmentService(ctx, NullLogger<EnvironmentService>.Instance);

            var req = new UpdateEnvironmentRequest
            {
                Name = "NoChangeEnv",
                Os = "SameOS",
                RunTime = "SameRT"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await svc.UpdateAsync(req, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenEnvironmentNotExists()
        {
            var dbName = Guid.NewGuid().ToString();
            using var ctx = CreateContext(dbName);

            var svc = new EnvironmentService(ctx, NullLogger<EnvironmentService>.Instance);

            var req = new UpdateEnvironmentRequest
            {
                Name = "MissingEnv",
                Os = "X",
                RunTime = "Y"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await svc.UpdateAsync(req, CancellationToken.None));
        }
    }
}
