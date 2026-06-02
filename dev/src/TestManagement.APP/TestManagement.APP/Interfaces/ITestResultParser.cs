using TestManagement.APP.Dto.TestResult;

namespace TestManagement.APP.Interfaces
{
    public interface ITestResultParser
    {
        ICollection<ParsedTestResult> Parse(Stream stream);

        Task<ICollection<ParsedTestResult>> ParseAsync(Stream stream, CancellationToken ct = default);
    }
}
