using TestManagement.APP.ViewModel.Dashboard;

namespace TestManagement.APP.Services
{
    /// <summary>
    /// Interface for a service that provides dashboard data and statistics.
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Retrieves dashboard view model data asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="DashboardViewModel"/>.</returns>
        Task<DashboardViewModel?> GetAsync();
    }
}
