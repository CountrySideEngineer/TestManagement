using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;

namespace TestManagement.API.Services.Xml
{
    public class TestResultXmlConverter : ITestResultXmlConverter
    {
        public Task<ICollection<CreateTestResultRequest>> ConvertAsync(TestSuitesXml suites, CancellationToken cancellationToken = default)
        {
            var results = new List<CreateTestResultRequest>();

            if (suites == null)
                return Task.FromResult((ICollection<CreateTestResultRequest>)results);

            foreach (var suite in suites.TestItems)
            {
                foreach (var tc in suite.TestCases)
                {
                    var result = new CreateTestResultRequest()
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

            return Task.FromResult((ICollection<CreateTestResultRequest>)results);
        }

        private TestStatus MapStatus(TestCaseXml tc)
        {
            return new TestStatus();
        }
    }
}