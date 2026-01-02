// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using TestManagement.Analyze.APP.ApiClient;
using TestManagement.Analyze.APP.DBContext;
using TestManagement.Analyze.APP.Entities;
using TestManagement.Analyze.APP.Infrastructure.DBConnectionFactory;
using TestManagement.Analyze.APP.Model;
using TestManagement.Analyze.APP.Model.Converter;
using TestManagement.Analyze.APP.Model.DTO;
using TestManagement.Analyze.APP.Model.Xml;
using TestManagement.Analyze.APP.Repository;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        string curDirPath = AppDomain.CurrentDomain.BaseDirectory;
        config.SetBasePath(curDirPath);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        // Register IOptions<T>
        services.Configure<ApiSetting>(configuration.GetSection("ApiSettings"));

        // Register DbContext
        string connectionString =
            DBConnectionFactory.CreatePostgresConnectionString(configuration)
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<RequestDbContext>(options => options.UseNpgsql(connectionString));

        // Register typed HttpClient
        services.AddHttpClient<TestCaseApiClient>((sp, client) =>
        {
            var setting = sp.GetRequiredService<IOptions<ApiSetting>>().Value;
            if (string.IsNullOrWhiteSpace(setting.BaseUrl))
            {
                throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");
            }
            client.BaseAddress = new Uri(setting.BaseUrl);
        });
        services.AddHttpClient<TestRunApiClient>((sp, client) =>
        {
            var setting = sp.GetRequiredService<IOptions<ApiSetting>>().Value;
            if (string.IsNullOrWhiteSpace(setting.BaseUrl))
            {
                throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");
            }
            client.BaseAddress = new Uri(setting.BaseUrl);
        });
        services.AddHttpClient<TestResultApiClient>((sp, client) =>
        {
            var setting = sp.GetRequiredService<IOptions<ApiSetting>>().Value;
            if (string.IsNullOrWhiteSpace(setting.BaseUrl))
            {
                throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");
            }
            client.BaseAddress = new Uri(setting.BaseUrl);
        });

        // その他 DI 登録
        services.AddTransient<TestSuitesReader>();
        services.AddTransient<TestSuitesConverter>();
        services.AddTransient(typeof(GenericRepository<>));
        services.AddLogging(builder => builder.AddConsole());
    })
    .Build();

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();

var dbContext = services.GetRequiredService<RequestDbContext>();
var requestRepo = new GenericRepository<Request>(dbContext);
var requests = requestRepo.Find(_ => _.StatusId == (int)STATUS.NOT_STARTED).ToList();

var suitesReader = services.GetRequiredService<TestSuitesReader>();
var converter = services.GetRequiredService<TestSuitesConverter>();
var testCaseApiClient = services.GetRequiredService<TestCaseApiClient>();
var testRunApiClient = services.GetRequiredService<TestRunApiClient>();
var testResultApiClient = services.GetRequiredService<TestResultApiClient>();
foreach (var requestItem in requests)
{
    logger.LogInformation($"{nameof(requestItem.Id),24} = {requestItem.Id}");
    logger.LogInformation($"{nameof(requestItem.DirectoryPath),24} = {requestItem.DirectoryPath}");

    try
    {
        IEnumerable<TestSuites> testSuitesCollection = suitesReader.ReadFromDir(requestItem.DirectoryPath);
        IEnumerable<TestCaseDto> testCases = converter.ToTestCases(testSuitesCollection);
        if (0 == testCases.Count())
        {
            continue;
        }

        ICollection<TestCaseDto>? registeredTestCases = testCaseApiClient.GetAll();
        var testCasesToRegister = new List<TestCaseDto>();
        if (null == registeredTestCases)
        {
            testCasesToRegister.AddRange(testCases);
        }
        else
        {
            // Check overlapped test case.
            var existingTestTitles = new HashSet<string>
                (registeredTestCases.Select(_ => (_.Title ?? string.Empty).Trim()), StringComparer.OrdinalIgnoreCase);
            foreach (var testCaseItem in testCases)
            {
                var title = (testCaseItem.Title ?? string.Empty).Trim();
                if (!existingTestTitles.Contains(title))
                {
                    logger.LogInformation($"Registering test case: {title}");
                    testCasesToRegister.Add(testCaseItem);
                }
                else
                {
                    logger.LogInformation($"[SKIP] \"{title}\" has already been registered.");
                }
            }
        }
        if (0 == testCasesToRegister.Count)
        {
            // In a case all test cases have already been registered.
            logger.LogInformation("No test cases to register.");
        }
        else
        {
            ICollection<TestCaseDto>? newRegisteredTestCases = testCaseApiClient.Add(testCasesToRegister);
            if (null == newRegisteredTestCases)
            {
                continue;
            }
        }
        requestItem.Status = new StatusMaster() { Id = (int)STATUS.RUNNING, Name = $"{nameof(STATUS.RUNNING)}" };
        requestItem.StatusId = requestItem.Status.Id;
        requestItem.UpdateAt = DateTime.UtcNow;
        requestRepo.Update(requestItem);

        //Create new TestRun.
        TestRunDto newTestRun = new()
        {
            ExecutedAt = DateTime.UtcNow
        };
        TestRunDto? regTestRun = testRunApiClient.Add(newTestRun);

        if (null != regTestRun)
        {
            logger.LogInformation($"New test run registered by Id = {regTestRun?.Id}");
        }
        else
        {
            continue;
        }

        registeredTestCases = testCaseApiClient.GetAll();
        List<TestResultDto> testResults = converter.ToTestResults(testSuitesCollection).ToList();
        foreach (var testResultItem in testResults)
        {
            string testCaseName = testResultItem.TestCase.Title;
            TestCaseDto? testCaseDto = registeredTestCases?.Where(_ => _.Title == testCaseName).First() ?? null;
            testResultItem.TestCaseId = testCaseDto.Id;
            testResultItem.TestCase = testCaseDto;

            testResultItem.TestRun = regTestRun!;
            testResultItem.TestRunId = regTestRun!.Id;
        }
        ICollection<TestResultDto>? addResults = testResultApiClient.Add(testResults);
        if (null == addResults)
        {
            requestItem.Status = new StatusMaster() { Id = (int)STATUS.COMPLETED_BY_SUCCESS, Name = $"{nameof(STATUS.COMPLETED_BY_SUCCESS)}" };
        }
        else
        {
            requestItem.Status = new StatusMaster() { Id = (int)STATUS.COMPLETED_BY_FAILURE, Name = $"{nameof(STATUS.COMPLETED_BY_SUCCESS)}" };
        }
        requestItem.StatusId = requestItem.Status.Id;
        requestItem.UpdateAt = DateTime.UtcNow;
        requestRepo.Update(requestItem);
    }
    catch (Exception ex)
    {
        logger.LogError(ex.ToString());
    }
}

return;