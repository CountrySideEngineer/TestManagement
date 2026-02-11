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

        public TestResultService(ITestResultRepository testResultRepository, ITestResultXmlConverter xmlConverter)
        {
            _testResultRepository = testResultRepository;
            _xmlConverter = xmlConverter;
        }

        public async Task<ICollection<Models.TestResult>> GetAllAsync()
        {
            return await _testResultRepository.GetAllAsyc();
        }

        public async Task<Models.TestResult> GetByIdAsync(int id)
        {
            return await _testResultRepository.GetByIdAsync(id);
        }

        public async Task Create(Models.TestResult result)
        {
            await _testResultRepository.AddAsync(result);
        }

        public async Task Create(ICollection<TestResult> results)
        {
            await _testResultRepository.AddAsync(results);
        }

        public async Task Create(TestSuitesXml suites)
        {
            var results = await _xmlConverter.ConvertAsync(suites);
            // At this point TestCaseId/TestRunId are not set. Depending on requirements, map by name or use defaults.
            await _testResultRepository.AddAsync(results);
        }

        public async Task<ICollection<TestResult>> ConvertSuitesAsync(TestSuitesXml suites, CancellationToken cancellationToken = default)
        {
            return await _xmlConverter.ConvertAsync(suites, cancellationToken);
        }
    }
}
