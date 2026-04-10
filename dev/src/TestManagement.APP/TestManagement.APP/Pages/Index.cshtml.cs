using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Models;
using TestManagement.APP.Services;
using TestManagement.APP.ViewModel.Dashboard;

namespace TestManagement.APP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;

        private readonly IDashboardService? _dashboardService;

        public IndexModel(
            ILogger<IndexModel>? logger,
            IDashboardService? dashboardService
            ) : base()
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public DashboardViewModel? DashboardViewModel { get; set; }

        public async Task OnGetAsync()
        {
            DashboardViewModel = await _dashboardService!.GetAsync();
        }
    }
}

public record SummaryDto(int ErrorNum, int SkippedNum, int DisabledNum, int ExecutedNum);
public record TestRecordDto(DateTime ExecutedAt, string Result);
public record RequestTrendDto(string Time, int Count);
public record ErrorDto(string Time, string Message);
