using Microsoft.EntityFrameworkCore;
using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;
using TestCase = TestManagement.API.Models.TestCase;
using TestManagement.API.Services.Xml;

namespace TestManagement.API.Data.Repositories
{
    public class TestResultRepository : ITestResultRepository
    {
        private readonly TestManagementDbContext _context;
        private readonly ITestResultXmlConverter _xmlConverter;

        public TestResultRepository(TestManagementDbContext context, ITestResultXmlConverter xmlConverter)
        {
            _context = context;
            _xmlConverter = xmlConverter;
        }

        public async Task<ICollection<TestResult>> GetAllAsyc()
        {
            return await _context.TestResults
                .Include(_ => _.TestCase)
                .Include(_ => _.TestRun)
                .ToListAsync();
        }

        public async Task<TestResult> GetByIdAsync(int id)
        {
            return await _context.TestResults
                .Where(_ => _.Id == id)
                .Include(_ => _.TestCase)
                .Include(_ => _.TestRun)
                .FirstAsync();
        }

        public async Task AddAsync(TestResult result)
        {
            TestCase testCase = _context.TestCases.Find(result.TestCaseId) ?? throw new Exception();
            TestRun testRun = _context.TestRuns.Find(result.TestRunId) ?? throw new Exception();

            result.TestCase = testCase;
            result.TestRun = testRun;
            _context.TestResults.Add(result);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(ICollection<TestResult> results)
        {
            foreach (var item in results)
            {
                TestCase testCase = _context.TestCases.Find(item.TestCaseId) ?? throw new Exception();
                TestRun testRun = _context.TestRuns.Find(item.TestRunId) ?? throw new Exception();
                item.TestCase = testCase;
                item.TestRun = testRun;
            }
            _context.TestResults.AddRange(results);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(TestSuitesXml suites)
        {
            var results = await _xmlConverter.ConvertAsync(suites);

            // Try to map TestCase by title (classname + name) and TestRun by timestamp
            foreach (var item in results)
            {
                // attempt to find test case by title matching combination of classname and test name
                var possibleTitle = item.TestCase?.Title ?? string.Empty;
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
