using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.ViewModel.Environment;

namespace TestManagement.APP.Services.Environment
{
    public interface IEnvironmentService
    {
        Task<ICollection<EnvironmentViewModel>?> GetEnvironmentsAsync();
        Task<ICollection<EnvironmentViewModel>?> GetEnvironmentsByNameAsync(string name);
        Task<EnvironmentViewModel?> GetLatestEnvironmentByNameAsync(string name);
    }
}
