using TestManagement.APP.Interfaces;

namespace TestManagement.APP.Dto.TestResult.Import
{
    public class ImportTestResultRequest
    {
        public required string Source { get; init; } = string.Empty;

        public required ITestResultParser Parser { get; init; }
    }
}
