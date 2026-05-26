using TestManagement.APP.Dto.TestResult;

namespace TestManagement.APP.Interfaces
{
    public interface ITestResultParser
    {
        ICollection<ParsedTestResult> Parse(string content);

        Task<ICollection<ParsedTestResult>> ParseAsync(string content);
    }
}
