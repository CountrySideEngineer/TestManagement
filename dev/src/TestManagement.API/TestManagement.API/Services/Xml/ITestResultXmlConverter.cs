using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;

namespace TestManagement.API.Services.Xml
{
    public interface ITestResultXmlConverter
    {
        /// <summary>
        /// Convert parsed XML test suites into a collection of domain TestResult entities.
        /// Note: caller is responsible for resolving TestCaseId/TestRunId and any persistence concerns.
        /// </summary>
        Task<ICollection<TestResult>> ConvertAsync(TestSuitesXml suites, CancellationToken cancellationToken = default);
    }
}