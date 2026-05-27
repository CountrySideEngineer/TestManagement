using TestManagement.APP.Dto.TestResult.Import;

namespace TestManagement.APP.Services.TestExecution.Import
{
    public interface IImportTestResultService
    {
        Task<ImportTestResultResponse> ImportTestResultAsync(
            ImportTestResultRequest request, 
            CancellationToken ct = default);
    }
}
