using TestManagement.API.Features.Environment.Create;
using TestManagement.API.Features.Environment.Get;
using TestManagement.API.Features.Environment.Update;

namespace TestManagement.API.Services
{
    public interface IEnvironmentService
    {
        Task<ICollection<GetEnvironmentResponse>> GetAllAsync(CancellationToken ct);
        Task<ICollection<GetEnvironmentResponse>> GetByIdAsync(int id, CancellationToken ct);
        Task<ICollection<GetEnvironmentResponse>> GetByNameAsync(string name, CancellationToken ct);
        Task<CreateEnvironmentResponse> CreateAsync(CreateEnvironmentRequest request, CancellationToken ct);
        Task<UpdateEnvironmentResponse> UpdateAsync(UpdateEnvironmentRequest request, CancellationToken ct);
    }
}
