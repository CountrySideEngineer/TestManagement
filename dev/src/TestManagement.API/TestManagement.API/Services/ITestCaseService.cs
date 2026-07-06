using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Features.TestCases.Update;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public interface ITestCaseService
    {
        Task<ICollection<GetTestCaseResponse>> GetAllAsync(CancellationToken ct);
        Task<ICollection<GetTestCaseResponse>> GetAllLatestVersionAsync(CancellationToken ct);
        Task<ICollection<TestCaseVersion>> GetByTestLevelIdAsync(int testLevelId, CancellationToken ct);
        Task<GetTestCaseResponse> GetByTestCaseIdAsync(long testCaseId, CancellationToken ct);
        Task<TestCaseVersion?> GetByVersionIdAsync(long id, CancellationToken ct);
        Task<TestCaseVersion> GetLatestVersionByTestCaseIdAsync(long testCaseId, CancellationToken ct);
        Task<CreateTestCaseResponse> CreateVersionForExistingCaseAsync(CreateTestCaseRequest request, CancellationToken ct);
        Task<CreateTestCaseResponse> CreateAsync(CreateTestCaseRequest request, CancellationToken ct);
        Task<ICollection<CreateTestCaseResponse>> CreateAsync(ICollection<CreateTestCaseRequest> requests, CancellationToken ct);
        Task<ICollection<CreateTestCaseResponse>> CreateIfNotExistsAsync(ICollection<CreateTestCaseRequest> requests, CancellationToken ct);
        Task<UpdateTestCaseResponse> UpdateAsync(UpdateTestCaseRequest request, CancellationToken ct);
    }
}
