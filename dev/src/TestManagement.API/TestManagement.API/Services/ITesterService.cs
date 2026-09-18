using TestManagement.API.Features.Tester.Create;
using TestManagement.API.Features.Tester.Get;

namespace TestManagement.API.Services;

public interface ITesterService
{
    Task<ICollection<GetTesterResponse>> GetAllAsync(CancellationToken ct);
    Task<GetTesterResponse> GetByIdAsync(long id, CancellationToken ct);
    Task<CreateTesterResponse> CreateAsync(CreateTesterRequest request, CancellationToken ct);
}
