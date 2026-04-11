using TestManagement.APP.ViewModel.Dashboard;

namespace TestManagement.APP.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetAsync();
    }
}
