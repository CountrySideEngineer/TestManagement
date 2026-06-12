using TestManagement.APP.Dto.TestResult.Import;

namespace TestManagement.APP.Services.TestExecution.Import
{
    public interface IImportTestResultService
    {
        Task<ImportTestResultResponse> ImportAsync(
            long execId,
            long execItemId,
            long testId,
            ImportTestResultRequest request,
            CancellationToken ct = default);
    }
}
