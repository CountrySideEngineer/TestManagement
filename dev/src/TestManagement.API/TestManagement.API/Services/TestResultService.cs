using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;
using TestManagement.API.Services.Xml;

namespace TestManagement.API.Services
{
    public class TestResultService
    {
        private readonly ITestResultRepository _testResultRepository;
        private readonly ITestResultXmlConverter _xmlConverter;
        private readonly ILogger<TestResultService> _logger;

        public TestResultService(
            ILogger<TestResultService> logger,
            ITestResultRepository testResultRepository, 
            ITestResultXmlConverter xmlConverter
            )
        {
            _logger = logger;
            _testResultRepository = testResultRepository;
            _xmlConverter = xmlConverter;
        }

        public async Task<ICollection<Models.CreateTestResultRequest>> GetAllAsync()
        {
            _logger.LogDebug("TestResultService::GetAllAsync() start!");

            return await _testResultRepository.GetAllAsyc();
        }

        public async Task<Models.CreateTestResultRequest> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestResultService::GetByIdAsync() start!");

            return await _testResultRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Models.CreateTestResultRequest result)
        {
            _logger.LogDebug("TestResultService::Create() start!");
    
            await _testResultRepository.AddAsync(result);
        }

        public async Task CreateAsync(ICollection<CreateTestResultRequest> results)
        {
            _logger.LogDebug("TestResultService::Create() start!");

            await _testResultRepository.AddAsync(results);
        }

        public async Task CreateAsync(TestSuitesXml suites)
        {
            _logger.LogDebug("TestResultService::Create() start!");

            var results = await _xmlConverter.ConvertAsync(suites);
            // At this point TestCaseId/TestRunId are not set. Depending on requirements, map by name or use defaults.
            await _testResultRepository.AddAsync(results);
        }

        public async Task<ICollection<CreateTestResultRequest>> ConvertSuitesAsync(TestSuitesXml suites, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("TestResultService::ConvertSuitesAsync() start!");

            return await _xmlConverter.ConvertAsync(suites, cancellationToken);
        }
    }
}
