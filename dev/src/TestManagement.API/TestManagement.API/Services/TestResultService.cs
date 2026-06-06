using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Data.Repositories;
using TestManagement.API.Features.TestResult.Create;
using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;
using TestManagement.API.Services.Xml;

namespace TestManagement.API.Services
{
    public class TestResultService
    {
        private readonly ITestResultRepository _testResultRepository;
        private readonly ITestResultXmlConverter _xmlConverter;

        /// <summary>
        /// Database context used to access and persist test execution related entities.
        /// </summary>
        private readonly TestManagementDbContext _dbContext;

        /// <summary>
        /// Optional logger instance for diagnostic logging within the service.
        /// </summary>
        private readonly ILogger<TestResultService> _logger;

        public TestResultService(
            TestManagementDbContext dbContext,
            ITestResultRepository testResultRepository,
            ITestResultXmlConverter xmlConverter,
            ILogger<TestResultService> logger
            )
        {
            _dbContext = dbContext;
            _testResultRepository = testResultRepository;
            _xmlConverter = xmlConverter;
            _logger = logger;
        }




        public async Task<ICollection<Models.TestResult>> GetAllAsync()
        {
            _logger.LogDebug("TestResultService::GetAllAsync() start!");

            var testResults = await _dbContext.TestResults
                .Include(_ => _.TestCaseVersion)
                .Include(_ => _.Status)
                .ToListAsync();

            return testResults;
        }

        public async Task<Models.TestResult> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestResultService::GetByIdAsync() start!");

            return await _testResultRepository.GetByIdAsync(id);
        }

        //public async Task CreateAsync(Models.TestResult result)
        //{
        //    _logger.LogDebug("TestResultService::Create() start!");

        //    await _testResultRepository.AddAsync(result);
        //}

        public async Task CreateAsync(CreateTestResultRequest request)
        {
            _logger.LogDebug("TestResultService::Create() start!");

            var testResult = new Models.TestResult();
            testResult.TestExecutionItemId = request.TestExecutionItemId;
            testResult.TestCaseVersionId = await _dbContext.TestCaseVersions
                .Where(_ =>
                    _.TestCaseId == request.TestCaseId &&
                    _.VersionNumber == request.TestCaseVersionNumber)
                .Select(_ => _.Id)
                .FirstOrDefaultAsync();
            testResult.StatusId = await _dbContext.TestStatuses
                .Where(_ => 
                    _.Code.ToLower() == request.TestResultStatus.ToLower())
                .Select(_ => _.Id)
                .FirstOrDefaultAsync();
            testResult.Message = request.Message;
            testResult.ExecutedAt = request.ExecutedAt;

            _dbContext.TestResults.Add(testResult);

            _dbContext.SaveChanges();
        }

        public async Task CreateAsync(ICollection<CreateTestResultRequest> requests)
        {
            _logger.LogDebug("TestResultService::Create() start!");

            foreach (var requestItem in requests)
            {
                var testResult = new Models.TestResult();
                testResult.TestExecutionItemId = requestItem.TestExecutionItemId;
                testResult.TestCaseVersionId = await _dbContext.TestCaseVersions
                    .Where(_ =>
                        _.TestCaseId == requestItem.TestCaseId &&
                        _.VersionNumber == requestItem.TestCaseVersionNumber)
                    .Select(_ => _.Id)
                    .FirstOrDefaultAsync();
                testResult.StatusId = await _dbContext.TestStatuses
                    .Where(_ =>
                        _.Code.ToLower() == requestItem.TestResultStatus.ToLower())
                    .Select(_ => _.Id)
                    .FirstOrDefaultAsync();
                testResult.Message = requestItem.Message;
                testResult.ExecutedAt = requestItem.ExecutedAt;

                _dbContext.TestResults.Add(testResult);
            }

            _dbContext.SaveChanges();
        }

        //public async Task CreateAsync(ICollection<TestResult> results)
        //{
        //    _logger.LogDebug("TestResultService::Create() start!");

        //    await _testResultRepository.AddAsync(results);
        //}

        //public async Task CreateAsync(TestSuitesXml suites)
        //{
        //    _logger.LogDebug("TestResultService::Create() start!");

        //    var results = await _xmlConverter.ConvertAsync(suites);
        //    // At this point TestCaseId/TestRunId are not set. Depending on requirements, map by name or use defaults.
        //    await _testResultRepository.AddAsync(results);
        //}

        //public async Task<ICollection<TestResult>> ConvertSuitesAsync(TestSuitesXml suites, CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug("TestResultService::ConvertSuitesAsync() start!");

        //    return await _xmlConverter.ConvertAsync(suites, cancellationToken);
        //}
    }
}
