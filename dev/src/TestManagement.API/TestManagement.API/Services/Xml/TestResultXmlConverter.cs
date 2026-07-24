using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;

namespace TestManagement.API.Services.Xml
{
    /// <summary>
    /// Converts test results from XML report models into domain <see cref="TestResult"/> instances.
    /// This converter maps XML test-suite and test-case structures into a collection of
    /// <see cref="TestResult"/> objects that can be persisted or further processed.
    /// </summary>
    public class TestResultXmlConverter : ITestResultXmlConverter
    {
        /// <summary>
        /// Convert the provided <see cref="TestSuitesXml"/> into a collection of <see cref="TestResult"/>.
        /// </summary>
        /// <param name="suites">The XML representation of test suites to convert. May be null.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that resolves to a collection of converted <see cref="TestResult"/> objects.</returns>
        public Task<ICollection<TestResult>> ConvertAsync(TestSuitesXml suites, CancellationToken cancellationToken = default)
        {
            var results = new List<TestResult>();

            // If no suites were provided, return an empty collection.
            if (suites == null)
                return Task.FromResult((ICollection<TestResult>)results);

            // Iterate through each suite and each test-case to create domain test results.
            foreach (var suite in suites.TestItems)
            {
                foreach (var tc in suite.TestCases)
                {
                    // Map XML test-case properties to the domain TestResult model.
                    var result = new TestResult()
                    {
                        // Use failure message when present, otherwise fall back to the result text or empty string.
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

        /// <summary>
        /// Map an individual <see cref="TestCaseXml"/> instance to a <see cref="TestStatus"/>.
        /// The mapping logic should translate XML result/state information into the domain status.
        /// </summary>
        /// <param name="tc">The XML test-case to map.</param>
        /// <returns>A <see cref="TestStatus"/> representing the mapped status.</returns>
        private TestStatus MapStatus(TestCaseXml tc)
        {
            // Placeholder implementation: callers should update mapping logic as needed.
            return new TestStatus();
        }
    }
}
