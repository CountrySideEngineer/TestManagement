using TestManagement.APP.Interfaces;

namespace TestManagement.APP.Dto.TestResult.Import
{
    public class ImportTestResultRequest
    {
        public required ITestResultSource Source { get; init; }

        public required ITestResultParser Parser { get; init; }
    }
}
