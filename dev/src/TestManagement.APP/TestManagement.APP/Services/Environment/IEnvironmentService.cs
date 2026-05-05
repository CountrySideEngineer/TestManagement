using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.ViewModel.Environment;

namespace TestManagement.APP.Services.Environment
{
    public interface IEnvironmentService
    {
        Task<ICollection<EnvironmentViewModel>?> GetEnvironmentsAsync();
    }
}
