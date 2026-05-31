using Microsoft.EntityFrameworkCore;
using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;
using TestCaseVersion = TestManagement.API.Models.TestCaseVersion;
using TestManagement.API.Services.Xml;

namespace TestManagement.API.Data.Repositories
{
    public class TestResultRepository : ITestResultRepository
    {
        private readonly TestManagementDbContext _context;
        private readonly ITestResultXmlConverter _xmlConverter;
        private readonly ILogger<TestResultRepository> _logger;

        public TestResultRepository(
            TestManagementDbContext context, 
            ITestResultXmlConverter xmlConverter,
            ILogger<TestResultRepository> logger
            )
        {
            _context = context;
            _xmlConverter = xmlConverter;
            _logger = logger;
        }

        public async Task<ICollection<TestResult>> GetAllAsyc()
        {
            _logger.LogInformation("TestResultRepository::GetAllAsyc() start!");

            return await _context.TestResults
                .Include(_ => _.TestCaseVersion)
                //.Include(_ => _.TestExecutionItem)
                .ToListAsync();
        }

        public async Task<TestResult> GetByIdAsync(int id)
        {
            _logger.LogInformation("TestResultRepository::GetByIdAsync() start!");

            return await _context.TestResults
                .Where(_ => _.Id == id)
                .Include(_ => _.TestCaseVersion)
                //.Include(_ => _.TestExecutionItem)
                .FirstAsync();
        }

        public async Task AddAsync(TestResult result)
        {
            _logger.LogInformation("TestResultRepository::AddAsync() start!");

            TestCaseVersion testCase = _context.TestCaseVersions.Find(result.TestCaseVersionId) ?? throw new Exception();
            TestExecution testExecution = _context.TestExecutions.Find(result.TestExecutionItemId) ?? throw new Exception();

            result.TestCaseVersion = testCase;
            //result.TestExecutionItem = testExecution;
            _context.TestResults.Add(result);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(ICollection<TestResult> results)
        {
            _logger.LogInformation("TestResultRepository::AddAsync() start!");

            foreach (var item in results)
            {
                TestCaseVersion testCase = _context.TestCaseVersions.Find(item.TestCaseVersionId) ?? throw new Exception();
                TestExecution testRun = _context.TestExecutions.Find(item.TestExecutionItemId) ?? throw new Exception();
                item.TestCaseVersion = testCase;
            }
            _context.TestResults.AddRange(results);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(TestSuitesXml suites)
        {
            _logger.LogInformation("TestResultRepository::AddAsync() start!");

            var results = await _xmlConverter.ConvertAsync(suites);

            // Try to map TestCase by title (classname + name) and TestRun by timestamp
            foreach (var item in results)
            {
                // attempt to find test case by title matching combination of classname and test name
                var possibleTitle = item.TestCaseVersion?.Name ?? string.Empty;
                if (string.IsNullOrEmpty(possibleTitle))
                {
                    // fallback: try to match by test case name stored in ActualResult? skip mapping here.
                }

                // leave TestCaseId/TestRunId unset; caller may handle association.
            }

            _context.TestResults.AddRange(results);
            await _context.SaveChangesAsync();
        }
    }
}
