using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;

namespace TestManagement.API.Services.Xml
{
    public class TestResultXmlConverter : ITestResultXmlConverter
    {
        public Task<ICollection<TestResult>> ConvertAsync(TestSuitesXml suites, CancellationToken cancellationToken = default)
        {
            var results = new List<TestResult>();

            if (suites == null)
                return Task.FromResult((ICollection<TestResult>)results);

            foreach (var suite in suites.TestItems)
            {
                foreach (var tc in suite.TestCases)
                {
                    var result = new TestResult()
                    {
                        ActualResult = tc.Failure?.Message ?? tc.Result ?? string.Empty,
                        Status = MapStatus(tc),
                        ExecutedAt = tc.Timestamp,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                        // NOTE: TestCaseId and TestRunId must be set by caller or resolved before saving.
                    };

                    results.Add(result);
                }
            }

            return Task.FromResult((ICollection<TestResult>)results);
        }

        private TestStatus MapStatus(TestCaseXml tc)
        {
            if (tc.IsFail)
                return TestStatus.Failure;

            if (!string.IsNullOrEmpty(tc.Status))
            {
                if (tc.Status.Equals("skipped", StringComparison.OrdinalIgnoreCase))
                    return TestStatus.Skipped;

                if (tc.Status.Equals("blocked", StringComparison.OrdinalIgnoreCase))
                    return TestStatus.Blocked;

                if (tc.Status.Equals("success", StringComparison.OrdinalIgnoreCase) || tc.Status.Equals("passed", StringComparison.OrdinalIgnoreCase) || tc.Status.Equals("ok", StringComparison.OrdinalIgnoreCase))
                    return TestStatus.Success;
            }

            // fallback based on Judge/Result
            if (!string.IsNullOrEmpty(tc.Result))
            {
                if (tc.Result.Equals("OK", StringComparison.OrdinalIgnoreCase) || tc.Result.Equals("Passed", StringComparison.OrdinalIgnoreCase))
                    return TestStatus.Success;

                if (tc.Result.Equals("NG", StringComparison.OrdinalIgnoreCase) || tc.Result.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                    return TestStatus.Failure;
            }

            return TestStatus.Unknown;
        }
    }
}